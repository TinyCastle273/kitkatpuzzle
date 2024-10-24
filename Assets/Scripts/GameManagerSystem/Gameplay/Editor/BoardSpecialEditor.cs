using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyCastle
{
    [CustomEditor(typeof(BoardSpecial))]
    public class BoardSpecialEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var board = (BoardSpecial)target;

            if (GUILayout.Button("Snap"))
            {
                board.Snap();
            }

            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                board.UpdateLayer();
            }
        }
    }


}