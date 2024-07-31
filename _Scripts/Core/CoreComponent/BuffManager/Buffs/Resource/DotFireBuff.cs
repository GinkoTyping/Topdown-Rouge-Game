using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotFireBuff : ResourceBuff
{
    protected override void ApplyBuffEffect()
    {
        if (calculateTime >= resourceBuffData.perTime)
        {
            calculateTime = 0;

            bool playSound = Random.Range(0f, 1f) <= 0.3f;
            damageReceiverComp.Damage(
                new DamageDetail(
                    Mathf.Abs(resourceBuffData.perValue),
                    playSound: playSound,
                    showHitParticle: false
                )
            );
        }

        if (buffManager.continousMovingTime > 0)
        {
            calculateTime += Time.deltaTime;
        } else if (buffManager.continousMovingTime <= 0 )
        {
            calculateTime = 0;
        }
    }
}
