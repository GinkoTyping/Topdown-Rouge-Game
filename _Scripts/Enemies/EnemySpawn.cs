using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public GameObject skeleton;
    [SerializeField]
    public bool isSpawning;
    [SerializeField]
    public bool isDebug;
    [SerializeField]
    public Vector2[] spawnPositions;

    private EnemyPool poolManager;

    private void Start()
    {
        poolManager = GameObject.Find("Enemies").GetComponent<EnemyPool>();

        if (spawnPositions.Length > 0 && isSpawning)
        {
            poolManager.SetCurrrentObject(skeleton);
            foreach (Vector2 pos in spawnPositions)
            {
                GameObject enemy = poolManager.Pool.Get();
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
