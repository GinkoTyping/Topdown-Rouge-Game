using Ginko.CoreSystem;
using Shared.Utilities;
using UnityEngine;

public class LaunchExplosion : BaseAbility
{
    [Header("Launch Explosion")]
    [SerializeField] private bool isDestructSelf;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionScale;
    [SerializeField] private float explosionDelayTime;
    [SerializeField] private float damegeDelayTime;
    [SerializeField] private GameObject explosionParticle;

    private PoolHelper poolHelper;
    private PoolManager poolManager;
    private Transform explosionPoolContainer;
    private SpriteEffect spriteEffect;
    private Timer explosionTimer;
    private Timer damageTimer;

    protected override void Awake()
    {
        base.Awake();

        explosionPoolContainer = GameObject.Find("Containers").transform;

        poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
        poolManager = poolHelper.GetPoolByPrefab(explosionPoolContainer, explosionParticle);

        explosionTimer = new Timer(explosionDelayTime);
        damageTimer = new Timer(damegeDelayTime);
    }

    protected override void Start()
    {
        base.Start();

        spriteEffect = entity.Core.GetCoreComponent<SpriteEffect>();
    }

    protected override void Update()
    {
        base.Update();

        explosionTimer.Tick();
        damageTimer.Tick();
    }

    public override void Activate()
    {
        spriteEffect.TintSprite(spriteEffect.warningColor, 6);

        explosionTimer.OnTimerDone += HandleExplode;
        explosionTimer.StartTimer();
    }

    private void HandleExplode()
    {
        explosionTimer.OnTimerDone -= HandleExplode;

        InitiateExplosion();
        InitiateDamege();
        DestuctSelf();
    }

    private void InitiateExplosion()
    {
        GameObject explosionGO = poolManager.Pool.Get();
        ParticleSystem particleSystem = explosionGO.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particleSystem.main;
        main.simulationSpeed = 0.5f;

        explosionGO.transform.localScale = Vector3.one * explosionScale;

        explosionGO.transform.position = transform.position;
    }

    private void InitiateDamege()
    {
        damageTimer.OnTimerDone += DetectDamega;
        damageTimer.StartTimer();
    }

    private void DetectDamega()
    {
        damageTimer.OnTimerDone -= DetectDamega;

        Collider2D[] colliders =  Physics2D.OverlapCircleAll(transform.position, explosionRadius, hostileLayer);
        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders) 
            {
                collider.GetComponentInParent<IDamageable>()?.Damage(attackDamage);
            }
        }
    }

    private void DestuctSelf()
    {
        if (isDestructSelf && stateMachine.CurrentState != entity.DeathState)
        {

            foreach (BaseAbility ability in entity.DeathState.deathrattles)
            {
                if (ability.GetComponent<LaunchExplosion>() != null)
                {
                    ability.SwitchAbleToActivate(false);
                }
            }
            stateMachine.ChangeState(entity.DeathState);
        }
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
