using GameEventBus.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class OnEventUpdateScrew : EventBase
    {
        public ScrewController controller;
        public List<BoardController> boardControllers;
        public bool addNewHole;
    }
}