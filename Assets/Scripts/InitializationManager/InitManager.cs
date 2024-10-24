using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
namespace TinyCastle
{

    public class InitManager : MMSingleton<InitManager>
    {
        public void Start()
        {
            StartCoroutine(RoutineInit());
        }

        IEnumerator RoutineInit()
        {
            UIManager.Instance.ShowUI(UIName.UILoading, new UILoading.UILoadingData { delayShowUIDuration = 0 });
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.LoadScene(GameConsts.SCENE_HOME, UIName.UIHome);
        }
    }
}