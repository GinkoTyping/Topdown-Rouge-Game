using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoretedAttribute
{
    public AttributeType[] PropertyAttributes = new AttributeType[]
    {
        AttributeType.Strength,
        AttributeType.Intelligence, 
        AttributeType.Agility
    };

    public AttributeType[] CriticalAttributes = new AttributeType[]
    {
        AttributeType.CriticalChance,
        AttributeType.CriticalDamage
    };

    public AttributeType[] ArmorAttributes = new AttributeType[]
    {
        AttributeType.DamageReduction,
        AttributeType.MoveSpeed
    };

    public AttributeType[] intAttribute = new AttributeType[]
    {
        AttributeType.MaxHealth,
        AttributeType.WeaponDamage,
        AttributeType.HealthRegeneration,
        AttributeType.HealthRecovery,


        AttributeType.Strength,
        AttributeType.Intelligence,
        AttributeType.Agility,
    };

    public AttributeType[] floatAttribute = new AttributeType[]
    {
        AttributeType.CriticalChance,
        AttributeType.CriticalDamage,
        AttributeType.DamageReduction,
        AttributeType.MoveSpeed,
    };
}

public enum AttributeType
{
    MaxHealth,
    WeaponDamage,
    
    Strength,
    Intelligence,
    Agility,

    CriticalChance,
    CriticalDamage,
    DamageReduction,
    MoveSpeed,

    HealthRegeneration,
    HealthRecovery,

    AttackInterval,
}
