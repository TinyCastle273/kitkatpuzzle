using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UIPause : UIBase
    {
        UIPauseData data;
        public override void ShowUI(object param)
        {
            data = (UIPauseData)param;
            base.ShowUI(param);
        }
        public void OnClickButtonClose()
        {
            data.gameController.ResumeGame();
            HideUI();
        }

        public void OnClickButtoNHome()
        {
            GameManager.Instance.LoadScene(GameConsts.SCENE_HOME, UIName.UIHome);
            HideUI();
        }

        public void OnClickButtonRetry()
        {
            data.gameController.Restart();
            HideUI();
        }

        public class UIPauseData
        {
            public GameController gameController;
        }
    }
}