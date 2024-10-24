using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

namespace TinyCastle
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textCoins;
        [SerializeField] private Transform[] coins;

        private Action<OnEventCoinsInGameChanged> onEventCoinsInGameChanged;
        private int coinInGame;
        private int currentCoinInGameValue;
        private float timeInterval = 0.2f;
        private float currentTimeInterval;
        private int baseCoinOnStart;
        private Queue<Vector3> listEffectStartPosition;
        private void OnEnable()
        {
            listEffectStartPosition = new Queue<Vector3>();
            coinInGame = 0;
            currentCoinInGameValue = 0;
            baseCoinOnStart = GameManager.Instance.UserData.coins;
            UpdateValue();
            ResetCoinEffect();
            GameManager.Bus.Subscribe<OnEventCoinsChanged>(UpdateValue);
            if (GameController.HasInstance)
            {
                onEventCoinsInGameChanged = OnEventCoinsInGameChanged;
                GameController.Instance.Bus.Subscribe(onEventCoinsInGameChanged);
            }
        }

        private void ResetCoinEffect()
        {
            foreach (Transform child in coins)
            {
                child.transform.DOKill();
                child.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (GameController.HasInstance)
            {
                GameController.Instance.Bus.Unsubscribe(onEventCoinsInGameChanged);
            }
            GameManager.Bus.Unsubscribe<OnEventCoinsChanged>(UpdateValue);
        }

        private void OnEventCoinsInGameChanged(OnEventCoinsInGameChanged eventData)
        {
            for (int i = 0; i < eventData.delta; i++)
            {
                listEffectStartPosition.Enqueue(eventData.moveFrom);
            }
            coinInGame = eventData.currentValue;
        }

        private void Update()
        {
            if (currentTimeInterval > 0)
            {
                currentTimeInterval -= Time.deltaTime;
                return;
            }
            if (listEffectStartPosition.Count > 0)
            {
                PlayMoveCoin();
            }
        }

        private void PlayMoveCoin()
        {
            Transform coin = GetCoin();
            if (coin == null) return;

            currentTimeInterval = timeInterval;
            var startPosition = listEffectStartPosition.Dequeue();
            coin.gameObject.SetActive(true);
            coin.position = startPosition;
            coin.transform.DOMove(transform.position, 1f).OnComplete(() =>
            {
                textCoins.transform.DOKill();
                currentCoinInGameValue++;
                textCoins.text = (baseCoinOnStart + currentCoinInGameValue).ToString();

                textCoins.transform.DOShakeScale(0.5f, 0.3f, 10).OnKill(() =>
                {
                    textCoins.transform.localScale = Vector3.one;
                });
                coin.gameObject.SetActive(false);
            }).SetEase(Ease.OutSine);
        }

        private void UpdateValue(OnEventCoinsChanged value = null)
        {
            if (listEffectStartPosition.Count > 0)
                return;
            baseCoinOnStart = GameManager.Instance.UserData.coins;
            textCoins.text = (GameManager.Instance.UserData.coins + coinInGame).ToString();
        }

        public void OnAddCoinsButtonClicked()
        {
#if UNITY_EDITOR
            GameManager.Instance.UserData.AddCoins(1);
#endif
        }
        private Transform GetCoin()
        {
            foreach (var child in coins)
            {
                if (!child.gameObject.activeSelf)
                {
                    return child;
                }
            }
            return null;
        }

    }
}