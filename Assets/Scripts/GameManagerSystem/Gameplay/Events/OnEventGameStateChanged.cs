using GameEventBus.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class OnEventGameStateChanged : EventBase
    {
        public GameplayData GameplayData;
    }
}