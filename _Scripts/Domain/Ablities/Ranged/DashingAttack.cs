using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System;
using UnityEngine;

public class DashingAttack : BaseAbility
{
    [Header("Dashing Attack")]
    [SerializeField] private Vector2 collideSize;
    [SerializeField] private Vector3 collideOffset;
    [SerializeField] private float attackChargeTime;
    [SerializeField] private float dashDuaration;
    [SerializeField] private float dashVelocity;

    private Core core;
    private Movement movement;
    private SpriteEffect spriteEffect;

    private float dashStart;
    private Vector3 dashDir;
    private bool allowAttackDetection;

    protected override void Awake()
    {
        base.Awake();

        core = GetComponentInParent<Core>();
    }
    
    protected override void Start()
    {
        base.Start();

        movement = core.GetCoreComponent<Movement>();
        spriteEffect = core.GetCoreComponent<SpriteEffect>();
    }

    private void Update()
    {
        UpdateDashAttack();
    }

    public override void Activate()
    {
        allowAttackDetection = true;

        spriteEffect.TintSprite(spriteEffect.warningColor, 3);
        Vector3 playerPos = Player.Instance.transform.position;
        dashDir = playerPos - movement.RB.transform.position;
        dashStart = Time.time;
    }

    private void UpdateDashAttack()
    {
        if (dashStart > 0)
        {
            if (Time.time < dashStart + attackChargeTime)
            {
                UpdateAnim(AnimBoolName.Charge);
                movement.FaceToItem(Player.Instance.transform);
            }
            else if (Time.time >= dashStart + attackChargeTime
                && Time.time < dashStart + attackChargeTime + dashDuaration)
            {
                UpdateAnim(AnimBoolName.RangedAttack);
                movement.SetVelocity(dashVelocity, (Vector2)dashDir);
                DetectDamegable();
            }
            else
            {
                OnDashEnd();
            }
        }
    }

    private void OnDashEnd()
    {
        allowAttackDetection = false;

        movement.SetVelocityZero();
        dashStart = 0;
        dashDir = Vector3.zero;
    }

    private void DetectDamegable()
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
