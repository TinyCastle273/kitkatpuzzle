using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyCastle
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleSFX : ToggleBase
    {
        public override bool GetSetting()
        {
            return GameManager.Instance.UserData.sfxEnabled;
        }

        public override void OnToggle(bool value)
        {
            GameManager.Instance.UserData.SetSFX(value);
        }
    }
}