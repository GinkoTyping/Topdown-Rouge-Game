using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularAttributeBuff : ResourceBuff
{
    private bool hasUpdatedAtOneTime;
    private AttributeStat targetAttribute;

    protected override void OnEnable()
    {
        base.OnEnable();

        targetAttribute = buffManager.stats.GetAttribute(resourceBuffData.attributeType);
        targetAttribute.Increase(resourceBuffData.staticValue);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        targetAttribute.Decrease(resourceBuffData.staticValue);
    }

    protected override void ApplyBuffEffect()
    {
        
    }
}
