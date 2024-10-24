using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TinyCastle
{
    [CustomEditor(typeof(HoleWithScrew))]
    public class HoleWithScrewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (HoleWithScrew)target;
            if (GUILayout.Button("Snap"))
            {
                script.Snap();
            }
            base.OnInspectorGUI();
        }
    }
}