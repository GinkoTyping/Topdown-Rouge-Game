using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    public GameObject inventoryItemPrefab;
    [SerializeField]
    public InventoryItemSO[] inventoryItemsData;

    private PlayerInputEventHandler playerInputEventHandler;
    private Grid selectedInventory;

    private InventoryItem selectedItem;
    private RectTransform selectedItemTransform;
    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;

    }

    private void Update()
    {
        UpdateSelectedItem();
        HandleSelectItem();

        Test();
    }
    public void SetSelectedInventory(Grid inventory)
    {
        selectedInventory = inventory;
    }

    private void UpdateSelectedItem()
    {
        if (selectedItem != null)
        {
            if (selectedItemTransform == null)
            {
                selectedItemTransform = selectedItem.GetComponent<RectTransform>(); 
            }
            selectedItemTransform.position = (Vector3)playerInputEventHandler.MousePosition;
        }
    }
    private void HandleSelectItem()
    {
        if (selectedInventory == null)
        {
            return;
        }
        if (playerInputEventHandler.Select)
        {
            playerInputEventHandler.useSelectSignal();

            Vector2Int inventoryPosition = GetInventoryPosition(selectedItem);
            if (selectedItem == null)
            {
                selectedItem = selectedInventory.PickUpItem(inventoryPosition);
                if (selectedItem != null)
                {
                    selectedItemTransform = selectedItem.GetComponent<RectTransform>();
                }
            }
            else
            {
                bool hasPlacedItem = selectedInventory.PlaceItem(selectedItem, inventoryPosition);
                if (hasPlacedItem)
                {
                    selectedItem = null;
                }
            }
        }
    }

    private Vector2Int GetInventoryPosition(InventoryItem item)
    {
        Vector2 mousePosition = playerInputEventHandler.MousePosition;

        Vector2 relativePostion = item == null 
            ? mousePosition 
            : new Vector2(
                mousePosition.x - (float)(item.data.size.x - 1) * selectedInventory.tileSize / 2, 
                mousePosition.y + (float)(item.data.size.y - 1) * selectedInventory.tileSize / 2);

        return selectedInventory.GetGridRelativePosition(relativePostion);
    }

    private void CreateItemOnMouse(InventoryItemSO[] items)
    {
        InventoryItemSO itemData = items[Random.Range(0,2)];

        GameObject itemGO = Instantiate(inventoryItemPrefab);
        selectedItem = itemGO.GetComponent<InventoryItem>();

        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();
        rectTransform?.SetParent(selectedInventory.GetComponent<RectTransform>());

        itemGO.GetComponent<InventoryItem>().Set(itemData, selectedInventory.tileSize);

    }
    public void Test()
    {
        if (playerInputEventHandler.Test)
        {
            playerInputEventHandler.useTestSignal();
            selectedItem = null;
            selectedItemTransform = null;

            CreateItemOnMouse(inventoryItemsData);
        }
    }
}
