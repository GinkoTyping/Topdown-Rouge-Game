using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public GameObject skeleton;

    [SerializeField]
    public bool isDebug;
    [SerializeField]
    public Vector2[] spawnPositions;

    private void Start()
    {
        if (spawnPositions.Length > 0)
        {
            PoolManager.Instance.SetCurrrentObject(skeleton);
            foreach (Vector2 pos in spawnPositions)
            {
                GameObject enemy = PoolManager.Instance.Pool.Get();
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
