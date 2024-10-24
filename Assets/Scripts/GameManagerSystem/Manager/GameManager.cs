using GameEventBus;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TinyCastle
{
    public class GameManager : MMSingleton<GameManager>
    {
        public static EventBus Bus = new EventBus();
        [SerializeField] private ScrewPuzzleSettings settings;
        private Action<OnEventGameSettingsChanged> onEventGameSettingsChanged;
        private UserData _userData;
        public UserData UserData
        {
            get
            {
                if (_userData == null)
                    LoadUserData();
                return _userData;
            }

        }
        protected override void Awake()
        {
            base.Awake();
            LoadUserData();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            OnGameSettingsChanged();
        }

        private void OnEnable()
        {
            onEventGameSettingsChanged = OnEventGameSettingsChanged;
            Bus.Subscribe(onEventGameSettingsChanged);
        }

        private void OnDisable()
        {
            Bus.Unsubscribe(onEventGameSettingsChanged);
        }

        public void OnEventGameSettingsChanged(OnEventGameSettingsChanged eventData)
        {
            OnGameSettingsChanged();
        }

        public void LoadUserData()
        {
            string jsonData = PlayerPrefs.GetString(GameConsts.USER_DATA_PLAYERPREF, "");
            _userData = new UserData();
            JsonUtility.FromJsonOverwrite(jsonData, _userData);
            SaveUserData();
        }

        public void OnGameSettingsChanged()
        {
            settings.AudioMixer.SetFloat("SFX", UserData.sfxEnabled ? 0 : -80);
            settings.AudioMixer.SetFloat("Music", UserData.musicEnabled ? 0 : -80);
        }

        public void SaveUserData()
        {
            string jsonData = JsonUtility.ToJson(UserData);
            PlayerPrefs.SetString(GameConsts.USER_DATA_PLAYERPREF, jsonData);
        }

        public void LoadScene(string sceneName, string showUI = "")
        {
            Time.timeScale = 1;
            StartCoroutine(LoadScene());
            IEnumerator LoadScene()
            {
                //Fadein UILoading
                UILoading uiLoading = (UILoading)UIManager.Instance.ShowUI(UIName.UILoading, new UILoading.UILoadingData
                {
                    delayShowUIDuration = GameConsts.FADEIN_LOADING_DURATION
                });
                uiLoading.UpdateLoadingBar(0);
                yield return new WaitForSeconds(GameConsts.FADEIN_LOADING_DURATION);

                //Show UILoading Completed
                UIManager.Instance.HideUI(UIName.UIGameplay);
                UIManager.Instance.HideUI(UIName.UIHome);

                //Fake Loading
                uiLoading.UpdateLoadingBar(0.5f, 2f);

                //Loading Scene
                var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
                while (!async.isDone)
                {
                    yield return null;
                }

                //Load Scene Completed
                if (!string.IsNullOrEmpty(showUI))
                    UIManager.Instance.ShowUI(showUI);

                //Fake Loading
                uiLoading.UpdateLoadingBar(1f, GameConsts.SMOOTH_LOADING_DURATION);
                yield return new WaitForSeconds(GameConsts.SMOOTH_LOADING_DURATION);
                UIManager.Instance.HideUI(UIName.UILoading);
                if (AudioManager.HasInstance)
                {
                    if (sceneName == GameConsts.SCENE_HOME)
                        AudioManager.Instance.PlayMainMenu();
                    else if (sceneName == GameConsts.SCENE_GAMEPLAY)
                        AudioManager.Instance.PlayInGame();
                }
            }
        }
    }
}