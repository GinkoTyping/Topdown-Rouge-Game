using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CoreComponent
{
    public float continousAttackTime;
    public float continousStayingTime;
    public float continousMovingTime;
    private bool isStaying = true;

    [SerializeField] public PoolManager buffsPool;
    [SerializeField] private float effectiveTime = 0.05f;

    public List<Buff> buffList;
    public event Action<int> OnBuffChange;

    public PlayerStats stats { get; private set; }
    public NormalAttack normalAttack { get; private set; }
    private Player player;

    private Timer effectiveTimer;
    private bool isEffectiveContinous;

    private void Start()
    {
        player = Core.GetComponentInParent<Player>();
        stats = Core.GetCoreComponent<PlayerStats>();
        normalAttack = Core.GetCoreComponent<NormalAttack>();

        effectiveTimer = new Timer(effectiveTime);
        effectiveTimer.OnTimerDone += HandleEffectiveTimerDone;

        foreach (Buff buff in buffList)
        {
            buff.Init();
        }
    }

    public void Add(Buff buff)
    {
        buffList.Add(buff);
        OnBuffChange?.Invoke(buffList.Count);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateContinousAttackTime();
        UpdateIsStay();
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

    private void UpdateIsStay()
    {
        effectiveTimer.Tick();

        if (isStaying)
        {
            if (player.Movement.CurrentVelocity.sqrMagnitude > 0)
            {
                isStaying = false;
            }
        } else if (!isStaying && player.Movement.CurrentVelocity.sqrMagnitude == 0 &&!effectiveTimer.isActive)
        {
            effectiveTimer.StartTimer();
        }

        if (isStaying)
        {
            continousStayingTime += Time.deltaTime;
            continousMovingTime = 0;
        } else
        {
            continousMovingTime += Time.deltaTime;
            continousStayingTime = 0;
        }
    }

    private void HandleEffectiveTimerDone()
    {
        isStaying = player.Movement.CurrentVelocity.sqrMagnitude == 0;
    }
}
