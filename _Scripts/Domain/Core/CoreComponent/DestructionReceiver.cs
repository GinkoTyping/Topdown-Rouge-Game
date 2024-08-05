using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class DestrcutionReceiver : CoreComponent, IDestructable
    {
        [SerializeField]
        private GameObject destructionParticles;
        [SerializeField]
        private AudioClip destructClip;

        private Stats stats;
        private ParticleManager particleManager;
        public void Destruct(float amount)
        {
            stats.Health.Decrease(amount);
            particleManager.StartParticlesWithRandomRotation(destructionParticles);
            SoundManager.Instance.PlaySound(destructClip);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = Core.GetCoreComponent<Stats>();
            particleManager = Core.GetCoreComponent<ParticleManager>();
        }
    }
}