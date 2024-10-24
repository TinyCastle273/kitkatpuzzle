using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;


namespace TinyCastle
{
    public class ScrewController : MonoBehaviour, IGameInputHandler
    {
        [SerializeField] private Rigidbody2D body2D;
        public Rigidbody2D Body2D { get { return body2D; } }
        [SerializeField] private ScrewPuzzleSettings settings;
        [SerializeField] private CircleCollider2D screwCollider;
        public CircleCollider2D ScrewCollider => screwCollider;

        [SerializeField] private SpriteRenderer screwRenderer;
        [SerializeField] private Transform highLight;

        [Header("Sprite State")]
        private Vector3 cacheLocalPos;
        [SerializeField] private Sprite normalState;
        [SerializeField] private Sprite selectedState;

        [Header("SFX")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] screwSfx;

        private HoleController currentHole;

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            private set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnSelectedChanged();
                }
            }
        }

        [ContextMenu("DebugRayCast")]
        private void DebugRayCast()
        {
            List<BoardController> boardControllers = new List<BoardController>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward, 0.1f);
            for (int i = 0; i < hits.Length; i++)
            {
                BoardController board = hits[i].transform.GetComponent<BoardController>();
                if (board != null)
                    boardControllers.Add(board);
            }
            Debug.Log(gameObject.name + " --- " + boardControllers.Count);
            UpdateScrewOnMap();
        }

        public void UpdateScrewOnMap(bool addNewHole = false)
        {
            bool screwActivated = gameObject.activeSelf;
            List<BoardController> boardControllers = new List<BoardController>();
            if (screwActivated)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward, 0.1f);
                for (int i = 0; i < hits.Length; i++)
                {
                    BoardController board = hits[i].transform.GetComponent<BoardController>();
                    if (board != null)
                        boardControllers.Add(board);
                }
            }
            GameController.Instance.Bus.Publish(new OnEventUpdateScrew()
            {
                controller = this,
                boardControllers = boardControllers,
                addNewHole = addNewHole
            });

            if (addNewHole)
            {
                if (currentHole == null)
                {
                    currentHole = Instantiate(settings.PrefabHoleController);
                    currentHole.transform.position = transform.position;
                    currentHole.AddScrew(this);
                }

            }
        }

        private void OnEnable()
        {
            if (cacheLocalPos == Vector3.zero)
                cacheLocalPos = screwRenderer.transform.localPosition;
            screwRenderer.transform.localPosition = cacheLocalPos;
            highLight.gameObject.SetActive(false);
            GameController.Instance.Bus.Subscribe<OnEventSelectScrew>(OnEventSelectScrew);
            GameController.Instance.Bus.Subscribe<OnEventSelectHole>(OnEventSelectHole);
            GameController.Instance.Bus.Subscribe<OnEventGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            if (!GameController.HasInstance) return;
            GameController.Instance.Bus.Unsubscribe<OnEventSelectScrew>(OnEventSelectScrew);
            GameController.Instance.Bus.Unsubscribe<OnEventSelectHole>(OnEventSelectHole);
            GameController.Instance.Bus.Unsubscribe<OnEventGameStateChanged>(OnGameStateChanged);
        }


        public bool OnPointerDown()
        {
            if (GameController.Instance.gameplayData.GameState == GameState.SelectScrewToRemove)
            {
                RemoveScrew();
                GameController.Instance.ResumeGame();
                return true;
            }

            if (GameController.Instance.gameplayData.GameState != GameState.Playing) return false;
            IsSelected = !IsSelected;
            GameController.Instance.Bus.Publish(new OnEventSelectScrew() { controller = this });
            return true;
        }

        private void OnEventSelectScrew(OnEventSelectScrew eventData)
        {
            if (eventData.controller != this)
            {
                IsSelected = false;
            }
        }

        private void OnEventSelectHole(OnEventSelectHole eventData)
        {
            if (!IsSelected) return;
            GameController.Instance.SaveNewStep(this, currentHole);
            ScrewInTheHole(eventData.controller);
        }

        private void OnGameStateChanged(OnEventGameStateChanged eventData)
        {
            if (eventData.GameplayData.GameState == GameState.SelectScrewToRemove)
            {
                highLight.gameObject.SetActive(true);
                IsSelected = false;
                return;
            }
            highLight.gameObject.SetActive(false);
        }

        public void ScrewInTheHole(HoleController holeController)
        {
            if (currentHole != null) currentHole.RemoveScrew();
            currentHole = holeController;
            currentHole.AddScrew(this);
            transform.position = holeController.transform.position;
            IsSelected = false;
            UpdateScrewOnMap();
        }

        public void RemoveScrew()
        {
            if (currentHole != null) currentHole.RemoveScrew();
            gameObject.SetActive(false);
            UpdateScrewOnMap();
        }

        private void OnSelectedChanged()
        {
            if (screwRenderer == null) return;
            Vector3 targetPos;
            if (IsSelected)
            {
                screwRenderer.sprite = selectedState;
                targetPos = cacheLocalPos + new Vector3(0.1f, 0.2f);
            }
            else
            {
                screwRenderer.sprite = normalState;
                targetPos = cacheLocalPos;
            }

            if (GameController.Instance.gameplayData.GameState == GameState.Playing)
            {

                screwRenderer.transform.DOLocalMove(targetPos, 0.3f);
                PlayClickSFX();
            }
            else
            {
                screwRenderer.transform.localPosition = cacheLocalPos;
            }
        }

        public float ScrewRadius
        {
            get
            {
                return screwCollider.radius;
            }
        }

        public void Snap()
        {
            transform.position = settings.GetSnapPosition(transform.position);
        }

        private void PlayClickSFX()
        {
            if (screwSfx.Length > 0)
            {
                audioSource.volume = Random.Range(0.8f, 1f);
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(screwSfx[Random.Range(0, screwSfx.Length)]);
            }
        }

    }

}