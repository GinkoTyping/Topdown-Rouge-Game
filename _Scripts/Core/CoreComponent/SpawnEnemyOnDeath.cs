using Ginko.EnemySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Ginko.CoreSystem
{
    public class SpawnEnemyOnDeath : CoreComponent
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnRadius;
        [SerializeField] private int defaultSpawnCount;
        [SerializeField] private int enemyCount;


        private int spawnCount;
        private EnemyPool poolManager;
        private Death deathComp;

        private void Start()
        {
            poolManager = GameObject.Find("Enemies").GetComponent<EnemyPool>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            spawnCount = defaultSpawnCount;
            deathComp = Core.GetCoreComponent<Death>();
            deathComp.OnDeath += Spawn;
        }

        private void OnDisable()
        {
            deathComp.OnDeath -= Spawn;
        }

        private Vector3 GetRamdonPos(float radius)
        {
            return new Vector3 (
                Random.Range(-radius, radius), 
                Random.Range(-radius, radius), 
                0);
        }

        public void SetSpawnCount(int count)
        {
            spawnCount = count;
        }

        public void Spawn()
        {
            if (spawnCount > 0)
            {
                poolManager.SetCurrrentObject(enemyPrefab);

                for (int i = 0; i < enemyCount; i++)
                {
                    GameObject obj = poolManager.Pool.Get();
                    obj.transform.position = transform.position + GetRamdonPos(spawnRadius);

                    Enemy enemy = obj.GetComponent<Enemy>();
                    enemy.Core.GetCoreComponent<SpawnEnemyOnDeath>().SetSpawnCount(spawnCount - 1);
                }
            }
        }
    }
}