using UnityEngine;
using UnityEngine.Audio;

namespace TinyCastle
{
    [CreateAssetMenu(menuName = "TinyCastle/ScrewPuzzleSettings")]
    public class ScrewPuzzleSettings : ScriptableObject
    {
        [SerializeField] private int mapHeight;
        public int MapHeight => mapHeight;

        [SerializeField] private int mapWidth;
        public int MapWidth => mapWidth;


        [SerializeField] private ScrewController prefabScrewController;

        [SerializeField] private HoleController prefabHoleController;
        public HoleController PrefabHoleController => prefabHoleController;

        [SerializeField] private HoleOnBoard prefabHoleOnBoard;
        public HoleOnBoard PrefabHoleOnBoard => prefabHoleOnBoard;

        [SerializeField] private Sprite[] spriteBoards;
        public Sprite[] SpriteBoards => spriteBoards;

        [SerializeField] private Sprite[] spriteTBoards;
        public Sprite[] SpriteTBoards => spriteTBoards;

        [SerializeField] private Sprite[] spriteCircleBoards;
        public Sprite[] SpriteCircleBoards => spriteCircleBoards;

        [SerializeField] private Sprite[] spritePlusBoards;
        public Sprite[] SpritePlusBoards => spritePlusBoards;

        [SerializeField] private Sprite[] spriteEBoards;
        public Sprite[] SpriteEBoards => spriteEBoards;

        [SerializeField] private Sprite[] spriteSmallCircleBoards;
        public Sprite[] SpriteSmallCircleBoards => spriteSmallCircleBoards;

        [SerializeField] private Sprite[] spriteHexagonBoards;
        public Sprite[] SpriteHexagonBoards => spriteHexagonBoards;

        [SerializeField] private Sprite[] spriteLBoards;
        public Sprite[] SpriteLBoards => spriteLBoards;

        [SerializeField] private UIManager prefabUIManager;
        public UIManager PrefabUIManager => prefabUIManager;

        [SerializeField] private GameManager prefabGameManager;
        public GameManager PrefabGameManager => prefabGameManager;

        [SerializeField] private float gameDuration = 120f;
        public float GameDuration => gameDuration;

        [SerializeField] private AudioMixer audioMixer;
        public AudioMixer AudioMixer => audioMixer;

        [SerializeField] private GameItems gameItems;
        public GameItems GameItems => gameItems;

        [SerializeField] private int coinsPerBoard;
        public int CoinsPerBoard => coinsPerBoard;

        public float ScrewRadius
        {
            get
            {
                return prefabScrewController.ScrewRadius;
            }
        }

        public Vector2 GetSnapPosition(Vector2 position)
        {
            Vector2 snapPosition = Vector2.zero;
            snapPosition.x = Mathf.Round(position.x / ScrewRadius) * ScrewRadius;
            snapPosition.y = Mathf.Round(position.y / ScrewRadius) * ScrewRadius;
            return snapPosition;
        }
    }
}