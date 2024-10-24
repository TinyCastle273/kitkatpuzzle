using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TinyCastle
{
    [CreateAssetMenu(menuName = "TinyCastle/ScrewPuzzleGameItems")]
    public class GameItems : ScriptableObject
    {
        public PuzzleItem[] items;

        public PuzzleItem GetItemByID(string id)
        {
            foreach (var item in items)
            {
                if (item.id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            return null;
        }
    }

    [Serializable]
    public class PuzzleItem
    {
        public string name;
        public string id;
        public string description;
        public Sprite icon;
        public int price;
    }


}