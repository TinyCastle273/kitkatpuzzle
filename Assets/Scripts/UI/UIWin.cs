using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyCastle
{
    public class UIWin : UIBase
    {
        [SerializeField] private Button buttonShowAds;
        UIWinData data;
        public override void ShowUI(object param)
        {
            data = (UIWinData)param;
            buttonShowAds.interactable = true;
            base.ShowUI(param);
        }

        public void OnClickButtonX2Coin()
        {
            UIManager.Instance.ShowUI(UIName.UIQuickLoading);
            TinyCastleAdsWrapper.Instance.ShowRewardedAds(() =>
            {
                UIManager.Instance.HideUI(UIName.UIQuickLoading);
                data.gameController.CoinInGame += data.gameController.CoinInGame;
                buttonShowAds.interactable = false;
            }, (error) =>
            {
                UIManager.Instance.HideUI(UIName.UIQuickLoading);
                UIManager.Instance.ShowUI(UIName.UIPopupError, new UIPopupError.Data()
                {
                    title = "Oh No!",
                    description = "Something went wrong\nPlease check your internet connection"
                });
            });
            Debug.Log("OnClickButtonX2Coin");
        }

        public void OnClickButtonNext()
        {
            data.gameController.Restart();
            HideUI();
        }

        public class UIWinData
        {
            public GameController gameController;
        }
    }
}