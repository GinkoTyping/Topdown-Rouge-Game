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

    public void Init()
    {
        CurrentValue = InitValue;
    }
}
