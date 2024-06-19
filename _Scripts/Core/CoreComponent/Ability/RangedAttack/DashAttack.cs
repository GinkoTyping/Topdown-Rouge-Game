using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class DashAttack : RangedAttack
    {
        [SerializeField]
        private Vector2 collideSize;
        [SerializeField]
        private Vector3 collideOffset;
        [SerializeField]
        private float attackChargeTime; 
        [SerializeField]
        private float dashDuaration;
        [SerializeField]
        private float dashVelocity;

        private Movement movement;
        private SpriteEffect spriteEffect;

        private float dashStart;
        private Vector3 dashDir;
        
        private void Start()
        {
            movement = Core.GetCoreComponent<Movement>();
            spriteEffect = Core.GetCoreComponent<SpriteEffect>();
        }

        public override void Attack()
        {
            if (dashStart != 0)
            {
                if (Time.time < dashStart + attackChargeTime)
                {
                    SetStatus(RangedAttackStatus.Charge);
                    movement.FaceToItem(Player.Instance.transform);
                }
                else if (Time.time >= dashStart + attackChargeTime
                    && Time.time < dashStart + attackChargeTime + dashDuaration)
                {
                    SetStatus(RangedAttackStatus.Attack);
                    movement.SetVelocity(dashVelocity, (Vector2)dashDir);
                }
                else
                {
                    Set(isDefault: true);
                }
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            DetectAttack();
        }

        public override void Set(bool isDefault = false)
        {
            if (isDefault)
            {
                movement.SetVelocityZero();
                dashStart = 0;
                dashDir = Vector3.zero;
            }
            else
            {
                SetAllowDetection(true);
                spriteEffect.TintSprite(spriteEffect.warningColor, 3);

                Vector3 playerPos = Player.Instance.transform.position;
                dashDir = playerPos - movement.RB.transform.position;
                dashStart = Time.time;

                statusIndex = RangedAttackStatus.Idle;
            }
        }

        public void DetectAttack()
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

        private void OnDrawGizmos()
        {
            if (isDebug)
            {
                Gizmos.DrawWireCube(transform.position + collideOffset, collideSize);
            }
        }
    }
}