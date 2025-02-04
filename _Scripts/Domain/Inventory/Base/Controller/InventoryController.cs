using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System;
using TMPro;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] public GameObject inventoryItemPrefab;
    [SerializeField] public GameObject equipmentItemPrefab;
    [SerializeField] private InventoryItemHoverController hoverController;
    [SerializeField] private UIManager UIManager;

    [Header("Equipments")]
    [SerializeField]
    private GameObject equipmentPage;
    [SerializeField]
    private GameObject equipmentDetailPage;

    [Header("Grid")]
    [SerializeField] private Grid lootInventory;
    [SerializeField] public Grid backpackInventory;
    [SerializeField] private EquipmentInventory equipmentInventory;

    [Header("Test")]
    [SerializeField]
    public InventoryItemSO[] inventoryItemsData;

    public event Action onInventoryChange;

    public Grid selectedInventory { get; private set; }
    public InventoryItem selectedItem { get; private set; }
    public int lootID { get; private set; }
    public float scaleParam { get; private set; }

    private InventoryItemIndicatorController indicatorController;
    private InventoryInteraction interactionController;
    private PlayerInputEventHandler playerInputEventHandler;
    private RectTransform selectedItemTransform;
    private RectTransform canvasTransform;
    private RectTransform pickupItemFrom;

    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        indicatorController = GetComponent<InventoryItemIndicatorController>();
        interactionController = GetComponent<InventoryInteraction>();

        canvasTransform = inventoryCanvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        scaleParam = canvasTransform.localScale.x;

        UpdateSelectedItem();

        HandleSelectItem();
        HandleSelectingItem();
        HandleRotateItem();
        HandleFastRemoveItem();

        HandleHoverItem();

        Test();
    }

    private void OnEnable()
    {
        UIManager.onInventoryUIClose += HandleInventoryClose;
        equipmentDetailPage.SetActive(false);
    }

    private void OnDisable()
    {
        UIManager.onInventoryUIClose -= HandleInventoryClose;
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

    #region Set
    public void SetSelectedInventory(Grid inventory)
    {
        selectedInventory = inventory;

        if (inventory == null)
        {
            indicatorController.HideIndicator();
        }
        else
        {
            onInventoryChange?.Invoke();
        }
    }

    public void SetSelectedItem(InventoryItem item)
    {
        selectedItem = item;
        if (selectedItem != null)
        {
            selectedItemTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    public void SetPickUpItemFrom(RectTransform transform)
    {
        pickupItemFrom = transform;
    }

    public void SetLootID(LootsSpawning lootsRespawning)
    {
        lootID = lootsRespawning.GetInstanceID();
    }
    #endregion

    #region Handle Inputs
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
                    selectedItem.GetComponent<RectTransform>().SetAsLastSibling();
                    SetPickUpItemFrom(selectedInventory.GetComponent<RectTransform>());
                }
            }
            else
            {
                bool hasPlacedItem = selectedInventory.PlaceItem(selectedItem, inventoryPosition);
                if (hasPlacedItem)
                {
                    selectedItem = null;
                    indicatorController.HideIndicator();
                }
                else
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

    private void HandleFastRemoveItem()
    {
        if (playerInputEventHandler.HoldCombineKey && playerInputEventHandler.Select)
        {
            playerInputEventHandler.useSelectSignal();

            Vector2Int inventoryPosition = GetInventoryPosition(null);
            selectedInventory.RemoveItem(inventoryPosition, true);
        }
    }

    private void HandleHoverItem()
    {
        if (selectedInventory != null || equipmentInventory.selectedEquipmentSlot != null)
        {
            InventoryItem item = null;
            if (selectedInventory != null)
            {
                Vector2Int pos = GetInventoryPosition(null);
                item = selectedInventory.GetItem(pos);
            }
            else
            {
                item = equipmentInventory.selectedEquipmentSlot.currentEquipment;
            }

            if (item == null)
            {
                hoverController.Hide();
            }
            else if (item != hoverController.currentItem && !item.GetComponent<ItemToSearch>().needSearch)
            {
                RectTransform rect = item.GetComponent<RectTransform>();
                hoverController.Set(item, rect);
            }
        }
        else if (selectedInventory == null && equipmentInventory.selectedEquipmentSlot == null)
        {
            hoverController.Hide();
        }
    }

    private void HandleSelectingItem()
    {
        if (selectedInventory != null && selectedItem != null)
        {
            indicatorController.ShowIndicator(GetInventoryPosition(selectedItem), selectedItem);
        }
        else
        {
            indicatorController.HideIndicator();
        }
    }

    public void HandleInventoryClose()
    {
        RestorePickUpItem();
    }
    #endregion

    public void ResetLootBox()
    {
        InventoryItem[] items = lootInventory.GetComponentsInChildren<InventoryItem>();

        foreach (InventoryItem item in items)
        {
            lootInventory.RemoveItem(item);
            Destroy(item.gameObject);
        }
    }

    private void RestorePickUpItem()
    {
        if (selectedItem != null && pickupItemFrom != null)
        {

            Grid grid = pickupItemFrom.GetComponent<Grid>();
            EquipmentSlot slot = pickupItemFrom.GetComponent<EquipmentSlot>();
            if (grid != null)
            {
                grid.PlaceItem(selectedItem, selectedItem.pivotPositionOnGrid);
            }
            else if (slot != null)
            {
                equipmentInventory.SetSelectedEquipmentSlot(slot);
                interactionController.HandleEquipItem(true);
            }

            SetSelectedItem(null);
        }
    }

    public Vector2Int GetInventoryPosition(InventoryItem item)
    {
        Vector2 mousePosition = playerInputEventHandler.MousePosition;

        Vector2 relativePostion = item == null
            ? mousePosition
            : new Vector2(
                mousePosition.x - (float)(item.width - 1) * (selectedInventory.tileSize * scaleParam) / 2,
                mousePosition.y + (float)(item.height - 1) * (selectedInventory.tileSize * scaleParam) / 2);

        return selectedInventory.GetGridRelativePosition(relativePostion);
    }

    public void Test()
    {
        if (playerInputEventHandler.Test)
        {
            playerInputEventHandler.useTestSignal();
            selectedItem = null;
            selectedItemTransform = null;

            int index = UnityEngine.Random.Range(0, inventoryItemsData.Length);
            InventoryItemSO itemData = inventoryItemsData[index];

            InventoryItem item = CreateItemOnMouse(itemData);

            selectedItem = null;

            selectedInventory.GetSpaceToPlaceItem(item);
        }
    }

    #region Create
    public InventoryItem CreateItemOnMouse(
        InventoryItemSO itemData,
        Rarity? rarity = null,
        Grid inventory = null)
    {
        if (inventory == null)
        {
            inventory = selectedInventory;
        }

        if (inventory == null)
        {
            return null;
        }

        GameObject itemGO = Instantiate(
            itemData.itemType == ItemType.Equipment
            ? equipmentItemPrefab
            : inventoryItemPrefab
            );

        SetSelectedItem(itemGO.GetComponent<InventoryItem>());

        Rarity setRarity = rarity == null
            ? itemData.defaultRarity
            : (Rarity)rarity;

        InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();
        inventoryItem.Set(itemData, inventory.GetComponent<RectTransform>(), setRarity, inventory.tileSize);
        if (itemData.itemType == ItemType.Equipment)
        {
            EquipmentItem equipmentItem = inventoryItem as EquipmentItem;
            equipmentItem.SetBonusAttribute();
        }

        return selectedItem;
    }

    public InventoryItem CreateItemInInventory(
        InventoryItemSO itemData,
        Rarity rarity,
        Grid inventory,
        bool isVisible,
        BonusAttribute[] bonusAttributes = null,
        Vector2Int? position = null
        )
    {
        GameObject itemGO = Instantiate(
            itemData.itemType == ItemType.Equipment
            ? equipmentItemPrefab
            : inventoryItemPrefab
        );


        InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();

        inventoryItem.Set(
            itemData,
            inventory.GetComponent<RectTransform>(),
            rarity,
            inventory.tileSize
        );

        inventoryItem.SwitchItemVisible(isVisible);

        EquipmentItem equipment = itemGO.GetComponent<EquipmentItem>();
        if (equipment != null)
        {
            equipment.SetBonusAttribute(bonusAttributes);
        }

        if (position == null)
        {
            lootInventory.GetSpaceToPlaceItem(inventoryItem, playAudio: false);
        }
        else
        {
            lootInventory.PlaceItem(inventoryItem, position, false);
        }

        return inventoryItem;
    }

    #endregion
}
