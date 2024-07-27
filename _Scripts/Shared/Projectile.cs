using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isDebug;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private PoolManager poolManager;

    private Vector3 collisionOffset;
    private Vector3 collisionSize;
    private Vector3 direction;
    private LayerMask collisionLayer;
    private float velocity;
    private float damageAmount;
    private float startTime;
    private float duaration;

    private Entity senderEntity;
    private Stats statsComp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (startTime != 0 && Time.time < startTime + duaration)
        {
            rb.velocity = new Vector3(direction.x * velocity, direction.y * velocity, 0);

            DetectCollision();
        } else
        {
            rb.velocity = Vector3.zero;
            startTime = 0;

            DestroySelf();
        }
    }

    public void Set(ProjectileDataSO data, Vector3 position, LayerMask layer, Entity sender = null)
    {
        damageAmount = data.damageAmount;
        spriteRenderer.sprite = data.sprite;
        spriteRenderer.material = data.glowMaterial;
        velocity = data.velocity;
        duaration = data.duaration;
        collisionOffset = data.collisionOffset;
        collisionSize = data.collisionSize;

        collisionLayer = layer;
        senderEntity = sender;
        statsComp = sender.Core.GetCoreComponent<Stats>();
        transform.position = position;
    }

    public void Fire(Vector3 fireDirection)
    {
        direction = fireDirection.normalized;
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, fireDirection));
        startTime = Time.time;
    }

    public void SetPool(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    private void DetectCollision()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position + collisionOffset, collisionSize, 0, collisionLayer);

        IDamageable damageable = collider?.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            DamageDetail damageDetail = new DamageDetail(damageAmount, statsComp, DamageEffect.Normal);
            damageable.Damage(damageDetail, senderEntity);
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        if (poolManager == null)
        {
            Destroy(gameObject);
        }
        else
        {
            poolManager.Pool.Release(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.DrawWireCube(collisionOffset, collisionSize);
        }
    }
}
