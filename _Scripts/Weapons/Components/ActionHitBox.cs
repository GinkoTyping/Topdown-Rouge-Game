using Ginko.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        public event Action<Collider2D[]> OnDetectedCollider2D;

        private CoreSystem.Movement movement;
        private Vector2 offset;
        private Collider2D[] detected;
        private void HandleAttackAction()
        {

            offset.Set(
                transform.position.x + (currentAttackData.HitBox.center.x * movement.FacingDirection),
                transform.position.y + currentAttackData.HitBox.center.y);
            detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0, data.DetectableLayers);
            if (detected.Length > 0)
            {
                OnDetectedCollider2D?.Invoke(detected);
            }
        }
        protected override void Start()
        {
            base.Start();

            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            EventHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.OnAttackAction -= HandleAttackAction;
        }

        private void OnDrawGizmos()
        {
            if (data != null)
            {
                foreach (var item in data.AttackData)
                {
                    if (item.debug)
                    {
                        Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
                    }
                }
            }
        }
    }
}