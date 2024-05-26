using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    [Header("Base")]
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
    [SerializeField]
    public AudioClip selectAudio;

    public event Action onInventoryChange;

    public Grid selectedInventory {  get; private set; }
    public InventoryItem selectedItem { get; private set; }
    public float scaleParam { get; private set; }

    private PlayerInputEventHandler playerInputEventHandler;
    private RectTransform selectedItemTransform;
    private RectTransform canvasTransform;
    private UIManager UIManager;
    private Grid lootInventory;

    private void Awake()
    {
        UIManager = GetComponentInParent<UIManager>();
        lootInventory = transform.Find("Loot").GetComponentInChildren<Grid>();
    }

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

    private void OnEnable()
    {
        UIManager.onInventoryUIClose += HandleInventoryClose;
    }

    private void OnDisable()
    {
        UIManager.onInventoryUIClose -= HandleInventoryClose;
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
                    SoundManager.Instance.PlaySound(selectAudio);
                    selectedItemTransform = selectedItem.GetComponent<RectTransform>();
                }
            }
            else
            {
                bool hasPlacedItem = selectedInventory.PlaceItem(selectedItem, inventoryPosition);
                if (hasPlacedItem)
                {
                    PlayeInteractItemAudio(selectedItem);
                    selectedItem = null;
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

    private void PlayeInteractItemAudio(InventoryItem item)
    {
        ItemType itemType = item.data.itemType;
        if (itemType == ItemType.Treasure)
        {
            SoundManager.Instance.PlaySound(treasureAudio);
        } else if (itemType == ItemType.Consumable)
        {
            SoundManager.Instance.PlaySound(consumableAudio);
        } else if (itemType == ItemType.Equipment)
        {
            SoundManager.Instance.PlaySound(equipmentAudio);
        }
    }
    public InventoryItem CreateItemInInventory(InventoryItemSO itemData, Rarity rarity,  Grid inventory, Vector2Int? position = null)
    {
        GameObject itemGO = Instantiate(inventoryItemPrefab);

        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();
        rectTransform?.SetParent(inventory.GetComponent<RectTransform>());

        itemGO.GetComponent<InventoryItem>().Set(itemData, rarity, inventory.tileSize);

        InventoryItem newItem = itemGO.GetComponent<InventoryItem>();

        if (position == null)
        {
            position = lootInventory.GetSpaceForItem(newItem);
            
        }

        if (position != null)
        {
            lootInventory.PlaceItem(newItem, position.Value);
        }

        return newItem;
    }

    public void HandleInventoryClose()
    {
        InventoryItem[] items = lootInventory.GetComponentsInChildren<InventoryItem>();

        foreach (InventoryItem item in items)
        {
            lootInventory.RemoveItem(item);
            Destroy(item.gameObject);
        }
    }
}
