using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TinyCastle
{
    public class UIHome : UIBase
    {
        public void OnClickButtonPlay()
        {
            GameManager.Instance.LoadScene(GameConsts.SCENE_GAMEPLAY);
        }

        public void OnButtonCreditsClicked()
        {
            UIManager.Instance.ShowUI(UIName.UICredits);
        }

        public void OnButtonSettingsClicked()
        {
            UIManager.Instance.ShowUI(UIName.UISettings);
        }

        public override void ShowUI(object param)
        {
            base.ShowUI(param);
        }
    }
}