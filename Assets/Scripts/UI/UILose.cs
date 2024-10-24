using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UILose : UIBase
    {
        UILoseData data;
        public override void ShowUI(object param)
        {
            data = (UILoseData)param;
            base.ShowUI(param);
        }
        public void OnClickButtonClose()
        {
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

        public class UILoseData
        {
            public GameController gameController;
        }
    }
}