using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemProp : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private List<AttributeType> plusAttribute = 
        new List<AttributeType> 
        { 
            AttributeType.Strength, 
            AttributeType.Intelligence, 
            AttributeType.Agility
        };
    private List<AttributeType> multiAttribute =
        new List<AttributeType>
        {
            AttributeType.CriticalChance,
            AttributeType.CriticalDamage,
        };
    private SoretedAttribute soretedAttribute;


    private void Awake()
    {
        soretedAttribute = new SoretedAttribute();
    }
    private void Start()
    {
        textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    public void Set(BonusAttribute attribute)
    {
        if (attribute?.type == null)
        {
            return;
        }

        if (textMesh == null)
        {
            textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        }

        string name = ShortenAttributeName(attribute.type);

        if (soretedAttribute.intAttribute.Contains(attribute.type))
        {
            textMesh.text = $"{name} +{attribute.value}";
        } 
        else if (soretedAttribute.floatAttribute.Contains(attribute.type))
        {
            textMesh.text = $"{name} +{attribute.value * 100}%";
        } else
        {
            Debug.LogError($"AttributeType Unmatched: {attribute.type}");
        }
    }

    private string ShortenAttributeName(AttributeType attribute)
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
        else
        {
            return attribute.ToString();
        }
    }
}
