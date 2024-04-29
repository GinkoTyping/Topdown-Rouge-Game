using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons;

namespace Ginko.CoreSystem
{
    public class Attack : CoreComponent
    {
        [SerializeField]
        public AttackDetail[] attackDetails;
        [SerializeField]
        public LayerMask hostileLayer;

        private Animator animator;
        private int atttackCounter;

        private Movement Movement
        {
            get => movement ??= Core.GetCoreComponent<Movement>();
        }
        private Movement movement;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponentInParent<Animator>();
        }

        public void InstaniateAttack()
        {
            atttackCounter = animator.GetInteger("AttackCounter");
            AttackDetail currentAttack = attackDetails[atttackCounter];
            Collider2D[] colliders = Physics2D.OverlapBoxAll(currentAttack.GetCenterPoint(transform.position, Movement.FacingDirection), currentAttack.hitBoxSize, 0, hostileLayer);

            foreach (var collider in colliders)
            {
                IDamageable damageble = collider.transform.parent.GetComponent<IDamageable>();
                if (damageble != null)
                {
                    damageble.Damage(currentAttack.damageAmount);
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var attackDetail in attackDetails)
            {
                if (attackDetail.isDebug)
                {
                    Vector2 center = new Vector2 (transform.position.x + attackDetail.hitBoxOffset.x, transform.position.y + attackDetail.hitBoxOffset.y);
                    Gizmos.DrawWireCube(center, attackDetail.hitBoxSize);
                }
            }
        }
    }

    [Serializable]
    public class AttackDetail
    {
        [SerializeField]
        public bool isDebug;

        [SerializeField]
        public float damageAmount;
        [SerializeField]
        public float moveVelocity;
        [SerializeField]
        public Vector2 hitBoxSize;
        [SerializeField]
        public Vector2 hitBoxOffset;

        public Vector2 GetCenterPoint(Vector3 origin, int facingDirection)
        {
            return new Vector2(origin.x + hitBoxOffset.x * facingDirection, origin.y + hitBoxOffset.y);
        }
    }
}