using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TinyCastle
{
    public class UIPopupError : UIBase
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        public override void ShowUI(object param)
        {
            var data = (Data)param;
            titleText.text = data.title;
            descriptionText.text = data.description;
            base.ShowUI(param);
        }

        public class Data
        {
            public string title;
            public string description;
        }
    }
}