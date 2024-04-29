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
        [SerializeField] Vector2 rayCastOffset;
        [SerializeField] LayerMask obstableLayer;


        public bool IsHostileDetected {  get; private set; }
        public bool IsInMeleeAttackRange { get; private set; }
        public bool IsCollidingUpper { get; private set; }

        public bool SetDetections(float radius)
        {
            Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position, radius, hostileLayer);
            return detections.Length > 0;
        }

        public bool SetRayCastDetection(Vector2 offset, float distance)
        {
            RaycastHit2D detection = Physics2D.Raycast(transform.position + (Vector3)offset, Vector2.up, distance, obstableLayer);
            return detection.collider != null;
        }

        private void Update()
        {
            IsHostileDetected = SetDetections(hostileDetectionRadius);
            IsInMeleeAttackRange = SetDetections(closeRangeAttackRadius);
            IsCollidingUpper = SetRayCastDetection(rayCastOffset, rayCastDistance);
        }

        private void OnDrawGizmos()
        {
            if (IsDebug)
            {
                Gizmos.DrawWireSphere(transform.position, hostileDetectionRadius);
                Gizmos.DrawWireSphere(transform.position, closeRangeAttackRadius);
                Gizmos.DrawLine(transform.position + (Vector3)rayCastOffset, transform.position + (Vector3)rayCastOffset + new Vector3(0, rayCastDistance, 0));
            }
        }
    }
}
