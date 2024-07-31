using Ginko.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TerrainTools;
using UnityEngine;

public class AttributeHelper : MonoBehaviour
{
    [SerializeField]
    public Color[] colors;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private List<AttributeToColor> attributesToColors;

    [Serializable]
    public class AttributeToColor
    {
        public Color color;
        public AttributeType type;
    }

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
            case AttributeType.LifeSteal:
                name = "Life Steal";
                break;
            default:
                break;
        }

        return name;
    }

    public string GetAttributeColor(ResourceType resourceType)
    {
        if (resourceType == ResourceType.Health)
        {
            return GetAttributeColor(AttributeType.MaxHealth);
        }
        // Blue
        return "#0096ff";
    }

    public string GetAttributeColor(AttributeType attributeType)
    {
        string color = "#ffffff";

        AttributeToColor attributeToColor = attributesToColors.Where(item => item.type == attributeType).FirstOrDefault();

        if(attributeToColor != null)
        {
            color = $"#{ColorUtility.ToHtmlStringRGB(attributeToColor.color)}";
        }

        return color;
    }

    public Color GetAttributeColor(Rarity rarity, bool isTransparent)
    {
        Color color = colors[(int)rarity];

        if (!isTransparent)
        {
            color = new Color(color.r, color.g, color.b, 1);
        }

        return color;
    }

    public string GetAttributeColor(Rarity rarity)
    {
        Color color = GetAttributeColor(rarity, false);
        return ColorToHex(color);
    }

    public string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        return hex;
    }

    public Material GetRarityColorMaterial(Rarity rarity)
    {
        return materials[(int)rarity];
    }
}
