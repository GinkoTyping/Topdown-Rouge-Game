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
