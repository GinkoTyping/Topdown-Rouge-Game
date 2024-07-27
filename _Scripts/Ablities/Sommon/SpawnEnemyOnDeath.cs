using Ginko.CoreSystem;
using Ginko.EnemySystem;
using Shared.Utilities;
using UnityEngine;

public class SpawnEnemyOnDeath : BaseAbility
{
    [Header("Spawn Enemy On Death")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int defaultSpawnCount;
    [SerializeField] private int enemyCount;

    private int spawnCount;
    private PoolHelper poolHelper;
    private PoolManager poolManager;
    private Transform enemiesContainer;
    protected override void Start()
    {
        base.Start();

        poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
        enemiesContainer = GameObject.Find("Enemies").transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SetSpawnCount(defaultSpawnCount);
    }

    private Vector3 GetRamdonPos(float radius)
    {
        return new Vector3(
            Random.Range(-radius, radius),
            Random.Range(-radius, radius),
            0);
    }

    private void SetSpawnCount(int count)
    {
        spawnCount = count;
    }

    public override void Activate()
    {
        if (spawnCount > 0)
        {
            if (poolManager == null)
            {
                poolManager = poolHelper.GetPoolByPrefab(enemiesContainer, enemyPrefab);
            }

            Debug.Log(poolManager);


            for (int i = 0; i < enemyCount; i++)
            {
                GameObject obj = poolManager.Pool.Get();
                obj.transform.position = transform.position + GetRamdonPos(spawnRadius);

                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Core.GetCoreComponent<Death>().GetComponentInChildren<SpawnEnemyOnDeath>().SetSpawnCount(spawnCount - 1);
            }
        }
    }
}