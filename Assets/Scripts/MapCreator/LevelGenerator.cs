using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TinyCastle
{

    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private ScrewPuzzleSettings settings;
        private ScrewController[] screwControllers;
        public ScrewController[] ScrewControllers => screwControllers;

        private BoardController[] boardControllers;
        public BoardController[] BoardControllers => boardControllers;


        private void Start()
        {
            screwControllers = GetComponentsInChildren<ScrewController>();
            foreach (var screw in screwControllers)
            {
                screw.UpdateScrewOnMap(true);
            }
            boardControllers = GetComponentsInChildren<BoardController>();
            foreach (var board in boardControllers)
            {
                board.UpdateBoardState();
            }
        }

        [ContextMenu("Update Layer")]
        public void UpdateLayer()
        {
            boardControllers = GetComponentsInChildren<BoardController>();
            foreach (var board in boardControllers)
            {
                board.UpdateLayer();
            }
        }

        private Vector2 offset
        {
            get
            {
                Vector2 offset = Vector2.zero;
                offset.x = -(settings.MapWidth - 1) * settings.ScrewRadius * 0.5f;
                offset.y = -(settings.MapHeight - 1) * settings.ScrewRadius * 0.5f;
                return offset;
            }
        }

#if UNITY_EDITOR
        [SerializeField] private bool showGrid;
        private void OnDrawGizmos()
        {
            if (!showGrid) return;
            Gizmos.color = Color.green;
            float screwRadius = settings.ScrewRadius;
            float screwDiameter = screwRadius * 2f;
            for (int x = 0; x < settings.MapWidth; x++)
            {
                for (int y = 0; y < settings.MapHeight; y++)
                {
                    Vector2 pos = offset + new Vector2(x * (screwRadius), y * (screwRadius));
                    Gizmos.DrawWireSphere(pos, screwRadius * 0.2f);
                }
            }
        }
#endif
    }




}