using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class GameplayAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] screwSfx;
        private void OnEnable()
        {
            GameController.Instance.Bus.Subscribe<OnEventSelectScrew>(OnEventSelectScrew);
            GameController.Instance.Bus.Subscribe<OnEventSelectHole>(OnEventSelectHole);
        }

        private void OnDisable()
        {
            if (!GameController.HasInstance) return;
            GameController.Instance.Bus.Unsubscribe<OnEventSelectScrew>(OnEventSelectScrew);
            GameController.Instance.Bus.Unsubscribe<OnEventSelectHole>(OnEventSelectHole);
        }

        private void OnEventSelectHole(OnEventSelectHole onEventSelectHole)
        {
            PlayClickSFX();
        }

        private void OnEventSelectScrew(OnEventSelectScrew eventData)
        {
            PlayClickSFX();
        }

        private void PlayClickSFX()
        {
            if (screwSfx.Length > 0)
            {
                audioSource.volume = Random.Range(0.8f, 1f);
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(screwSfx[Random.Range(0, screwSfx.Length)]);
            }
        }
    }
}