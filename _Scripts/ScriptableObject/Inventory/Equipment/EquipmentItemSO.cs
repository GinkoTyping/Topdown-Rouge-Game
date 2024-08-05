using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentItemSO : InventoryItemSO
{
    public EquipmentType equipmentType;
}

public enum EquipmentType
{
    Weapon,
    Head,
    Body,
    Hand,
    Leg,
    Feet,
    Necklace,
    Ring,
}

public class SortedEquipmentType
{
    public EquipmentType[] MainArmorEquipments = new EquipmentType[]
    {
        EquipmentType.Body,
        EquipmentType.Leg,
    };

    public EquipmentType[] SubArmorEquipments = new EquipmentType[]
    {
        EquipmentType.Head,
        EquipmentType.Hand,
        EquipmentType.Feet,
    };

    public EquipmentType[] JewelryEquipments = new EquipmentType[]
    {
        EquipmentType.Ring,
        EquipmentType.Necklace,
    };
}

[Serializable]
public class BonusAttribute
{
    public AttributeType type;
    public float value;

    public BonusAttribute(AttributeType type, float value) 
    {
        this.type = type;
        this.value = value;
    }
}

[Serializable]
public class BaseAttributeByRarity
{
    public Rarity rarity;
    public BonusAttribute[] attributes;
}
