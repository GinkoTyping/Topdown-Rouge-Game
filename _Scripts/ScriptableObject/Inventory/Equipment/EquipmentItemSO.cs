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
