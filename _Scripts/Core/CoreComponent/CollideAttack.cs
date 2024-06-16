using Ginko.EnemySystem;
using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class CollideAttack : CoreComponent
    {
        [SerializeField]
        private float attackDamage;
        [SerializeField]
        private Vector2 collideSize;
        [SerializeField]
        private Vector3 collideOffset;
        [SerializeField]
        private LayerMask hostileLayer;
        [SerializeField]
        public AudioClip attackSound;

        public bool allowCollideAttack {  get; private set; }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            InstaniateAttack();
        }

        public void StartDetection(bool isAllow)
        {
            allowCollideAttack = isAllow;
        }

        public void InstaniateAttack()
        {
            if (allowCollideAttack)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + collideOffset, collideSize, 0, hostileLayer);
                if (colliders.Length > 0)
                {
                    allowCollideAttack = false;
                    IDamageable damageble = colliders[0].transform.parent.GetComponent<IDamageable>();
                    if (damageble != null)
                    {
                        damageble.Damage(attackDamage);
                    }
                }
            }
        }
    }
}