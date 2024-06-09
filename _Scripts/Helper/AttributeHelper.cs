using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeHelper : MonoBehaviour
{
    [SerializeField]
    public Color[] colors;
    [SerializeField]
    private Material[] materials;

    public string ShortenAttributeName(AttributeType attribute)
    {
        string name = attribute.ToString();
        switch (attribute)
        {
            case AttributeType.MaxHealth:
                name = "Max Health";
                break;
            case AttributeType.WeaponDamage:
                name = "Weapon Damage";
                break;
            case AttributeType.Strength:
                break;
            case AttributeType.Intelligence:
                break;
            case AttributeType.Agility:
                break;
            case AttributeType.CriticalChance:
                name = "Crit. Chance";
                break;
            case AttributeType.CriticalDamage:
                name = "Crit. DMG.";
                break;
            case AttributeType.DamageReduction:
                name = "DMG. Reduction";
                break;
            case AttributeType.MoveSpeed:
                name = "Move Speed";
                break;
            case AttributeType.HealthRegeneration:
                name = "HP Regeneration";
                break;
            case AttributeType.HealthRecovery:
                name = "HP Recovery";
                break;
            default:
                break;
        }

        return name;
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

    public Material GetRarityColorMaterial(Rarity rarity)
    {
        return materials[(int)rarity];
    }
}
