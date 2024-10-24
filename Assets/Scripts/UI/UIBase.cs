using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TinyCastle
{

    public abstract class UIBase : MonoBehaviour
    {
        public virtual void ShowUI(object param)
        {
            gameObject.SetActive(true);
        }

        public virtual void HideUI()
        {
            gameObject.SetActive(false);
        }
    }
}