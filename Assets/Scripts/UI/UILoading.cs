using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.Tools;
using System;

namespace TinyCastle
{
    public class UILoading : UIBase
    {
        [SerializeField] private MMFader mFader;
        public override void ShowUI(object param)
        {
            var data = (UILoadingData)param;
            if (data.delayShowUIDuration == 0)
                mFader.OnMMEvent(new MMFadeStopEvent());
            else
                mFader.OnMMEvent(new MMFadeInEvent(data.delayShowUIDuration, MMTweenType.DefaultEaseInCubic));
            base.ShowUI(param);
        }

        public override void HideUI()
        {
            mFader.OnMMEvent(new MMFadeOutEvent(0.5f, MMTweenType.DefaultEaseInCubic));
            DOVirtual.DelayedCall(0.5f, () =>
            {
                base.HideUI();
            });

        }
        [SerializeField] private Slider sliderLoadingBar;
        public void UpdateLoadingBar(float percent, float duration = 0)
        {
            sliderLoadingBar.DOKill();
            if (duration > 0)
            {
                sliderLoadingBar.DOValue(percent, duration);
            }
            else
            {
                sliderLoadingBar.value = percent;
            }
        }

        [Serializable]
        public class UILoadingData
        {
            public float delayShowUIDuration;
        }
    }
}