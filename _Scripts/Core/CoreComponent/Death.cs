using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.EnemySystem;
using Ginko.StateMachineSystem;
using System;

namespace Ginko.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField]
        private GameObject[] deathParticles;
        [SerializeField]
        private AudioClip deathAudioClip;
        [SerializeField]
        private IDeathrattle[] deathrattles;

        private ParticleManager ParticleManager
        {
            get => particleManager ??= Core.GetCoreComponent<ParticleManager>();

        }
        private Stats Stats
        {
            get => stats ??= Core.GetCoreComponent<Stats>();
        }
        private ParticleManager particleManager;
        private Stats stats;

        public event Action OnDeath;
        
        public void Die()
        {
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            SoundManager.Instance.PlaySound(deathAudioClip);

            GameObject GO = Core.transform.parent.gameObject;
            Entity obj = GO.GetComponent<Entity>();

            OnDeath?.Invoke();

            if (obj)
            {
                // 如果是 enemy， 使用对象池
                if (GO.GetComponent<Enemy>())
                {
                    if (GO.GetComponent<Entity>().pool == null)
                    {
                        Destroy(GO);
                    } else
                    {
                        obj.pool.Release(obj.gameObject);
                    }
                }
                // 如果是玩家
                else
                {
                    GO.SetActive(false);
                }
            } 

            // 如果只是 普通物品
            else
            {
                Destroy(GO);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            Stats.Health.OnCurrentValueZero += HandleOnDeath;
        }
        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= HandleOnDeath;
        }

        private void HandleOnDeath()
        {
            Entity entity = Core.transform.parent.GetComponent<Entity>();
            entity.StateMachine.ChangeState(entity.DeathState);
        }
    }
}