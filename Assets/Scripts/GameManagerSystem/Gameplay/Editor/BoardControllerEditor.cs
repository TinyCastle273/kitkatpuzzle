using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyCastle
{
    [CustomEditor(typeof(BoardController))]
    public class BoardControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var board = (BoardController)target;

            if (GUILayout.Button("Snap"))
            {
                board.Snap();
            }

            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                board.UpdateBoardLength();
                board.UpdateLayer();
            }
        }
    }


}