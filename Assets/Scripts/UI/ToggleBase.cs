using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyCastle
{
    [RequireComponent(typeof(Toggle))]
    public abstract class ToggleBase : MonoBehaviour
    {
        private Toggle m_Toggle;
        public abstract bool GetSetting();
        private void OnEnable()
        {
            if (m_Toggle == null)
                m_Toggle = GetComponent<Toggle>();
            m_Toggle.onValueChanged.RemoveListener(OnToggle);
            m_Toggle.isOn = GetSetting();

            m_Toggle.onValueChanged.AddListener(OnToggle);
        }

        public abstract void OnToggle(bool value);
    }
}