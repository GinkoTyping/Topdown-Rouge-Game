using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventoryItemData", menuName = "Data/Inventory/Item Data")]
public class InventoryItemSO : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int size;
    public Rarity defaultRarity;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legend,
}
