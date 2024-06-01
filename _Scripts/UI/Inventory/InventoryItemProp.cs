using System.Collections;
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
    

    private void Start()
    {
        textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    public void Set(BonusAttribute attribute)
    {
        if (textMesh == null)
        {
            textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        }
        if (plusAttribute.Contains(attribute.type))
        {
            textMesh.text = $"{attribute.type} +{attribute.value}";
        } 
        else if (multiAttribute.Contains(attribute.type))
        {
            textMesh.text = $"{attribute.type} +{attribute.value}%";
        }
    }
}
