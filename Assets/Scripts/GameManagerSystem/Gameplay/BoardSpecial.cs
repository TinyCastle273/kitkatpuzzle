using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TinyCastle
{
    public class BoardSpecial : BoardController
    {
        [SerializeField] private BoardType boardType;
        public override void UpdateLayer()
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Layer" + layer);
            switch (boardType)
            {
                case BoardType.T:
                    spriteRenderer.sprite = settings.SpriteTBoards[layer - 1];
                    break;
                case BoardType.Circle:
                    spriteRenderer.sprite = settings.SpriteCircleBoards[layer - 1];
                    break;
                case BoardType.Plus:
                    spriteRenderer.sprite = settings.SpritePlusBoards[layer - 1];
                    break;
                case BoardType.E:
                    spriteRenderer.sprite = settings.SpriteEBoards[layer - 1];
                    break;
                case BoardType.SmallCircle:
                    spriteRenderer.sprite = settings.SpriteSmallCircleBoards[layer - 1];
                    break;
                case BoardType.Hexagon:
                    spriteRenderer.sprite = settings.SpriteHexagonBoards[layer - 1];
                    break;
                case BoardType.L:
                    spriteRenderer.sprite = settings.SpriteLBoards[layer - 1];
                    break;
            }
            spriteRenderer.sortingOrder = layer * 10 + 1;
        }

        public override void UpdateBoardLength()
        {

        }
    }

    public enum BoardType
    {
        Default, T, Circle, Plus, E, SmallCircle, Hexagon, L
    }
}
