using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipementEffect
{
    protected EffectType effectType;
    public EquipementEffect(EffectType effectType)
    {
        this.effectType = effectType;
    }
}

public enum EffectType
{
    Attack,
    Buff,
}
