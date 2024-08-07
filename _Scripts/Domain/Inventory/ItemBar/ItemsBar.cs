using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsBar : MonoBehaviour
{
    [SerializeField] private Grid pockectInventory;
    private ItemOnBar[] items;

    private void Awake()
    {
        items = GetComponentsInChildren<ItemOnBar>();
    }

    private void OnEnable()
    {
        pockectInventory.OnItemChange += UpdateItemsOnBar;
    }

    private void OnDisable()
    {
        pockectInventory.OnItemChange -= UpdateItemsOnBar;
    }

    private void Start()
    {

    }

    private void UpdateItemsOnBar(bool isAdd, InventoryItem item)
    {
        if (isAdd)
        {
            items[0].Set(item);
        } else
        {
            InventoryItem[] leftItems = pockectInventory.GetComponentsInChildren<InventoryItem>().Where(i => i != item).ToArray();

            if (leftItems.Length == 0)
            {
                items[0].Clear();
            } else
            {
                items[0].Set(leftItems[0]);
            }
        }
    }
}
