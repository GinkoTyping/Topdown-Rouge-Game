using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    public GameObject inventoryItemPrefab;
    [SerializeField]
    public InventoryItemSO[] inventoryItemsData;

    [Header("Audios")]
    [SerializeField]
    public AudioClip equipmentAudio;
    [SerializeField]
    public AudioClip treasureAudio;
    [SerializeField]
    public AudioClip consumableAudio;
    [SerializeField]
    public AudioClip removeAudio;

    public event Action onInventoryChange;
    public Grid selectedInventory;
    public InventoryItem selectedItem;
    public float scaleParam;

    private PlayerInputEventHandler playerInputEventHandler;
    private RectTransform selectedItemTransform;
    private RectTransform canvasTransform;

    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        canvasTransform = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        scaleParam = canvasTransform.localScale.x;

        UpdateSelectedItem();
        HandleSelectItem();
        HandleRotateItem();
        HandleRemoveItem();

        Test();
    }
    public void SetSelectedInventory(Grid inventory)
    {
        selectedInventory = inventory;

        if (inventory != null)
        {
            onInventoryChange?.Invoke();
        }
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
        if (playerInputEventHandler.Select && !playerInputEventHandler.HoldCombineKey)
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
                    SoundManager.Instance.PlaySound(equipmentAudio);
                } else
                {
                    SoundManager.Instance.Warning();
                }
            }
        }
    }

    private void HandleRotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        if (playerInputEventHandler.RotateItem)
        {
            playerInputEventHandler.UseRotateItemSignal();

            selectedItem.Rotate();
        }
    }

    private void HandleRemoveItem()
    {
        if (playerInputEventHandler.HoldCombineKey && playerInputEventHandler.Select)
        {
            playerInputEventHandler.useSelectSignal();

            Vector2Int inventoryPosition = GetInventoryPosition(null);
            InventoryItem item = selectedInventory.RemoveItem(inventoryPosition);
            Destroy(item.gameObject);

            SoundManager.Instance.PlaySound(removeAudio);
        }
    }

    private Vector2Int GetInventoryPosition(InventoryItem item)
    {
        Vector2 mousePosition = playerInputEventHandler.MousePosition;

        Vector2 relativePostion = item == null
            ? mousePosition
            : new Vector2(
                mousePosition.x - (float)(item.width - 1) * (selectedInventory.tileSize * scaleParam) / 2,
                mousePosition.y + (float)(item.height - 1) * (selectedInventory.tileSize * scaleParam) / 2);

        return selectedInventory.GetGridRelativePosition(relativePostion);
    }

    private InventoryItem CreateItemOnMouse(InventoryItemSO[] items)
    {
        if (selectedInventory == null)
        {
            return null;
        }

        InventoryItemSO itemData = items[UnityEngine.Random.Range(0,3)];

        GameObject itemGO = Instantiate(inventoryItemPrefab);
        selectedItem = itemGO.GetComponent<InventoryItem>();

        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();
        rectTransform?.SetParent(selectedInventory.GetComponent<RectTransform>());

        itemGO.GetComponent<InventoryItem>().Set(itemData, Rarity.Common, selectedInventory.tileSize);

        return selectedItem;
    }
    public void Test()
    {
        if (playerInputEventHandler.Test)
        {
            playerInputEventHandler.useTestSignal();
            selectedItem = null;
            selectedItemTransform = null;

            InventoryItem item =  CreateItemOnMouse(inventoryItemsData);

            selectedItem = null;

            Vector2Int? pos = selectedInventory.GetSpaceForItem(item);
            if (pos != null)
            {
                selectedInventory.PlaceItem(item, pos.Value);
            }
        }
    }

    public InventoryItem CreateItemInInventory(InventoryItemSO itemData, Rarity rarity,  Grid inventory)
    {
        GameObject itemGO = Instantiate(inventoryItemPrefab);

        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();
        rectTransform?.SetParent(inventory.GetComponent<RectTransform>());

        itemGO.GetComponent<InventoryItem>().Set(itemData, rarity, inventory.tileSize);

        return itemGO.GetComponent<InventoryItem>();
    }
}
