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
        [SerializeField] float hostileDetectionRadius;
        [SerializeField] float closeRangeAttackRadius;
        [SerializeField] LayerMask hostileLayer;

        public bool IsHostileDetected {  get; private set; }
        public bool IsInMeleeAttackRange { get; private set; }

        public bool SetDetections(float radius)
        {
            Collider2D[] detections = Physics2D.OverlapCircleAll(transform.position, radius, hostileLayer);
            return detections.Length > 0;
        }

        private void Update()
        {
            IsHostileDetected = SetDetections(hostileDetectionRadius);
            IsInMeleeAttackRange = SetDetections(closeRangeAttackRadius);
        }

        private void OnDrawGizmos()
        {
            if (IsDebug)
            {
                Gizmos.DrawWireSphere(transform.position, hostileDetectionRadius);
                Gizmos.DrawWireSphere(transform.position, closeRangeAttackRadius);
            }
        }
    }
}
