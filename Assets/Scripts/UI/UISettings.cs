using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UISettings : UIBase
    {
        public void OnButtonCloseClicked()
        {
            HideUI();
        }

        public void OnButtonFeebackClicked()
        {
            Debug.Log("OnButtonFeebackClicked");
        }

        public void OnButtonRateUsClicked()
        {
            Debug.Log("OnButtonRateUsClicked");
        }

    }
}