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

        private Stats stats;
        private ParticleManager particleManager;

        public void Damage(float amount)
        {
            stats.Health.Decrease(amount);
            particleManager.StartParticlesWithRandomRotation(damageParticles);
            SoundManager.Instance.PlaySound(damageClip);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = Core.GetCoreComponent<Stats>();
            particleManager = Core.GetCoreComponent<ParticleManager>();
        }
    }
}