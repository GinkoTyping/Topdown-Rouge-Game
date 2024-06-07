using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroppedItemController : MonoBehaviour
{
    [SerializeField]
    public LayerMask dropsLayer;
    [SerializeField]
    public Material[] laserMaterials;

    private DroppedItemPool poolManager;

    private void Start()
    {
        poolManager = GetComponent<DroppedItemPool>();
    }
    public void CreateDroppedItem(InventoryItem item)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            DroppedEquipment droppedItem =  poolManager.Pool.Get().GetComponent<DroppedEquipment>();
            droppedItem.Set(item);
        }
    }
}
