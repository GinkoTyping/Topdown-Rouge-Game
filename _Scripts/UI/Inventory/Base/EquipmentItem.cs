using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Rendering.CameraUI;

public class EquipmentItem : InventoryItem
{
    private SortedEquipmentType sortedEquipmentType;
    private SoretedAttribute soretedAttribute;

    public BonusAttribute[] baseAttributes;
    public BonusAttribute[] bonusAttributes;

    protected override void Awake()
    {
        base.Awake();
        sortedEquipmentType = new SortedEquipmentType();
        soretedAttribute = new SoretedAttribute();

    }

    public override void Set(InventoryItemSO itemSO, RectTransform parent, Rarity rarity, int tileSize)
    {
        base.Set(itemSO, parent, rarity, tileSize);

        SetBaseAttribute(itemSO as EquipmentItemSO);
    }

    public void SetBaseAttribute(EquipmentItemSO data)
    {
        BonusAttribute[] attributes = data.baseAttributes.Where(attrubuteInfo => attrubuteInfo.rarity == rarity).ToArray()[0].attributes;
        if (attributes.Length > 0)
        {
            baseAttributes = attributes;
        }
    }

    public void SetBonusAttribute(BonusAttribute[] customData = null)
    {
        if (customData != null)
        {
            bonusAttributes = customData;
        } else
        {
            EquipmentItemSO equipmentData = data as EquipmentItemSO;

            if (sortedEquipmentType.JewelryEquipments.Contains(equipmentData.equipmentType))
            {
                SetBaseAttributes(rarity);
            }
            else if (sortedEquipmentType.MainArmorEquipments.Contains(equipmentData.equipmentType))
            {
                SetArmorAttributes();
            }
        }
    }

    private void SetArmorAttributes()
    {
        BonusAttribute[] output = null;
        float[] values = GetArmorAttributeValue();

        if (rarity == Rarity.Common || rarity == Rarity.Uncommon)
        {
            output = new BonusAttribute[]
            {
                new BonusAttribute(AttributeType.DamageReduction, values[0]),
            };
        } else
        {
            output = new BonusAttribute[] 
            {
                new BonusAttribute(AttributeType.DamageReduction, values[0]),
                new BonusAttribute(AttributeType.MoveSpeed, values[1]),
            };
        }

        UpdateBonuseAttribute(output);
    }
    private void SetBaseAttributes(Rarity rarity)
    {
        int attributeCount = 0;
        if (rarity == Rarity.Common || rarity == Rarity.Uncommon)
        {
            attributeCount = 1;
        }
        else
        {
            attributeCount = 2;
        }

        List<AttributeType> attributes = new List<AttributeType>();
        int randomInt = Random.Range(0, 3);
        AttributeType[] baseAttributes = soretedAttribute.BaseAttributes;
        attributes.Add(baseAttributes[randomInt]);
        if (attributeCount == 2)
        {
            attributes.Add(baseAttributes[randomInt + 1 >= baseAttributes.Length ? 0 : randomInt + 1]);
        }

        BonusAttribute[] output = new BonusAttribute[attributeCount];

        for (int i = 0; i < attributes.Count; i++)
        {


            output[i] = new BonusAttribute(attributes[i], GetBaseAttributeValue(rarity));
        }

        UpdateBonuseAttribute(output);
    }

    private void UpdateBonuseAttribute(BonusAttribute[] attributes)
    {
        bonusAttributes = bonusAttributes.Concat(attributes).ToArray();
    }

    private int GetBaseAttributeValue(Rarity rarity)
    {
        int output = 0;
        switch (rarity)
        {
            case Rarity.Common:
                output = 1;
                break;
            case Rarity.Uncommon:
                output = Random.Range(2, 4);
                break;
            case Rarity.Rare:
                output = Random.Range(2, 4);
                break;
            case Rarity.Legend:
                output = Random.Range(5, 8);
                break;
        }

        return output;
    }
    private float[] GetArmorAttributeValue()
    {
        float damageReduction = 0;
        float moveSpeed = 0;
        switch (rarity)
        {
            case Rarity.Common:
                damageReduction = Random.Range(2, 6);
                moveSpeed = Random.Range(1, 4);
                break;
            case Rarity.Uncommon:
                damageReduction = Random.Range(8, 13);
                moveSpeed = Random.Range(5, 8);
                break;
            case Rarity.Rare:
                damageReduction = Random.Range(15, 20);
                moveSpeed = Random.Range(10, 13);
                break;
            case Rarity.Legend:
                damageReduction = Random.Range(25, 40);
                moveSpeed = Random.Range(15, 21);
                break;
        }

        float[] output = new float[]
        {
            damageReduction / 100,
            moveSpeed / 100,
        };

        return output;
    }
}
