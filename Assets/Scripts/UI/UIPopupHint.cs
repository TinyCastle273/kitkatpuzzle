using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TinyCastle
{
    public class UIPopupHint : UIBase
    {
        private Data data;
        [SerializeField] private Image hintIcon;
        [SerializeField] private TextMeshProUGUI detailText;
        [SerializeField] private TextMeshProUGUI coinstext;
        public override void ShowUI(object param)
        {
            data = (Data)param;
            hintIcon.sprite = data.item.icon;
            detailText.text = data.item.description;
            coinstext.text = $"{data.item.price}<sprite=0>";
            base.ShowUI(param);
        }

        public void OnButtonClickUseCoins()
        {
            if (GameManager.Instance.UserData.coins >= data.item.price)
            {
                GameManager.Instance.UserData.RemoveCoins(data.item.price);
                data.onHintUsed?.Invoke();
                HideUI();
            }
            else
            {
                UIManager.Instance.ShowUI(UIName.UIPopupError, new UIPopupError.Data
                {
                    title = "Oh No!",
                    description = "Not enough coins!"
                });
            }
        }

        public void OnButtonClickUseRewardAds()
        {
            UIManager.Instance.ShowUI(UIName.UIQuickLoading);
            TinyCastleAdsWrapper.Instance.ShowRewardedAds(
                () =>
                {
                    UIManager.Instance.HideUI(UIName.UIQuickLoading);
                    data.onHintUsed?.Invoke();
                    HideUI();
                },
                (error) =>
                {
                    UIManager.Instance.HideUI(UIName.UIQuickLoading);
                    UIManager.Instance.ShowUI(UIName.UIPopupError, new UIPopupError.Data()
                    {
                        title = "Oh No!",
                        description = "Something went wrong\nPlease check your internet connection"
                    });

                }
            );
        }

        public void OnButtonCloseClicked()
        {
            data.onClosePopup?.Invoke();
            HideUI();
        }

        [Serializable]
        public class Data
        {
            public PuzzleItem item;
            public Action onHintUsed;
            public Action onClosePopup;
        }
    }
}