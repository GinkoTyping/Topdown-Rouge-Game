using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDamageEffectMultiplierData", menuName = "Data/Projectile Multiplier/Damage Effect")]
public class DamageEffectMultiplierDataSO : ScriptableObject
{
    [SerializeField] public bool mixed;
    [SerializeField] public DamageEffect effect;
    [SerializeField] public EffectItem[] effectRanges;

    [Serializable]
    public class EffectItem
    {
        public float possibility;
        public DamageEffect effect;
    }
}


