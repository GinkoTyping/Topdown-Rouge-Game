using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class KnockBack : WeaponComponent<KnockBackData, AttackKnockBack>, IKnockbackable
    {
        private ActionHitBox hitBox;
        private CoreSystem.Movement movement;

        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            hitBox.OnDetectedCollider2D += HandleCollider2D;
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleCollider2D;
        }
        private void HandleCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(currentAttackData.Angel,currentAttackData.Strength, movement.FacingDirection);
                }
            }
        }
        public void Knockback(Vector2 angle, float strength, int direction)
        {
        }
    }
}