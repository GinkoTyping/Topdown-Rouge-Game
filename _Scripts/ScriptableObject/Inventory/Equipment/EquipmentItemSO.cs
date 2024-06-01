using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentItemSO : InventoryItemSO
{
    public EquipmentType equipmentType;
    public BonusAttribute[] bonusAttributes;
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
}
