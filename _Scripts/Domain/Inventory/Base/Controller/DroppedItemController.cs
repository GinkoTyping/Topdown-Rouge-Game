using Ginko.PlayerSystem;
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

    private CommonPool poolManager;

    private void Start()
    {
        poolManager = GetComponent<CommonPool>();
    }
    public void CreateDroppedItem(InventoryItem item, Vector3? position = null)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            DroppedEquipment droppedItem =  poolManager.Pool.Get().GetComponent<DroppedEquipment>();

            Vector3 dropppedPos = position == null
                ? Player.Instance.transform.position
                : (Vector3)position;

            droppedItem.Set(item, dropppedPos);
        }
    }
}
