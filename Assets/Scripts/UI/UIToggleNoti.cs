using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UIToggleNoti : ToggleBase
    {
        public override bool GetSetting()
        {
            return GameManager.Instance.UserData.notiEnabled;
        }

        public override void OnToggle(bool value)
        {
            GameManager.Instance.UserData.SetNoti(value);
        }

    }
}