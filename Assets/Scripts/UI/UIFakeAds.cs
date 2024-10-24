using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UIFakeAds : UIBase
    {
        UIFakeAdsData data;
        public override void ShowUI(object param)
        {
            data = (UIFakeAdsData)param;
            base.ShowUI(param);
        }

        public void OnClickButtonClose()
        {
            data.OnShowAdsSuccessful?.Invoke();
            HideUI();
        }

        public class UIFakeAdsData
        {
            public Action OnShowAdsSuccessful;
        }
    }
}