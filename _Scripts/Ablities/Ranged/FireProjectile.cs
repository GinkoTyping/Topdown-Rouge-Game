using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class FireProjectile : BaseAbility
{
    [Header("Fire Projectile")]
    [SerializeField] private ShotType shotType;
    [SerializeField] private float fireDuaration;
    [SerializeField] private float fireVelocity;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private float offsetSize;

    private Vector3 startPosition;
    private Vector3 fireDirection;

    private CommonPool poolManager;
    private Movement movement;
    private VectorHelper vectorHelper;

    private enum ShotType
    {
        Single,
        Single_Aligned,
        Tripple,
        Tripple_Aligned,
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
        poolManager = GameObject.Find("Containers").transform.Find("AnimatedProjectiles").GetComponent<CommonPool>();
    }

    protected override void Start()
    {
        movement = GetComponentInParent<Core>().GetCoreComponent<Movement>();
    }

    public override void Activate()
    {
        movement.FaceToItem(Player.Instance.transform);

        List<ProjectileData> datas = GetFireDirection();
        foreach (ProjectileData data in datas)
        {
            Projectile projectile = poolManager.Pool.Get().GetComponent<Projectile>();

            projectile.SetPool(poolManager);
            projectile.Set(data.startPosition, data.fireDirection, attackDamage, hostileLayer);
            projectile.Fire(fireVelocity, fireDuaration);
        }
    }

    public Vector3 GetStartPostion(Vector3 direction)
    {
        return transform.position + direction.normalized * offsetSize;
    }

    private List<ProjectileData> GetFireDirection()
    {
        List<ProjectileData> projectileData = new List<ProjectileData>();
        Vector3 basicDirection = Player.Instance.transform.position - transform.position;
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
        }

        return projectileData;
    }
}
