using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class TutorialItem : MonoBehaviour
    {
        public static int CurrenTutorialIndex;
        [SerializeField] private int tutorialIndex;
        [SerializeField] private Transform tutorialObject;
        private void OnEnable()
        {
            GameController.Instance.Bus.Subscribe<OnEventSelectScrew>(OnEventSelectScrew);
            CurrenTutorialIndex = 0;
        }

        private void OnDisable()
        {
            if (GameController.HasInstance)
                GameController.Instance.Bus.Unsubscribe<OnEventSelectScrew>(OnEventSelectScrew);
        }

        private void Update()
        {
            tutorialObject.gameObject.SetActive(CurrenTutorialIndex == tutorialIndex);
        }


        private void OnEventSelectScrew(OnEventSelectScrew eventData)
        {
            if (tutorialIndex == CurrenTutorialIndex)
            {
                StopAllCoroutines();
                StartCoroutine(NextTutorial());
            }
        }

        IEnumerator NextTutorial()
        {
            yield return new WaitForSeconds(0.1f);
            CurrenTutorialIndex++;
        }
    }
}
