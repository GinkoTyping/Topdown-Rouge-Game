using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons.Components;
using Ginko.CoreSystem;
namespace Ginko.Weapons.Components
{
    public class Damage : WeaponComponent<DamageData, AttackDamage>
    {
        private ActionHitBox hitBox;
        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders)
            {
                DamageReceiver damageble = collider.transform.parent.GetComponent<DamageReceiver>();
                if (damageble != null)
                {
                    damageble.Damage(damageble.IsDesctructableItem ? currentAttackData.DestructionAmount : currentAttackData.Amount);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;

        }
    }
}
