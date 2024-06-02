using Ginko.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AttributeStat : BaseStat
{
    [SerializeField]
    public AttributeType type;
    [SerializeField]
    private float initValue;

    public void Init()
    {
        CurrentValue = initValue;
    }
}
