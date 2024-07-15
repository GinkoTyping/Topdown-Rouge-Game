using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : BaseAbility
{
    [Header("Projectiles")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private ProjectileDataSO defaultProjectileData;
    [SerializeField] private ProjectileDataSO[] specialProjectilesData;

    [Header("Firing Type")]
    [SerializeField] private TargetType targetType;
    [SerializeField] private ShotType shotType;
    [SerializeField] private float offsetSize;

    private PoolManager poolManager;
    private Movement movement;
    private VectorHelper vectorHelper;
    private PoolHelper poolHelper;
    private Transform projectilePoolContainer;
    private ProjectileMultiplier[] projectileMultipliers;

    private enum ShotType
    {
        Single,
        Single_Aligned,
        Tripple,
        Tripple_Aligned,
        Up_Down_Left_Right,

        Manual,
    }

    private enum TargetType
    {
        ToPlayer,
        Aim,
    }

    private class ProjectileData
    {
        public Vector3 startPosition;
        public Vector3 fireDirection;

        public ProjectileData(Vector3 direction, Vector3 postion)
        {
            fireDirection = direction;
            startPosition = postion;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        vectorHelper = new VectorHelper();
        projectilePoolContainer = GameObject.Find("Containers").transform;
        poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
    }

    protected override void Start()
    {
        movement = GetComponentInParent<Core>().GetCoreComponent<Movement>();
        projectileMultipliers = GetComponents<ProjectileMultiplier>();

        if (poolManager == null)
        {
            poolManager = poolHelper.GetPoolByPrefab(projectilePoolContainer, projectilePrefab);
        }
    }

    public override void Activate()
    {
        movement.FaceToItem(Player.Instance.transform);

        List<ProjectileData> datas = GetFireDirection();

        if (datas.Count > 0 && playAudioOnActivate)
        {
            PlayAbilitySound();
        }

        foreach (ProjectileData data in datas)
        {
            Projectile projectile = poolManager.Pool.Get().GetComponent<Projectile>();

            projectile.SetPool(poolManager);
            projectile.Set(GetCurrentProjectileData(), data.startPosition, hostileLayer);
            ApplyMultiplier(projectile);

            projectile.Fire(data.fireDirection);
        }
    }

    private void ApplyMultiplier(Projectile projectile)
    {
        if (projectileMultipliers.Length > 0)
        {
            foreach (ProjectileMultiplier multiplier in projectileMultipliers)
            {
                multiplier.Apply(projectile);
            }
        }
    }

    private List<ProjectileData> GetFireDirection()
    {
        List<ProjectileData> projectileData = new List<ProjectileData>();

        Vector3 basicDirection =
            targetType == TargetType.ToPlayer
            ? Player.Instance.transform.position - transform.position
            : Player.Instance.InputHandler.AimPosition;

        Vector3 nearesetDirection = vectorHelper.GetNearestVector(basicDirection);
        Quaternion posRotation = Quaternion.Euler(0, 0, 20f);
        Quaternion negaRotation = Quaternion.Euler(0, 0, -20f);

        switch (shotType)
        {
            case ShotType.Single:
                projectileData.Add(new ProjectileData(
                    basicDirection,
                    GetStartPostion(basicDirection)));
                break;

            case ShotType.Single_Aligned:
                projectileData.Add(new ProjectileData(
                    nearesetDirection,
                    GetStartPostion(nearesetDirection)));
                break;

            case ShotType.Tripple:
                projectileData.Add(new ProjectileData(
                    basicDirection,
                    GetStartPostion(basicDirection)));
                projectileData.Add(new ProjectileData(
                    posRotation * basicDirection,
                    GetStartPostion(posRotation * basicDirection)));
                projectileData.Add(new ProjectileData(
                    negaRotation * basicDirection,
                    GetStartPostion(negaRotation * basicDirection)));
                break;

            case ShotType.Tripple_Aligned:
                projectileData.Add(new ProjectileData(
                    nearesetDirection,
                    GetStartPostion(nearesetDirection)));
                projectileData.Add(new ProjectileData(
                    posRotation * nearesetDirection,
                    GetStartPostion(posRotation * basicDirection)));
                projectileData.Add(new ProjectileData(
                    negaRotation * nearesetDirection,
                    GetStartPostion(negaRotation * nearesetDirection)));
                break;
            
            case ShotType.Up_Down_Left_Right:
                projectileData.Add(new ProjectileData(
                    Vector3.up,
                    GetStartPostion(Vector3.up)));
                projectileData.Add(new ProjectileData(
                    Vector3.down,
                    GetStartPostion(Vector3.down)));
                projectileData.Add(new ProjectileData(
                    Vector3.left,
                    GetStartPostion(Vector3.left)));
                projectileData.Add(new ProjectileData(
                    Vector3.right,
                    GetStartPostion(Vector3.right)));
                break;
        }

        return projectileData;
    }
    
    private Vector3 GetStartPostion(Vector3 direction)
    {
        return transform.position + direction.normalized * offsetSize;
    }

    private ProjectileDataSO GetCurrentProjectileData()
    {
        if (specialProjectilesData.Length == 0)
        {
            return defaultProjectileData;
        }

        ProjectileDataSO output = null;
        float ramdonFloat = UnityEngine.Random.Range(0f, 1f);

        for (int i = 0; i < specialProjectilesData.Length; ++i)
        {
            float min = i == 0 ? 0 : specialProjectilesData[i - 1].possibility;
            float max = specialProjectilesData[i].possibility;
            if (ramdonFloat > min && ramdonFloat <= max)
            {
                output = specialProjectilesData[i];
                break;
            }
        }

        return output == null ? defaultProjectileData : output;
    }
}
