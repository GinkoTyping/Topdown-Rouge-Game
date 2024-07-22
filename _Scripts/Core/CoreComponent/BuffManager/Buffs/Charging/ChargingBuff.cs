using Ginko.CoreSystem;
using UnityEngine;

public class ChargingBuff : Buff
{
    private AbilityManager abilityManager;
    private CharingBuffDataSO chargeBuffData;

    private AttributeStat stat;

    private void Awake()
    {
        chargeBuffData = data as CharingBuffDataSO;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void LogicUpdate()
    {
        float stack = GetBuffStackCount();
        if (stack <= chargeBuffData.maxStack && stack != currenrStack)
        {
            ApplyBuffEffect(stack);

            currenrStack = stack;
        }

        if (currenrStack == 0 && currentBuffIcon != null)
        {
            currentBuffIcon = null;
        }

        if (currenrStack > 0 && currentBuffIcon == null)
        {
            InstantiateBuffIcon();
        }
    }

    private float GetBuffStackCount()
    {
        float time = 0;
        switch (chargeBuffData.chargingBaseType)
        {
            case ChargingBaseType.ContinousAttack:
                {
                    time = buffManager.continousAttackTime;
                    break;
                }
            case ChargingBaseType.Moving:
                {
                    time = buffManager.continousMovingTime;
                    break;
                }
            case ChargingBaseType.NotMoving:
                {
                    time = buffManager.continousStayingTime;
                    break;
                }
            default:
                {
                    Debug.LogError($"ChargingBaseType '{chargeBuffData.chargingBaseType}' Not Matched");
                    break;
                }
        }

        return Mathf.Floor(time / chargeBuffData.timePerStack);
    }

    private void ApplyBuffEffect(float stack)
    {
        switch (chargeBuffData.chargingTargetAttribute)
        {
            case AttributeType.AttackInterval:
                {
                    ApplyAttackIntervalBuff(stack);
                    break;
                }

            case AttributeType.CriticalChance:
                {
                    ApplyBaseAttributeBuff(stack);
                    break;
                }
            case AttributeType.DamageReduction:
                {
                    ApplyBaseAttributeBuff(stack);
                    break;
                }
            default:
                {
                    Debug.LogError($"Unmatched AttributeType '{chargeBuffData.chargingTargetAttribute}'");
                    break;
                }
        }
    }

    private void ApplyBaseAttributeBuff(float stack)
    {
        if (stat == null)
        {
            stat = buffManager.stats.GetAttribute(chargeBuffData.chargingTargetAttribute);
        }

        if (currenrStack != 0 && stack == 0f)
        {
            stat.Decrease(currenrStack * chargeBuffData.modifier);
        } else
        {
            stat.Increase(chargeBuffData.modifier);
        }
    }

    private void ApplyAttackIntervalBuff(float stack)
    {
        if (abilityManager == null)
        {
            abilityManager = buffManager.normalAttack.GetComponent<AbilityManager>();
        }

        if (currenrStack != 0 && stack == 0f)
        {
            currentBuffIcon = null;
            ResetBaseAttackInterval();
        }
        else
        {
            abilityManager.ModifyCooldown(abilityManager.totalCooldownTime * chargeBuffData.modifier);
        }
    }
    
    private void ResetBaseAttackInterval()
    {
        AttributeStat attackInterval = buffManager.stats.GetAttribute(AttributeType.AttackInterval);
        abilityManager.ModifyCooldown(attackInterval.CurrentValue);
    }
}
