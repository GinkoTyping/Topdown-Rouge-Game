using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Ginko.CoreSystem.RangedAttack;

public class DashingAttack : MonoBehaviour,IAblity
{
    [SerializeField] private bool isDebug;
    [SerializeField] protected LayerMask hostileLayer;
    [SerializeField] protected float attackDamage;

    [SerializeField]
    private Vector2 collideSize;
    [SerializeField]
    private Vector3 collideOffset;
    [SerializeField]
    private float attackChargeTime;
    [SerializeField]
    private float dashDuaration;
    [SerializeField]
    private float dashVelocity;

    private Core core;

    private Movement movement;
    private SpriteEffect spriteEffect;

    private float dashStart;
    private Vector3 dashDir;

    public event Action OnCharge;
    public event Action OnAttack;

    private bool allowAttackDetection;

    private void Awake()
    {
        core = GetComponentInParent<Core>();
    }
    private void Start()
    {
        movement = core.GetCoreComponent<Movement>();
        spriteEffect = core.GetCoreComponent<SpriteEffect>();
    }

    private void Update()
    {
        UpdateDashAttack();
    }

    public void Activate()
    {
        Set();
    }

    private void UpdateDashAttack()
    {
        if (dashStart > 0)
        {
            if (Time.time < dashStart + attackChargeTime)
            {
                OnCharge?.Invoke();

                movement.FaceToItem(Player.Instance.transform);
            }
            else if (Time.time >= dashStart + attackChargeTime
                && Time.time < dashStart + attackChargeTime + dashDuaration)
            {
                OnAttack?.Invoke();

                movement.SetVelocity(dashVelocity, (Vector2)dashDir);
                DetectAttack();
            }
            else
            {
                Set(isDefault: true);
            }
        }
    }

    public void Set(bool isDefault = false)
    {
        if (isDefault)
        {
            allowAttackDetection = false;

            movement.SetVelocityZero();
            dashStart = 0;
            dashDir = Vector3.zero;
        }
        else
        {
            allowAttackDetection = true;

            spriteEffect.TintSprite(spriteEffect.warningColor, 3);

            Vector3 playerPos = Player.Instance.transform.position;
            dashDir = playerPos - movement.RB.transform.position;
            dashStart = Time.time;
        }
    }

    public void DetectAttack()
    {
        if (allowAttackDetection)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + collideOffset, collideSize, 0, hostileLayer);
            if (colliders.Length > 0)
            {
                allowAttackDetection = false;

                IDamageable damageble = colliders[0].transform.parent.GetComponent<IDamageable>();
                if (damageble != null)
                {
                    damageble.Damage(attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.DrawWireCube(transform.position + collideOffset, collideSize);
        }
    }
}
