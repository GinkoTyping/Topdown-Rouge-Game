using Ginko.EnemySystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        [SerializeField]
        public bool IsDesctructableItem;
        [SerializeField]
        private GameObject damageParticles;
        [SerializeField]
        private AudioClip damageClip;
        [SerializeField]
        private Color damageColor;

        private Stats stats;
        private ParticleManager particleManager;
        private SpriteEffect spriteHandler;

        protected override void Awake()
        {
            base.Awake();

            stats = Core.GetCoreComponent<Stats>();
            particleManager = Core.GetCoreComponent<ParticleManager>();
            spriteHandler = Core.GetCoreComponent<SpriteEffect>();
        }

        public void Damage(float amount, Entity sender = null)
        {
            stats.Health.Decrease(amount);
            HandleDamageSender(sender);
            particleManager.StartParticlesWithRandomRotation(damageParticles);
            spriteHandler?.TintSprite(damageColor);
            SoundManager.Instance.PlaySound(damageClip);
        }

        private void HandleDamageSender(Entity sender)
        {
            if (sender == null)
            {
                return;
            }

            BuffManager senderBuffs = sender.Core.GetCoreComponent<BuffManager>();

            LifeSteal lifeSteal = senderBuffs.GetComponentInChildren<LifeSteal>();
            if (lifeSteal != null)
            {
                lifeSteal.Activate();
            }
        }
    }
}