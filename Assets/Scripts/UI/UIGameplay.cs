using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using MoreMountains.Tools;

namespace TinyCastle
{
    public class UIGameplay : UIBase
    {
        [SerializeField] private TextMeshProUGUI textTime;
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private GameItems gameItems;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip sfxOnButtonHintClicked;
        [SerializeField] private Button buttonHintUndo;
        [SerializeField] private MMFader mFader;
        private UIGameplayData data;
        private Action<OnEventGameTimeChanged> onEventGameTimeChanged;
        private Action<OnEventSaveStep> onEventSaveStep;
        public override void ShowUI(object param)
        {
            mFader.OnMMEvent(new MMFadeInEvent(0.5f, MMTweenType.DefaultEaseInCubic));
            data = (UIGameplayData)param;
            buttonHintUndo.interactable = false;
            textLevel.text = $"Level {data.level + 1}";
            base.ShowUI(param);
        }

        public void OnClickPauseButton()
        {
            data.gameController.PauseGame();
            UIManager.Instance.ShowUI(UIName.UIPause, new UIPause.UIPauseData()
            {
                gameController = data.gameController
            });
        }

        private void OnEnable()
        {
            onEventGameTimeChanged = OnEventGameTimeChanged;
            onEventSaveStep = OnEventSaveStep;
            if (!GameController.HasInstance) return;
            GameController.Instance.Bus.Subscribe(onEventGameTimeChanged);
            GameController.Instance.Bus.Subscribe(onEventSaveStep);
        }

        private void OnDisable()
        {
            if (!GameController.HasInstance) return;
            GameController.Instance.Bus.Unsubscribe(onEventGameTimeChanged);
            GameController.Instance.Bus.Unsubscribe(onEventSaveStep);
        }
        private void OnEventGameTimeChanged(OnEventGameTimeChanged data)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(data.GameplayData.CurrentTime);
            textTime.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }

        private void OnEventSaveStep(OnEventSaveStep data)
        {
            buttonHintUndo.interactable = data.canUndo;
        }

        public void OnScrewItemButtonClicked()
        {
            data.gameController.PauseGame();
            audioSource.PlayOneShot(sfxOnButtonHintClicked);
            UIManager.Instance.ShowUI(UIName.UIPopupHint, new UIPopupHint.Data()
            {
                item = gameItems.GetItemByID("screw"),
                onHintUsed = () =>
                {
                    data.gameController.UseHintScrew();
                },
                onClosePopup = () =>
                {
                    data.gameController.ResumeGame();
                }
            });
        }

        public void OnRestartItemButtonClicked()
        {
            data.gameController.PauseGame();
            audioSource.PlayOneShot(sfxOnButtonHintClicked);
            UIManager.Instance.ShowUI(UIName.UIPopupHint, new UIPopupHint.Data()
            {
                item = gameItems.GetItemByID("restart"),
                onHintUsed = () =>
                {
                    data.gameController.UseHintRestart();
                },
                onClosePopup = () =>
                {
                    data.gameController.ResumeGame();
                }
            });
        }

        public void OnUnDoItemButtonClicked()
        {
            data.gameController.PauseGame();
            audioSource.PlayOneShot(sfxOnButtonHintClicked);
            UIManager.Instance.ShowUI(UIName.UIPopupHint, new UIPopupHint.Data()
            {
                item = gameItems.GetItemByID("undo"),
                onHintUsed = () =>
                {
                    data.gameController.UseHintUndo();
                },
                onClosePopup = () =>
                {
                    data.gameController.ResumeGame();
                }
            });
        }
        public void OnKnifeItemButtonClicked()
        {
            data.gameController.PauseGame();
            audioSource.PlayOneShot(sfxOnButtonHintClicked);
            UIManager.Instance.ShowUI(UIName.UIPopupHint, new UIPopupHint.Data()
            {
                item = gameItems.GetItemByID("knife"),
                onHintUsed = () =>
                {
                    data.gameController.UseHintKnife();
                },
                onClosePopup = () =>
                {
                    data.gameController.ResumeGame();
                }
            });
        }


        [SerializeField]
        public class UIGameplayData
        {
            public int level;
            public GameController gameController;
        }
    }
}