using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventoryItemData", menuName = "Data/Inventory/Item Data")]
public class InventoryItemSO : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public ItemType itemType;
    public Vector2Int size;
    public Rarity defaultRarity;
    public LootPriceDataSO priceSetting;

    public BaseAttributeByRarity[] baseAttributes;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legend,
}
public enum ItemType
{
    Equipment,
    Consumable,
    Treasure,
}