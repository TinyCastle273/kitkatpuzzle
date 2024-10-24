using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyCastle
{
    [CustomEditor(typeof(ScrewController))]
    public class ScrewControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (ScrewController)target;

            if (GUILayout.Button("Snap"))
            {
                script.Snap();
            }

            EditorGUI.BeginChangeCheck();


            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("ahihi");
            }
        }
    }
}