using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CoreComponent
{
    public float continousAttackTime;
    public float continousStayingTime;
    public float continousMovingTime;

    [SerializeField] public PoolManager buffsPool;
    public List<Buff> buffList;

    public PlayerStats stats { get; private set; }
    public NormalAttack normalAttack { get; private set; }
    private Player player;

    private void Start()
    {
        player = Core.GetComponentInParent<Player>();
        stats = Core.GetCoreComponent<PlayerStats>();
        normalAttack = Core.GetCoreComponent<NormalAttack>();

        foreach (Buff buff in buffList)
        {
            buff.Init();
        }
    }

    public void Add(Buff buff)
    {
        buffList.Add(buff);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateContinousAttackTime();
        UpdateMovingTime();
        UpdateStayingTime();

        UpdateBuffs();
    }

    private void UpdateBuffs()
    {
        foreach (Buff buff in buffList)
        {
            buff.LogicUpdate();
        }
    }

    private void UpdateContinousAttackTime()
    {
        if (player.IsAttackInput)
        {
            continousAttackTime += Time.deltaTime;
        }
        else if (continousAttackTime != 0)
        {
            continousAttackTime = 0;
        }
    }

    private void UpdateMovingTime()
    {
        if (player.Movement.CurrentVelocity != Vector2.zero)
        {
            continousMovingTime += Time.deltaTime;
        } else if (continousMovingTime != 0)
        {
            continousMovingTime = 0;
        }
    }

    private void UpdateStayingTime()
    {
        if (player.Movement.CurrentVelocity == Vector2.zero)
        {
            continousStayingTime += Time.deltaTime;
        }
        else if (continousStayingTime != 0)
        {
            continousStayingTime = 0;
        }
    }
}
