using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class HoleWithScrew : MonoBehaviour
    {
        [SerializeField] private ScrewPuzzleSettings settings;
        [SerializeField] private ScrewController screwController;
        public ScrewController ScrewController => screwController;
        [SerializeField] private HoleController holeController;
        private void OnEnable()
        {
            screwController.ScrewInTheHole(holeController);
        }

        public void Snap()
        {
            transform.position = settings.GetSnapPosition(transform.position);
        }
    }
}