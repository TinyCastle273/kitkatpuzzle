using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TinyCastle
{
    public class HoleOnBoard : MonoBehaviour
    {
        [SerializeField] private ScrewPuzzleSettings settings;
        [SerializeField] private SpriteMask mask;

        public void SetLayer(int layer)
        {
            mask.isCustomRangeActive = true;
            mask.frontSortingOrder = layer * 10 + 9;
            mask.backSortingOrder = layer * 10;
        }
        public void Snap()
        {
            transform.position = settings.GetSnapPosition(transform.position);
        }


    }
}