using GameEventBus.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class OnEventSaveStep : EventBase
    {
        public bool canUndo;
    }
}