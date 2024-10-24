using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEventBus.Events;
namespace TinyCastle
{
    public class OnEventSelectHole : EventBase
    {
        public HoleController controller;
    }
}