using Ginko.CoreSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float amount, bool isCritical = false, Entity sender = null);
    void Damage(DamageDetail damageDetail, Entity sender = null);
}

public class DamageDetail
{
    public float amount;
    public bool isCritical;
    public AttributeStat criticalChance;
    public AttributeStat criticalDamage;
    public DamageEffect damageEffect;

    public DamageDetail(float amount, Stats stats = null, DamageEffect damageEffect = DamageEffect.Normal)
    {
        this.damageEffect = damageEffect;

        if (stats != null)
        {
            criticalChance = stats.GetAttribute(AttributeType.CriticalChance);
            criticalDamage = stats.GetAttribute(AttributeType.CriticalDamage);
        }

        if (criticalChance != null && criticalDamage != null)
        {
            PossibilityHelper helper = new PossibilityHelper();
            isCritical = helper.GetChance(criticalChance.CurrentValue);
            this.amount = isCritical ? amount * criticalDamage.CurrentValue : amount;
        }
        else
        {
            this.amount = amount;
        }
    }
}

public enum DamageEffect
{
    Normal,
    Fire,
    Ice,
    Toxic
}

