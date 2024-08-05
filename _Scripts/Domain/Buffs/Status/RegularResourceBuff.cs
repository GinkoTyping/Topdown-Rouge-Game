using Ginko.CoreSystem;
using UnityEngine;

// TODO: 修改类的名称
public class RegularResourceBuff : ResourceBuff
{
    private bool hasUpdatedAtOneTime;
    private AttributeStat targetAttribute;

    protected override void OnEnable()
    {
        base.OnEnable();

        hasUpdatedAtOneTime = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (statusBuffData != null && !statusBuffData.isUpdateOverTime)
        {
            targetAttribute.Decrease(statusBuffData.staticValue);
        }
    }

    protected override void ApplyBuffEffect()
    {
        if (statusBuffData.isUpdateOverTime)
        {
            UpdateStatOverTime();
        } else
        {
            if (!hasUpdatedAtOneTime)
            {
                hasUpdatedAtOneTime = true;

                targetAttribute = buffManager.stats.GetAttribute(statusBuffData.attributeType);
                targetAttribute.Increase(statusBuffData.staticValue);
            }
        }
    }

    private void UpdateStatOverTime()
    {
        calculateTime += Time.deltaTime;

        if (calculateTime >= statusBuffData.perTime)
        {
            calculateTime -= statusBuffData.perTime;

            if (!statusBuffData.isAttribute 
                && statusBuffData.resourceType == ResourceType.Health
                && statusBuffData.perValue < 0)
            {
                bool playSound = Random.Range(0f, 1f) <= 0.3f;
                damageReceiverComp.Damage(
                    new DamageDetail(
                        Mathf.Abs(statusBuffData.perValue),
                        playSound: playSound,
                        showHitParticle: false
                    )
                );
            }
            else
            {
                stat.Increase(statusBuffData.perValue);
            }
        }
    }
}
