using Ginko.CoreSystem;
using UnityEngine;

public class ChargingBuff : Buff
{
    private AbilityManager abilityManager;
    private CharingBuffDataSO chargeBuffData;

    private AttributeStat stat;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ApplyBuffEffect(0);
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
            SwitchBuffIcon(true);
        }
    }

    private float GetBuffStackCount()
    {
        float time = buffManager.GetContinousTime(chargeBuffData.chargingBaseType, startTime);

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
            abilityManager.ModifyCooldown(abilityManager.totalCooldownTime * ( 1 + chargeBuffData.modifier));
        }
    }
    
    private void ResetBaseAttackInterval()
    {
        AttributeStat attackInterval = buffManager.stats.GetAttribute(AttributeType.AttackInterval);
        abilityManager.ModifyCooldown(attackInterval.CurrentValue);
    }

    protected override void UpdateSpecificBuffData()
    {
        chargeBuffData = data as CharingBuffDataSO;
    }

    public override string GetDesc(bool hasDurationText = false, BaseBuffDataSO specificData = null)
    {
        base.GetDesc();

        CharingBuffDataSO dataToUse = chargeBuffData;
        if (specificData != null)
        {
            dataToUse = specificData as CharingBuffDataSO;
        }

        string moduleDesc = dataToUse.desc;


        moduleDesc = moduleDesc.Replace("{$1}", GetSpecialText(dataToUse.timePerStack));
        moduleDesc = moduleDesc.Replace("{$2}", GetSpecialText(dataToUse.chargingBaseType));
        moduleDesc = moduleDesc.Replace("{$3}", GetSpecialText(attributeHelper.ShortenAttributeName(dataToUse.chargingTargetAttribute), attributeHelper.GetAttributeColor(dataToUse.chargingTargetAttribute)));
        moduleDesc = moduleDesc.Replace("{$4}", GetSpecialText($"{Mathf.Abs(dataToUse.modifier) * 100}%"));
        moduleDesc = moduleDesc.Replace("{$5}", GetSpecialText(dataToUse.maxStack, underline: true));

        return moduleDesc;
    }
}
