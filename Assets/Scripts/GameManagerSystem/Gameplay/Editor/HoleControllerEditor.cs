using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TinyCastle
{
    [CustomEditor(typeof(HoleController))]
    public class HoleControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (HoleController)target;
            if (GUILayout.Button("Snap"))
            {
                script.Snap();
            }
            base.OnInspectorGUI();

        }

    }
}