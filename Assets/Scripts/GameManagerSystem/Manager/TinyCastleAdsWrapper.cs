using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System;

namespace TinyCastle
{
    public class TinyCastleAdsWrapper : MMSingleton<TinyCastleAdsWrapper>
    {
        public void ShowAds()
        {

        }

        public void ShowRewardedAds(Action callbackOnSuccess, Action<string> callbackOnFailed)
        {
#if UNITY_EDITOR
            if (UnityEngine.Random.Range(0, 3) == 0)
            {
                callbackOnFailed?.Invoke("Error!");
                return;
            }
#endif
            callbackOnSuccess?.Invoke();
        }
    }
}