using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.EnemySystem;
using Ginko.StateMachineSystem;

namespace Ginko.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField]
        private GameObject[] deathParticles;
        [SerializeField]
        private AudioClip deathAudioClip;

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
        public void Die()
        {
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            SoundManager.Instance.PlaySound(deathAudioClip);

            GameObject GO = Core.transform.parent.gameObject;
            Entity obj = GO.GetComponent<Entity>();

            if (obj)
            {
                // ����� enemy�� ʹ�ö����
                if (GO.GetComponent<Enemy>())
                {
                    obj.pool.Release(obj.gameObject);
                }
                // ��������
                else
                {
                    GO.SetActive(false);
                }
            } 

            // ���ֻ�� ��ͨ��Ʒ
            else
            {
                Destroy(GO);
            }
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }
        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}