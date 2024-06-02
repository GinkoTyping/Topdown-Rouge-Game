using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentItem : InventoryItem
{
    private AttributeType[] baseAttributes;
    private AttributeType[] criticalAttributes;
    private AttributeType[] armorAttributes;

    public BonusAttribute[] bonusAttributes;

    protected override void Awake()
    {
        base.Awake();

        SoretedAttribute soretedAttribute = new SoretedAttribute();
        baseAttributes = soretedAttribute.BaseAttributes;
        criticalAttributes = soretedAttribute.CriticalAttributes;
        armorAttributes = soretedAttribute.ArmorAttributes;
    }

    public void SetBonusAttribute(BonusAttribute[] data = null)
    {
        if (data != null)
        {
            bonusAttributes = data;
        } else
        {
            bonusAttributes = bonusAttributes.Concat(SetBaseAttribute(rarity)).ToArray();
        }
    }

    private BonusAttribute[] SetBaseAttribute(Rarity rarity)
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

        return output;
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
}
