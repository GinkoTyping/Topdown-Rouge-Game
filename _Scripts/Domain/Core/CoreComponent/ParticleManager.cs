using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class ParticleManager : CoreComponent
    {
        private Transform particleContainer;

        protected override void Awake()
        {
            base.Awake();

            particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
        }

        public GameObject StartParticles(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            return Instantiate(prefab, position, rotation, particleContainer);
        }
        public GameObject StartParticles(GameObject prefab)
        {
            return StartParticles(prefab, transform.position, Quaternion.identity);
        }
        public GameObject StartParticlesWithRandomRotation(GameObject prefab)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            return StartParticles(prefab, transform.position, rotation);
        }
    }
}