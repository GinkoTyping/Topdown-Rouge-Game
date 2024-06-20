using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector3 detectionOffset;
    [SerializeField] private Vector3 detectionSize;
    [SerializeField] private LayerMask detectionLayer;

    private float velocity;
    private float damageAmount;
    private Vector3 direction;

    private float startTime;
    private float duaration;

    private Rigidbody rb;
    private PoolManager poolManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    public void Set(Vector3 position, Vector3 direction, float damageAmount)
    {
        this.direction = direction.normalized;
        this.damageAmount = damageAmount;

        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0 , Vector2.SignedAngle(Vector2.right, this.direction));
    }

    public void Fire(float velocity, float duaration)
    {
        this.velocity = velocity;
        this.duaration = duaration;

        startTime = Time.time;
    }

    public void SetPool(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    private void DetectCollision()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position + detectionOffset, detectionSize, detectionLayer);

        IDamageable damageable = collider?.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(damageAmount);
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
}
