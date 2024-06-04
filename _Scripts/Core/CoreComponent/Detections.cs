﻿using System;
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
        [SerializeField] public bool ActiveHostileDetection;
        [SerializeField] public float hostileDetectionRadius;
        [SerializeField] public float closeRangeAttackRadius;
        [SerializeField] public LayerMask hostileLayer;

        [Header("Sprite Render")]
        [SerializeField] public bool ActiveSpriteRenderDetection;
        [SerializeField] public Vector2 hidingDetectionSize;
        [SerializeField] public Vector2 hidingDetectionOffset;
        [SerializeField] public Vector3 upperDetectionOffset;
        [SerializeField] public Vector3 upperDetectionSize;
        [SerializeField] public LayerMask obstableLayer;
        [SerializeField] public LayerMask bigObjectLayer;

        [Header("Interaction")]
        [SerializeField] public bool ActiveInteractionDetection;
        [SerializeField] public Vector2 interactionSize;
        [SerializeField] public Vector2 interactionOffset;
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

        public bool GetInteractions(out Collider2D[] detections)
        {
            detections = Physics2D.OverlapBoxAll(transform.position + (Vector3)interactionOffset, interactionSize, interactionLayer)
                .Where(x => x.tag == "Interactive" && x.GetComponentInChildren<IInteractable>().isInteractive)
                .ToArray();
            return detections.Length > 0;
        }


        public bool GetBoxDetections(Vector2 offset, Vector2 size)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(offset, size, 0, obstableLayer);
            return colliders.Length > 0;
        }

        private void Update()
        {
            if (ActiveHostileDetection)
            {
                IsHostileDetected = GetDetections(hostileDetectionRadius, hostileLayer);
                IsInMeleeAttackRange = GetDetections(closeRangeAttackRadius, hostileLayer);
            }

            if (ActiveSpriteRenderDetection)
            {
                IsCollidingUpper = GetBoxDetections(transform.position + upperDetectionOffset, upperDetectionSize);
            }

            if (ActiveInteractionDetection)
            {
                IsAbleToInteract = GetInteractions(out interactiveObjects);
            }

            EmitHidingBehindEvent();
        }

        private void EmitHidingBehindEvent()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)hidingDetectionOffset, hidingDetectionSize, 0, bigObjectLayer);

            OnHidingBehind.Invoke(colliders);
        }

        private void OnDrawGizmos()
        {
            if (IsDebug)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, hostileDetectionRadius);
                Gizmos.DrawWireSphere(transform.position, closeRangeAttackRadius);
                Gizmos.DrawWireCube(transform.position + (Vector3)hidingDetectionOffset, hidingDetectionSize);
                Gizmos.DrawWireCube(transform.position + (Vector3)interactionOffset, interactionSize);
                Gizmos.DrawWireCube(transform.position + upperDetectionOffset, upperDetectionSize);
            }
        }
    }
}
