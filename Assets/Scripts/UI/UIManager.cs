using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class UIManager : MMSingleton<UIManager>
    {
        [SerializeField] private List<UIBase> _prefabUI;
        [SerializeField] private Canvas canvas;
        private Dictionary<string, UIBase> activeUIs = new Dictionary<string, UIBase>();
        public UIBase ShowUI(string uiName, object param = default)
        {
            if (activeUIs.ContainsKey(uiName))
            {
                activeUIs[uiName].ShowUI(param);
                return activeUIs[uiName];
            }

            UIBase uiBase = _prefabUI.Find(p => p.name == uiName);
            if (uiBase == null)
            {
                Debug.LogError(uiName + " does not exist! Please check!");
                return null;
            }

            uiBase = Instantiate(uiBase, canvas.transform);
            activeUIs.Add(uiName, uiBase);
            uiBase.ShowUI(param);
            return uiBase;
        }

        public void HideUI(string uiName)
        {
            if (activeUIs.ContainsKey(uiName))
            {
                activeUIs[uiName].HideUI();
            }
        }
    }
}