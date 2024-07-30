using Ginko.StateMachineSystem;
using Shared.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public GameObject enemyPrefab;
    [SerializeField]
    public bool isSpawning;
    [SerializeField]
    public bool isDebug;
    [SerializeField]
    public Vector2[] spawnPositions;

    private PoolManager poolManager;
    private PoolHelper poolHelper;
    private Transform enemyPoolsContainer;

    private void Awake()
    {
        poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
        enemyPoolsContainer = GameObject.Find("Enemies").transform;
    }

    private void Start()
    {
        poolManager = poolHelper.GetPoolByPrefab(enemyPoolsContainer, enemyPrefab);

        if (spawnPositions.Length > 0 && isSpawning)
        {
            poolManager.SetCurrrentObject(enemyPrefab);
            foreach (Vector2 pos in spawnPositions)
            {
                GameObject enemy = poolManager.Pool.Get();
                enemy.GetComponent<Entity>().SetPool(poolManager.Pool);
                enemy.transform.position = pos;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.color = Color.red;
            foreach (Vector2 pos in spawnPositions)
            {
                Gizmos.DrawWireSphere(pos, 1);
            }
        }
    }
}
