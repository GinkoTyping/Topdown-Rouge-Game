using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace Ginko.CoreSystem
{
    public class Detections : CoreComponent
    {
        [Header("Hostile Detection")]
        [SerializeField] bool IsDebug;
        [SerializeField] public bool ActiveHostileDetection;
        [SerializeField] public float hostileDetectionRadius;

        [SerializeField] private Vector3 meleeAttackOffset;
        [SerializeField] public float meleeAttackRadius;

        [SerializeField] public float rangedAttackRadius;
        [SerializeField] public LayerMask hostileLayer;

        [Header("Sprite Render")]
        [SerializeField] private BoundaryHelper hidingBehindBoundary;
        [SerializeField] private BoundaryHelper upperCollideBoundary;
        [SerializeField] public LayerMask obstableLayer;
        [SerializeField] public LayerMask bigObjectLayer;

        [Header("Interaction")]
        [SerializeField] private BoundaryHelper interactBoundary;
        [SerializeField] public LayerMask interactionLayer;
        [SerializeField] public string tagName;

        public bool IsHostileDetected {  get; private set; }
        public bool IsInMeleeAttackRange { get; private set; }
        public bool IsInRangedAttackRange { get; private set; }
        public bool IsCollidingUpper { get; private set; }
        public bool IsAbleToInteract { get; private set; }

        public Collider2D[] interactiveObjects;

        public Action<Collider2D[]> OnHidingBehind;
        public Action<Collider2D[]> OnInteractionItemsChange;
        private void Update()
        {
            SetHostileDetection();
            SetSpriteDetection();
            SetInteractDetection();
        }
        
        private bool GetDetections(float radius, LayerMask layerMask, Vector3 offset)
        {
            Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position + offset, radius, layerMask);
            return detections.Length > 0;
        }

        private bool GetInteractions(out Collider2D[] detections)
        {

            Collider2D[] newInteractItems = Physics2D.OverlapBoxAll(interactBoundary.transform.position, interactBoundary.rectSize
                , 0, interactionLayer)
                .Where(x => 
                    x.tag == "Interactive" &&
                    (x.GetComponentInChildren<IInteractable>().isInteractive ||
                    x.GetComponent<IInteractable>().isInteractive)
                    )
                .ToArray();

            if (!interactiveObjects.SequenceEqual(newInteractItems))
            {
                OnInteractionItemsChange?.Invoke(newInteractItems);
            }

            detections = newInteractItems;

            return detections.Length > 0;
        }

        private bool GetBoxDetections(Vector2 offset, Vector2 size)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(offset, size, 0, obstableLayer);
            return colliders.Length > 0;
        }

        private void SetHostileDetection()
        {
            if (ActiveHostileDetection)
            {
                IsHostileDetected = GetDetections(hostileDetectionRadius, hostileLayer, Vector3.zero);
                IsInMeleeAttackRange = GetDetections(meleeAttackRadius, hostileLayer, meleeAttackOffset);
                IsInRangedAttackRange = GetDetections(rangedAttackRadius, hostileLayer, Vector3.zero);
            }
        }

        private void SetSpriteDetection()
        {
            if (hidingBehindBoundary != null)
            {
                IsCollidingUpper = GetBoxDetections(hidingBehindBoundary.transform.position, hidingBehindBoundary.rectSize);
            }

            if (upperCollideBoundary != null)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(upperCollideBoundary.transform.position, upperCollideBoundary.rectSize, 0, bigObjectLayer);

                OnHidingBehind?.Invoke(colliders);
            }
        }

        private void SetInteractDetection()
        {
            if (interactBoundary != null)
            {
                IsAbleToInteract = GetInteractions(out interactiveObjects);
            }
        }

        private void OnDrawGizmos()
        {
            if (IsDebug)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(transform.position, hostileDetectionRadius);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + meleeAttackOffset, meleeAttackRadius);
                Gizmos.DrawWireSphere(transform.position, rangedAttackRadius);
            }
        }
    }
}
