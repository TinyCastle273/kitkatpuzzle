using CandyCoded.HapticFeedback;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace TinyCastle
{
    public class GameInputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameController gameController;
        [SerializeField] private LayerMask layer;
        [SerializeField] private LayerMask boardLayer;
        [SerializeField] private ParticleSystem removeBoardEffect;
        [SerializeField] private ParticleSystem inputEffect;
        private List<IGameInputHandler> inputHandlers;

        private void OnEnable()
        {
            inputHandlers = new List<IGameInputHandler>();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputHandlers.Clear();
                float minDistance = Mathf.Infinity;
                Vector3 mousePositionInWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (gameController.gameplayData.GameState == GameState.Playing)
                {
                    inputEffect.transform.position = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y, inputEffect.transform.position.z);
                    inputEffect.Play();
                    if (GameManager.Instance.UserData.vibrationEnabled)
                        HapticFeedback.LightFeedback();
                }
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePositionInWorld, Vector2.zero, Mathf.Infinity, layer);
                for (int i = 0; i < hits.Length; i++)
                {
                    var handler = hits[i].collider.GetComponent<IGameInputHandler>();
                    if (handler != null)
                    {
                        float distance = Vector2.Distance(mousePositionInWorld, hits[i].transform.position);
                        if (distance <= minDistance)
                        {
                            minDistance = distance;
                            inputHandlers.Insert(0, handler);
                        }
                        else
                        {
                            inputHandlers.Add(handler);
                        }
                    }
                }
                foreach (var handler in inputHandlers)
                {
                    bool handleSuccess = handler.OnPointerDown();
                    if (handleSuccess)
                        break;
                }

                if (gameController.gameplayData.GameState == GameState.SelectBoardToRemove)
                {
                    RaycastHit2D[] hitBoards = Physics2D.RaycastAll(mousePositionInWorld, Vector2.zero, Mathf.Infinity, boardLayer);
                    if (hitBoards.Length == 0) return;

                    if (hitBoards.Length >= 2)
                    {
                        Array.Sort(hitBoards, ((a, b) => a.transform.gameObject.layer < b.transform.gameObject.layer ? 1 : -1));
                    }
                    var hit = hitBoards[0];

                    var board = hit.collider.GetComponent<BoardController>();
                    if (board != null)
                    {
                        board.RemoveBoard();
                        if (GameManager.Instance.UserData.vibrationEnabled)
                            HapticFeedback.HeavyFeedback();
                        GameController.Instance.ResumeGame();
                        removeBoardEffect.transform.position = hit.transform.position;
                        removeBoardEffect.Play();
                    }


                }


            }


        }
    }
}