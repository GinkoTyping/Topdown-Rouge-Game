using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemHoverAttribute : MonoBehaviour
{
    [SerializeField]
    private Color randomAttributeColor;
    [SerializeField]
    private Color baseAttributeColor;

    private TextMeshProUGUI textMesh;
    private SoretedAttribute soretedAttribute;
    private AttributeHelper helper;


    private void Awake()
    {
        soretedAttribute = new SoretedAttribute();
        helper = GameObject.Find("Helper").GetComponent<AttributeHelper>();
    }
    private void Start()
    {
        textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    public void Set(BonusAttribute attribute, bool isBaseAttribute = false)
    {
        if (attribute?.type == null)
        {
            return;
        }

        if (textMesh == null)
        {
            textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        }

        textMesh.color = isBaseAttribute 
            ? baseAttributeColor 
            : randomAttributeColor;

        string name = helper.ShortenAttributeName(attribute.type);

        if (soretedAttribute.intAttribute.Contains(attribute.type))
        {
            textMesh.text = isBaseAttribute 
                ? $"{name} {attribute.value}"
                : $"+{attribute.value} {name}";
        } 
        else if (soretedAttribute.floatAttribute.Contains(attribute.type))
        {
            textMesh.text = isBaseAttribute
                ? $"{name} {attribute.value * 100}%"
                : $"+{attribute.value * 100}% {name}";
        } else
        {
            Debug.LogError($"AttributeType Unmatched: {attribute.type}");
        }
    }
}
