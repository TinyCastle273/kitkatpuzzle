using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TinyCastle
{
    public class AudioManager : MMSingleton<AudioManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip inGameAudio;
        [SerializeField] private AudioClip menuGameAudio;

        public void PlayMainMenu()
        {
            if (audioSource.clip == null)
                audioSource.clip = menuGameAudio;
            audioSource.DOFade(0, 1f).OnComplete(() =>
            {
                audioSource.clip = menuGameAudio;
                audioSource.Play();
                audioSource.DOFade(1, 1f);
            });
        }

        public void PlayInGame()
        {
            if (audioSource.clip == null)
                audioSource.clip = inGameAudio;
            audioSource.DOFade(0, 1f).OnComplete(() =>
            {
                audioSource.clip = inGameAudio;
                audioSource.Play();
                audioSource.DOFade(1, 1f);
            });
        }
    }
}