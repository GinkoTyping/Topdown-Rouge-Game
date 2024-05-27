using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLootListData", menuName = "Data/Loot/Loot List Data")]
public class LootListDataSO : ScriptableObject
{
    public LootGroup[] lootGroups;
}

[Serializable]
public class LootGroup
{
    public LootType lootType;
    public LootDetail[] loots;
}

[Serializable]
public class LootDetail
{
    public InventoryItemSO itemData;
    public Rarity rarity;
    [Range(0,1)]public float possibility;
}

public enum LootType
{
    Treasure,
    Equipments,
    Consumables,
}
