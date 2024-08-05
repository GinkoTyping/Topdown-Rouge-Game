using System;
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

    [Header("Buff")]
    public Buff buff;
    public BuffDataByRarity[] buffDataByRarities;

    [Tooltip("It holds all the possible base attributes among different rarity.")]
    public BaseAttributeByRarity[] baseAttributeByRarities;
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

[Serializable]
public class BaseAttributeByRarity
{
    public Rarity rarity;
    public BonusAttribute[] attributes;
}

[Serializable]
public class BuffDataByRarity
{
    public Rarity rarity;
    public BaseBuffDataSO buffData;
}