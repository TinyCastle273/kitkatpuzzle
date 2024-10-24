using CandyCoded.HapticFeedback;
using DG.Tweening;
using GameEventBus;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace TinyCastle
{
    public class GameController : MMSingleton<GameController>
    {
        [SerializeField] private ScrewPuzzleSettings settings;
        [SerializeField] private LevelGenerator[] levelGenerators;
        [SerializeField] private Transform effectWin;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip sfxRemoveBoard;
        [SerializeField] private ParticleSystem effectBoardCompleted;
        private int coinInGame;
        private Vector3 lastBoardPosition;
        public int CoinInGame
        {
            get { return coinInGame; }
            set
            {
                if (coinInGame != value)
                {
                    int delta = value - coinInGame;
                    coinInGame = value;
                    Bus.Publish(new OnEventCoinsInGameChanged()
                    {
                        delta = Mathf.Max(delta, 0),
                        currentValue = coinInGame,
                        moveFrom = Camera.main.WorldToScreenPoint(lastBoardPosition)
                    });
                }
            }
        }
        private SaveStep _saveStep;
        public SaveStep saveStep
        {
            get
            {
                return _saveStep;
            }
            private set
            {
                _saveStep = value;
                Bus.Publish<OnEventSaveStep>(new OnEventSaveStep()
                {
                    canUndo = _saveStep != null
                }); ;
            }
        }

        private LevelGenerator currentLevelGenerator;
        public EventBus Bus = new EventBus();
        public GameplayData gameplayData
        {
            get; private set;
        }

        [SerializeField] private int currentTestLevel;

        protected override void Awake()
        {
            base.Awake();
            if (!GameManager.HasInstance)
                Instantiate(settings.PrefabGameManager);

            gameplayData = new GameplayData(Bus);
            gameplayData.GameState = GameState.Ready;
            gameplayData.CurrentTime = settings.GameDuration;
            CoinInGame = 0;
            if (GameManager.Instance.UserData.currentLevel >= levelGenerators.Length)
            {
                GameManager.Instance.UserData.UpdateUserLevel(0);
            }
            effectWin.gameObject.SetActive(false);
            InitLevel(GameManager.Instance.UserData.currentLevel);
            if (!UIManager.HasInstance)
                Instantiate(settings.PrefabUIManager);


            UIManager.Instance.ShowUI(UIName.UIGameplay, new UIGameplay.UIGameplayData
            {
                level = GameManager.Instance.UserData.currentLevel,
                gameController = this,
            });
            saveStep = null;
        }

        private void Start()
        {
            gameplayData.GameState = GameState.Playing;
        }

        private void Update()
        {
            if (gameplayData.GameState == GameState.Playing)
            {
                gameplayData.CurrentTime -= Time.deltaTime;
            }
            CheckLose();
            CheckWin();
        }

        public void PauseGame()
        {
            gameplayData.GameState = GameState.Paused;
            //Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            gameplayData.GameState = GameState.Playing;
            //Time.timeScale = 1;
        }

        [ContextMenu("ForceWin")]
        public void ForceWin()
        {
            foreach (var controller in currentLevelGenerator.ScrewControllers)
            {
                controller.RemoveScrew();
            }
        }

        private void CheckLose()
        {
            if (gameplayData.GameState != GameState.Playing)
                return;
            if (gameplayData.CurrentTime <= 0)
            {
                UIManager.Instance.ShowUI(UIName.UILose, new UILose.UILoseData()
                {
                    gameController = this
                });
                gameplayData.GameState = GameState.Lose;
            }

        }
        private void CheckWin()
        {
            if (gameplayData.GameState != GameState.Playing)
                return;
            if (currentLevelGenerator == null)
                return;
            if (currentLevelGenerator.BoardControllers == null)
                return;
            bool haveActiveBoardOnGame = false;
            foreach (var controller in currentLevelGenerator.BoardControllers)
            {
                if (!controller.gameObject.activeSelf) continue;
                if (controller.transform.position.y >= -15f)
                {
                    haveActiveBoardOnGame = true;
                    continue;
                }
                RemoveBoard(controller);
            }
            if (haveActiveBoardOnGame) return;
            effectWin.gameObject.SetActive(true);
            GameManager.Instance.UserData.AddCoins(CoinInGame);

            DOVirtual.DelayedCall(1f, () =>
            {
                UIManager.Instance.ShowUI(UIName.UIWin, new UIWin.UIWinData()
                {
                    gameController = this
                });
            });
            int nextLevel = GameManager.Instance.UserData.currentLevel + 1;
            GameManager.Instance.UserData.UpdateUserLevel(nextLevel);
            gameplayData.GameState = GameState.Win;
        }

        private void RemoveBoard(BoardController controller)
        {
            PlaySFXRemoveBoard();

            if (GameManager.Instance.UserData.vibrationEnabled)
                HapticFeedback.HeavyFeedback();

            effectBoardCompleted.transform.position =
                new Vector3(controller.transform.position.x, controller.transform.position.y, effectBoardCompleted.transform.position.z);
            effectBoardCompleted.Play();
            controller.gameObject.SetActive(false);
            lastBoardPosition = controller.transform.position;
            CoinInGame += settings.CoinsPerBoard;
        }

        private void PlaySFXRemoveBoard()
        {
            audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(sfxRemoveBoard);
        }

        private void InitLevel(int level)
        {
            currentLevelGenerator = Instantiate(levelGenerators[level]);
        }

        public void Restart()
        {
            UIManager.Instance.HideUI(UIName.UIGameplay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void UseHintScrew()
        {
            Bus.Publish(new OnEventSelectScrew() { controller = null });//UnSelect all Screws
            gameplayData.GameState = GameState.SelectScrewToRemove;
        }

        public void UseHintUndo()
        {
            Bus.Publish(new OnEventSelectScrew() { controller = null });//UnSelect all Screws
            ResumeGame();
            if (saveStep == null) return;
            if (saveStep.currentScrew == null) return;
            if (saveStep.lastHole == null) return;

            foreach (var board in saveStep.saveBoardPositions)
            {
                board.boardController.transform.position = board.position;
                board.boardController.transform.rotation = board.rotation;
                board.boardController.RemoveForce();
            }

            StartCoroutine(DelayCheckCollider());
            IEnumerator DelayCheckCollider()
            {
                yield return new WaitForFixedUpdate();
                saveStep.currentScrew.ScrewInTheHole(saveStep.lastHole);
                foreach (var board in saveStep.saveBoardPositions)
                {
                    board.boardController.UpdateBoardState();
                }
                saveStep = null;
            }

        }

        public void UseHintRestart()
        {
            Restart();
        }

        public void UseHintKnife()
        {
            Bus.Publish(new OnEventSelectScrew() { controller = null });//UnSelect all Screws
            gameplayData.GameState = GameState.SelectBoardToRemove;
        }

        public void SaveNewStep(ScrewController currentScrew, HoleController lastHole)
        {
            List<SaveBoardPosition> saveBoardPositions = new List<SaveBoardPosition>();
            foreach (var item in currentLevelGenerator.BoardControllers)
            {
                if (!item.gameObject.activeSelf) continue;
                var boardPosition = new SaveBoardPosition()
                {
                    boardController = item,
                    position = item.transform.position,
                    rotation = item.transform.rotation
                };
                saveBoardPositions.Add(boardPosition);
            }

            saveStep = new SaveStep()
            {
                currentScrew = currentScrew,
                lastHole = lastHole,
                saveBoardPositions = saveBoardPositions
            };
        }

        [ContextMenu("Test Level")]
        public void TestLevel()
        {
            GameManager.Instance.UserData.UpdateUserLevel(currentTestLevel);
            Restart();
        }
    }

    [Serializable]
    public enum GameState
    {
        Ready,
        Playing,
        Paused,
        Win,
        Lose,
        SelectScrewToRemove,
        SelectBoardToRemove
    }

    [Serializable]
    public class SaveStep
    {
        public HoleController lastHole;
        public ScrewController currentScrew;
        public List<SaveBoardPosition> saveBoardPositions;
    }

    [Serializable]
    public class SaveBoardPosition
    {
        public BoardController boardController;
        public Vector3 position;
        public Quaternion rotation;
    }

}