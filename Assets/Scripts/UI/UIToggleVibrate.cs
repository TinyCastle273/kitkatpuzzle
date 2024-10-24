using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyCastle
{
    public class UIToggleVibrate : ToggleBase
    {
        public override bool GetSetting()
        {
            return GameManager.Instance.UserData.vibrationEnabled;
        }

        public override void OnToggle(bool value)
        {
            GameManager.Instance.UserData.SetVibrate(value);
        }
    }
}