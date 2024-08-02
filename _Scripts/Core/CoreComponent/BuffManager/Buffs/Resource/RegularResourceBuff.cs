using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (resourceBuffData != null && !resourceBuffData.isUpdateOverTime)
        {
            targetAttribute.Decrease(resourceBuffData.staticValue);
        }
    }

    protected override void ApplyBuffEffect()
    {
        if (resourceBuffData.isUpdateOverTime)
        {
            UpdateStatOverTime();
        } else
        {
            if (!hasUpdatedAtOneTime)
            {
                hasUpdatedAtOneTime = true;

                targetAttribute = buffManager.stats.GetAttribute(resourceBuffData.attributeType);
                targetAttribute.Increase(resourceBuffData.staticValue);
            }
        }
    }

    private void UpdateStatOverTime()
    {
        if (calculateTime >= resourceBuffData.perTime)
        {
            calculateTime -= resourceBuffData.perTime;

            if (resourceBuffData.perValue < 0)
            {
                bool playSound = Random.Range(0f, 1f) <= 0.3f;
                damageReceiverComp.Damage(
                    new DamageDetail(
                        Mathf.Abs(resourceBuffData.perValue),
                        playSound: playSound,
                        showHitParticle: false
                    )
                );
            }
            else
            {
                resourceStat.Increase(resourceBuffData.perValue);
            }
        }

        calculateTime += Time.deltaTime;
    }
}
