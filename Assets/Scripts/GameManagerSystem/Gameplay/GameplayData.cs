using GameEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    [Serializable]
    public class GameplayData
    {
        public EventBus eventBus;
        public GameplayData(EventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        private float _currentTime;
        public float CurrentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = Mathf.Max(value, 0);
                    eventBus.Publish(new OnEventGameTimeChanged()
                    {
                        GameplayData = this
                    });
                }
            }
        }
        private GameState _gameState;
        public GameState GameState
        {
            get
            {
                return _gameState;
            }

            set
            {
                if (_gameState != value)
                {
                    _gameState = value;
                    eventBus.Publish(new OnEventGameStateChanged()
                    {
                        GameplayData = this
                    });
                }

            }

        }
    }

}
