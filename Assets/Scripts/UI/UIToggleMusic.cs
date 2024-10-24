using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyCastle
{
    public class UIToggleMusic : ToggleBase
    {
        public override bool GetSetting()
        {
            return GameManager.Instance.UserData.musicEnabled;
        }

        public override void OnToggle(bool value)
        {
            GameManager.Instance.UserData.SetMusic(value);
        }
    }
}