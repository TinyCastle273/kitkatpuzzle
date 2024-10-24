using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyCastle
{
    [CustomEditor(typeof(HoleOnBoard))]
    public class HoleOnBoardControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (HoleOnBoard)target;

            if (GUILayout.Button("Snap"))
            {
                script.Snap();
            }

            base.OnInspectorGUI();

        }
    }
}
