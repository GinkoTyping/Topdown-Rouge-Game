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
        public bool allowLifeSteal;
        [SerializeField]
        private GameObject damageParticles;
        [SerializeField]
        private AudioClip damageClip;
        [SerializeField]
        private Color damageColor;

        private Stats stats;
        private ParticleManager particleManager;
        private SpriteEffect spriteHandler;
        private FloatingText floatingTextComp;
        private BuffManager buffManager;

        protected override void Awake()
        {
            base.Awake();

            stats = Core.GetCoreComponent<Stats>();
            particleManager = Core.GetCoreComponent<ParticleManager>();
            spriteHandler = Core.GetCoreComponent<SpriteEffect>();
            floatingTextComp = Core.GetCoreComponent<FloatingText>();
            buffManager = Core.GetCoreComponent<BuffManager>();
        }

        public void Damage(float amount, bool isCritical = false, Entity sender = null)
        {
            Damage(new DamageDetail(amount), sender);
        }

        private void HandleDamageSender(Entity sender)
        {
            if (sender == null)
            {
                return;
            }

            BuffManager senderBuffs = sender.Core.GetCoreComponent<BuffManager>();

            LifeSteal lifeSteal = senderBuffs.GetComponentInChildren<LifeSteal>();
            if (lifeSteal != null && allowLifeSteal)
            {
                lifeSteal.Activate();
            }
        }

        public void Damage(DamageDetail damageDetail, Entity sender = null)
        {
            stats.Health.Decrease(damageDetail.amount);
            HandleDamageSender(sender);
            ApplyDamageEffect(damageDetail.buffEffect);

            if (floatingTextComp != null)
            {
                floatingTextComp.FloatDamageText(damageDetail);
            }

            GameObject hitParticles = damageDetail.hitParticle == null ? damageParticles : damageDetail.hitParticle;
            if (damageDetail.showHitParticle)
            {
                particleManager.StartParticlesWithRandomRotation(hitParticles);
            }

            spriteHandler?.TintSprite(damageColor);

            if (damageDetail.playSound)
            {
                SoundManager.Instance.PlaySound(damageClip);
            }
        }

        public void ApplyDamageEffect(Buff buffEffect)
        {
            if (buffEffect != null && buffManager != null)
            {
                buffManager.Add(buffEffect);
            }
        }
    }
}