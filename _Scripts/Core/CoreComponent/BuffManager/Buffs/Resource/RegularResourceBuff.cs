using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularResourceBuff : ResourceBuff
{
    protected override void ApplyBuffEffect()
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
