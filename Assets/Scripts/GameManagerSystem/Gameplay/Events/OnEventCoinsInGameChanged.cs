using GameEventBus.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class OnEventCoinsInGameChanged : EventBase
    {
        public int delta;
        public int currentValue;
        public Vector3 moveFrom;
    }
}