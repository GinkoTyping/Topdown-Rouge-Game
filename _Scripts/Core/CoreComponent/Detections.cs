using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Ginko.CoreSystem
{
    public class Detections : CoreComponent
    {

        [SerializeField] bool IsDebug;

        [Header("Hostile Detection")]
        [SerializeField] float hostileDetectionRadius;
        [SerializeField] float closeRangeAttackRadius;
        [SerializeField] LayerMask hostileLayer;

        [Header("Sprite Render")]
        [SerializeField] float rayCastDistance;
        [SerializeField] float hidingDetectionDistance;
        [SerializeField] Vector2 rayCastOffset;
        [SerializeField] LayerMask obstableLayer;
        [SerializeField] LayerMask bigObjectLayer;


        public bool IsHostileDetected {  get; private set; }
        public bool IsInMeleeAttackRange { get; private set; }
        public bool IsCollidingUpper { get; private set; }

        public Action<Collider2D[]> OnHidingBehind;

        public bool SetDetections(float radius, LayerMask layerMask)
        {
            Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
            return detections.Length > 0;
        }

        public bool SetRayCastDetection(Vector2 offset, float distance)
        {
            RaycastHit2D detection = Physics2D.Raycast(transform.position + (Vector3)offset, Vector2.up, distance, obstableLayer);
            return detection.collider != null;
        }

        private void Update()
        {
            IsHostileDetected = SetDetections(hostileDetectionRadius, hostileLayer);
            IsInMeleeAttackRange = SetDetections(closeRangeAttackRadius, hostileLayer);

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
                Gizmos.DrawLine(transform.position + (Vector3)rayCastOffset, transform.position + (Vector3)rayCastOffset + new Vector3(0, rayCastDistance, 0));
            }
        }
    }
}
