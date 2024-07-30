using Ginko.CoreSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSteal : Buff
{
    private LifeStealingBuffDataSO lifeStealingBuffData;

    private ResourceStat health;

    public override void Init()
    {
        base.Init();

        PlayerStats playerStats = buffManager.stats;
        health = playerStats.Health;
        SwitchBuffIcon(true);
    }

    private void OnDisable()
    {
        SwitchBuffIcon(false);
    }

    public override void LogicUpdate()
    {
        
    }

    public override void RefreshBuff()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateSpecificBuffData()
    {
        lifeStealingBuffData = data as LifeStealingBuffDataSO;
    }

    public void Activate()
    {
        if (lifeStealingBuffData.type == LifeStealType.Amount)
        {
            health.Increase(lifeStealingBuffData.value);
        } else if (lifeStealingBuffData.type == LifeStealType.Percentage)
        {
            health.Increase(health.MaxValue * lifeStealingBuffData.value);
        }
    }
}