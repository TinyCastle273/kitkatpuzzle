using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyCastle
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] protected ScrewPuzzleSettings settings;
        [SerializeField] private Rigidbody2D body2d;
        [SerializeField] private BoxCollider2D boxCollider2D;
        public BoxCollider2D BoxCollider2D => boxCollider2D;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        [SerializeField] private HingeJoint2D hingeJoint2D;
        [SerializeField] private int length;
        public int Length => length;

        [SerializeField] private float height;
        public float Height => height;

        private List<ScrewController> _screwControllers;


        [SerializeField] protected int layer = 1;

        private List<HoleOnBoard> holeOnBoards;
        public int Layer => layer;

        private void OnEnable()
        {
            holeOnBoards = new List<HoleOnBoard>();
            UpdateLayer();
            GameController.Instance.Bus.Subscribe<OnEventUpdateScrew>(OnEventUpdateScrew);
        }

        private void OnEventUpdateScrew(OnEventUpdateScrew eventUpdateScrew)
        {
            if (eventUpdateScrew.boardControllers.Contains(this)) //Screw on this Board
            {
                if (eventUpdateScrew.addNewHole)
                {
                    var hole = Instantiate(settings.PrefabHoleOnBoard, transform);
                    hole.transform.position = eventUpdateScrew.controller.transform.position;
                    hole.SetLayer(layer);
                    holeOnBoards.Add(hole);
                }
                AddScrew(eventUpdateScrew.controller);
            }
            else //Screw not on this Board
            {
                RemoveScrew(eventUpdateScrew.controller);
            }
        }


        private void AddScrew(ScrewController screwController)
        {
            if (_screwControllers == null)
                _screwControllers = new List<ScrewController>();
            if (!_screwControllers.Contains(screwController))
            {
                _screwControllers.Add(screwController);
            }
            UpdateBoardState();

        }

        private void RemoveScrew(ScrewController screwController)
        {
            if (_screwControllers == null) return;
            if (_screwControllers.Contains(screwController))
            {
                _screwControllers.Remove(screwController);
                UpdateBoardState();
            }
        }

        public void UpdateBoardState()
        {
            if (_screwControllers != null && _screwControllers.Count >= 2)
            {
                RemoveForce();
            }
            else
            {
                if (_screwControllers != null && _screwControllers.Count >= 1)
                {
                    hingeJoint2D.enabled = true;
                    hingeJoint2D.connectedBody = _screwControllers[0].Body2D;
                    hingeJoint2D.anchor = transform.InverseTransformPoint(_screwControllers[0].Body2D.transform.position);
                }
                else
                {
                    hingeJoint2D.enabled = false;
                }
                body2d.bodyType = RigidbodyType2D.Dynamic;
                body2d.AddForce(Random.insideUnitCircle * 5f);
            }
        }

        public void RemoveForce()
        {
            body2d.bodyType = RigidbodyType2D.Kinematic;
            body2d.velocity = Vector2.zero;
            body2d.angularVelocity = 0;
        }

        public virtual void UpdateBoardLength()
        {
            Vector2 newSize = Vector2.zero;
            newSize.x = length * (settings.ScrewRadius * 2f);
            newSize.y = height * (settings.ScrewRadius * 2f);
            spriteRenderer.size = newSize;
            boxCollider2D.size = newSize;
        }
        public void Snap()
        {
            transform.position = settings.GetSnapPosition(transform.position);
        }

        public virtual void UpdateLayer()
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Layer" + layer);
            spriteRenderer.sprite = settings.SpriteBoards[layer - 1];
            spriteRenderer.sortingOrder = layer * 10 + 1;
        }

        public bool IsScrewInsideTheHole(Vector3 position)
        {
            foreach (var hole in holeOnBoards)
            {
                float distance = Vector3.Distance(position, hole.transform.position);
                if (distance <= settings.ScrewRadius * GameConsts.SCREW_INSIDE_HOLE_BOARD_ALLOW_RADIUS)
                    return true;
            }
            return false;
        }

        public void RemoveBoard()
        {
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (boxCollider2D == null) return;
            Vector2 from = transform.position + transform.right * boxCollider2D.size.x * 0.5f;
            Vector2 to = transform.position + -transform.right * boxCollider2D.size.x * 0.5f;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(from, to);
        }
#endif
    }
}