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
    Helemet,
    Hand,
    Leg,
    Feet,
    Nacklace,
    Ring,
}
