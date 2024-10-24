using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TinyCastle
{
    public class HoleController : MonoBehaviour, IGameInputHandler
    {
        [SerializeField] private bool adsUnlock;
        [SerializeField] private GameObject adsIcon;
        [SerializeField] private ScrewPuzzleSettings settings;
        private ScrewController currentScrew;

        private void OnEnable()
        {
            adsIcon.SetActive(adsUnlock);
        }
        public bool OnPointerDown()
        {
            if (currentScrew != null) return false; //Unable to assign new Screw
            if (GameController.Instance.gameplayData.GameState != GameState.Playing) return false;

            if (adsUnlock)
            {
                RequestAds();
                return false;
            }
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward, 0.1f);
            for (int i = 0; i < hits.Length; i++)
            {
                BoardController board = hits[i].transform.GetComponent<BoardController>();
                if (board != null)
                {
                    if (!board.IsScrewInsideTheHole(transform.position)) //Screw out side the hole
                        return false;

                }
            }

            //Able to assign new Screw
            GameController.Instance.Bus.Publish(new OnEventSelectHole()
            {
                controller = this
            });
            return true;
        }

        private void RequestAds()
        {
            GameController.Instance.PauseGame();
            UIManager.Instance.ShowUI(UIName.UIPopupHint, new UIPopupHint.Data()
            {
                item = settings.GameItems.GetItemByID("Hole"),
                onHintUsed = () =>
                {
                    OnShowAdsSuccessful();
                },
                onClosePopup = () =>
                {
                    GameController.Instance.ResumeGame();
                }
            });
        }

        private void OnShowAdsSuccessful()
        {
            adsUnlock = false;
            adsIcon.SetActive(adsUnlock);
            GameController.Instance.ResumeGame();
        }

        public void AddScrew(ScrewController screw)
        {
            currentScrew = screw;
        }

        public void RemoveScrew()
        {
            currentScrew = null;
        }

        public void Snap()
        {
            transform.position = settings.GetSnapPosition(transform.position);
        }
    }
}