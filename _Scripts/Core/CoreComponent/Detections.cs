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

        [SerializeField] bool IsDebug;

        [Header("Hostile Detection")]
        [SerializeField] public float hostileDetectionRadius;
        [SerializeField] public float closeRangeAttackRadius;
        [SerializeField] public LayerMask hostileLayer;

        [Header("Sprite Render")]
        [SerializeField] public float rayCastDistance;
        [SerializeField] public float hidingDetectionDistance;
        [SerializeField] public Vector2 rayCastOffset;
        [SerializeField] public LayerMask obstableLayer;
        [SerializeField] public LayerMask bigObjectLayer;

        [Header("Interaction")]
        [SerializeField] public float interactionDistance;
        [SerializeField] public LayerMask interactionLayer;
        [SerializeField] public string tagName;


        public bool IsHostileDetected {  get; private set; }
        public bool IsInMeleeAttackRange { get; private set; }
        public bool IsCollidingUpper { get; private set; }
        public bool IsAbleToInteract { get; private set; }

        public Collider2D[] interactiveObjects;

        public Action<Collider2D[]> OnHidingBehind;

        public bool GetDetections(float radius, LayerMask layerMask)
        {
            Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
            return detections.Length > 0;
        }

        public bool GetDetections(float radius, LayerMask layerMask, string tagName, out Collider2D[] detections)
        {
            detections = Physics2D.OverlapCircleAll(transform.position, radius, layerMask).Where(x => x.tag == tagName).ToArray();
            return detections.Length > 0;
        }


        public bool SetRayCastDetection(Vector2 offset, float distance)
        {
            RaycastHit2D detection = Physics2D.Raycast(transform.position + (Vector3)offset, Vector2.up, distance, obstableLayer);
            return detection.collider != null;
        }

        private void Update()
        {
            IsHostileDetected = GetDetections(hostileDetectionRadius, hostileLayer);
            IsInMeleeAttackRange = GetDetections(closeRangeAttackRadius, hostileLayer);
            IsAbleToInteract = GetDetections(interactionDistance, interactionLayer, "Interactive",out interactiveObjects);

            IsCollidingUpper = SetRayCastDetection(rayCastOffset, rayCastDistance);

            EmitHidingBehindEvent();
        }

        private void EmitHidingBehindEvent()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hidingDetectionDistance, bigObjectLayer);
            OnHidingBehind.Invoke(colliders);
        }

        private void OnDrawGizmos()
        {
            if (IsDebug)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, hostileDetectionRadius);
                Gizmos.DrawWireSphere(transform.position, closeRangeAttackRadius);
                Gizmos.DrawWireSphere(transform.position, interactionDistance);
                Gizmos.DrawLine(transform.position + (Vector3)rayCastOffset, transform.position + (Vector3)rayCastOffset + new Vector3(0, rayCastDistance, 0));
            }
        }
    }
}
