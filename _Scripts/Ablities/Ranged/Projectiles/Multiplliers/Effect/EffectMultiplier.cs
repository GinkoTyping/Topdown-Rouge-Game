using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectMultiplier : ProjectileMultiplier
{
    [SerializeField] private DamageEffectMultiplierDataSO data;
    [SerializeField] private EffectToColor[] effectColors;
    
    private PossibilityHelper helper;

    [Serializable]
    private class EffectToColor
    {
        public DamageEffect effect;
        public Color color;
    }

    private void Awake()
    {
        helper = new PossibilityHelper();
    }

    public override Projectile Apply(Projectile projectile)
    {
        DamageEffect defaultEffect = DamageEffect.Normal;
        if (data.mixed && data.effectRanges.Length > 0)
        {
            int index = helper.GetAmongItems(data.effectRanges.Select(item => item.possibility).ToArray());
            if (index != -1)
            {
                defaultEffect = data.effectRanges[index].effect;
            }
        } else
        {
            defaultEffect = data.effect;
        }

        projectile.SetDamageEffect(defaultEffect);
        projectile.spriteRenderer.color = GetEffectColor(defaultEffect);

        return projectile;
    }

    private Color GetEffectColor(DamageEffect damageEffect)
    {
        Color color = Color.white;
        foreach (EffectToColor item in effectColors)
        {
            if (damageEffect == item.effect)
            {
                color = item.color;
                break;
            }
        }

        return color;
    }
}
