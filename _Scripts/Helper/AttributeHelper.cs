using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeHelper : MonoBehaviour
{
    [SerializeField]
    public Color[] colors;
    public string ShortenAttributeName(AttributeType attribute)
    {
        if (attribute == AttributeType.CriticalChance)
        {
            return "Crit. Chance";
        }
        else if (attribute == AttributeType.CriticalDamage)
        {
            return "Crit. Damage";
        }
        else if (attribute == AttributeType.DamageReduction)
        {
            return "DMG Reduction";
        }
        else if (attribute == AttributeType.MoveSpeed)
        {
            return "Move Speed";
        }
        else
        {
            return attribute.ToString();
        }
    }

    public Color GetAttributeColor(Rarity rarity, bool isTransparent = true)
    {
        Color color = colors[(int)rarity];

        if (!isTransparent)
        {
            color = new Color(color.r, color.g, color.b, 1);
        }

        return color;
    }
}
