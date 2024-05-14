using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newInventoryItemData", menuName = "Data/Inventory/Item Data")]
public class InventoryItemSO : ScriptableObject
{
    public Vector2Int size;
    public Sprite sprite;
}
