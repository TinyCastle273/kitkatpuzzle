using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    [Serializable]
    public class UserData
    {
        public int coins = 0;
        public int currentLevel = 0;

        #region Settings
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool vibrationEnabled = true;
        public bool notiEnabled = true;
        #endregion
        public void UpdateUserLevel(int level)
        {
            if (currentLevel == level) return;
            currentLevel = level;
            GameManager.Instance.SaveUserData();
        }
        public void AddCoins(int value)
        {
            coins += value;
            GameManager.Bus.Publish(new OnEventCoinsChanged());
            GameManager.Instance.SaveUserData();
        }

        public void RemoveCoins(int value)
        {
            coins -= value;
            coins = Mathf.Max(coins, 0);
            GameManager.Bus.Publish(new OnEventCoinsChanged());
            GameManager.Instance.SaveUserData();
        }

        public void SetMusic(bool value)
        {
            musicEnabled = value;
            GameManager.Bus.Publish(new OnEventGameSettingsChanged());
            GameManager.Instance.SaveUserData();
        }

        public void SetSFX(bool value)
        {
            sfxEnabled = value;
            GameManager.Bus.Publish(new OnEventGameSettingsChanged());
            GameManager.Instance.SaveUserData();
        }

        public void SetVibrate(bool value)
        {
            vibrationEnabled = value;
            GameManager.Bus.Publish(new OnEventGameSettingsChanged());
            GameManager.Instance.SaveUserData();
        }

        public void SetNoti(bool value)
        {
            notiEnabled = value;
            GameManager.Bus.Publish(new OnEventGameSettingsChanged());
            GameManager.Instance.SaveUserData();
        }
    }
}