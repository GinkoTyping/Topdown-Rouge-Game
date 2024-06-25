using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : BaseAbility
{
    [Header("Fire Projectile")]
    [SerializeField] private float fireDuaration;
    [SerializeField] private float fireVelocity;
    [SerializeField] private Vector3 startOffset;

    private Vector3 startPosition;
    private Vector3 fireDirection;

    private CommonPool poolManager;
    private Movement movement;

    protected override void Awake()
    {
        base.Awake();
        
        poolManager = GameObject.Find("Containers").transform.Find("AnimatedProjectiles").GetComponent<CommonPool>();
    }

    protected override void Start()
    {
        movement = GetComponentInParent<Core>().GetCoreComponent<Movement>();
    }

    public override void Activate()
    {
        movement.FaceToItem(Player.Instance.transform);

        startPosition = transform.position + startOffset * movement.FacingDirection;
        fireDirection = Player.Instance.transform.position - startPosition;

        Projectile projectile = poolManager.Pool.Get().GetComponent<Projectile>();

        projectile.SetPool(poolManager);
        projectile.Set(startPosition, fireDirection, attackDamage);
        projectile.Fire(fireVelocity, fireDuaration);
    }
}
