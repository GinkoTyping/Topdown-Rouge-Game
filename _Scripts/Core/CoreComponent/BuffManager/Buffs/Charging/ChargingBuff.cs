using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingBuff : Buff
{
    [SerializeField] public CharingBuffData data;

    private AbilityManager abilityManager;

    protected override void Start()
    {
        base.Start();

        abilityManager = buffManager.normalAttack.GetComponent<AbilityManager>();
    }

    public override void LogicUpdate()
    {
        float stack = Mathf.Floor(buffManager.continousAttackTime / data.timePerStack);
        if (stack <= data.maxStack && stack != currenrStack)
        {
            if (currenrStack != 0 && stack == 0f)
            {
                ResetBaseAttackInterval();
            }
            else
            {
                abilityManager.ModifyCooldown(abilityManager.totalCooldownTime * data.modifier);
            }

            currenrStack = stack;
        }
    }

    private void ResetBaseAttackInterval()
    {
        AttributeStat attackInterval = buffManager.stats.GetAttribute(AttributeType.AttackInterval);
        abilityManager.ModifyCooldown(attackInterval.CurrentValue);
    }
}
