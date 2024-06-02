using Ginko.PlayerSystem;
using System;
using TMPro;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    [Header("Base")]
    [SerializeField]
    public GameObject inventoryItemPrefab;
    [SerializeField]
    public GameObject equipmentItemPrefab;
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
    [SerializeField]
    public AudioClip equipAudio;

    public event Action onInventoryChange;

    private EquipmentSlot selectedEquipmentSlot;
    private EquipmentSlot[] equipmentSlots;
    public Grid selectedInventory {  get; private set; }
    public InventoryItem selectedItem { get; private set; }
    public float scaleParam { get; private set; }

    private PlayerInputEventHandler playerInputEventHandler;
    private RectTransform selectedItemTransform;
    private RectTransform canvasTransform;
    private UIManager UIManager;
    private Grid lootInventory;

    private GameObject equipmentPage;
    private GameObject equipmentDetailPage;
    private ItemHoverController hoverController;
    private TextMeshProUGUI switchEquipemntPageButton;
    private bool isShowEquipmentPage;
    private float scaleUnit;
    private RectTransform pickupItemFrom;

    private void Awake()
    {
        UIManager = GetComponentInParent<UIManager>();
        lootInventory = transform.Find("Loot").GetComponentInChildren<Grid>();
        equipmentPage = transform.Find("Equipment").Find("SlotsPage").gameObject;
        equipmentDetailPage = transform.Find("Equipment").Find("Details").gameObject;
        switchEquipemntPageButton = transform.Find("Equipment").Find("SwitchDetailButton").GetComponentInChildren<TextMeshProUGUI>();
        hoverController = GameObject.Find("Hover").GetComponent<ItemHoverController>();

        scaleUnit = Camera.main.orthographicSize / Screen.height * 2;
        isShowEquipmentPage = true;
    }

    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        canvasTransform = GetComponent<RectTransform>();
        equipmentSlots = transform.Find("Equipment").Find("SlotsPage").GetComponentsInChildren<EquipmentSlot>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        scaleParam = canvasTransform.localScale.x;

        UpdateSelectedItem();

        HandleSelectItem();
        HandleRotateItem();
        HandleFastRemoveItem();

        HandleEquipItem();
        HandleUnequipItem();

        HandleFastEquipItem();

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


    public void SetSelectedInventory(Grid inventory)
    {
        selectedInventory = inventory;

        if (inventory != null)
        {
            onInventoryChange?.Invoke();
        }
    }

    public void SetSelectedEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        selectedEquipmentSlot = equipmentSlot;
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
                SetPickUpItemFrom(selectedInventory.GetComponent<RectTransform>());
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

    private void HandleFastRemoveItem()
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

    private void HandleFastEquipItem()
    {
        if (playerInputEventHandler.HoldCombineKey && playerInputEventHandler.DeSelect)
        {
            playerInputEventHandler.useDeSelectSignal();
            Vector2Int inventoryPosition = GetInventoryPosition(null);

            InventoryItem itemToEquip = selectedInventory.PickUpItem(inventoryPosition);

            if (itemToEquip.data.itemType != ItemType.Equipment)
            {
                return;
            }

            EquipmentType equipmentType = ((EquipmentItemSO)(itemToEquip.data)).equipmentType;

            SetSelectedItem(itemToEquip);

            int ringSlotIndex = 0;

            foreach (EquipmentSlot slot in equipmentSlots)
            {
                if (slot.type == equipmentType)
                {
                    SetSelectedEquipmentSlot(slot);

                    if (slot.type == EquipmentType.Ring)
                    {
                        if (slot.currentEquipment == null)
                        {
                            HandleEquipItem(true);
                            break;
                        } 
                        else if (slot.currentEquipment != null && ringSlotIndex == 1)
                        {
                            HandleEquipItem(true);
                            break;
                        }
                        ringSlotIndex++;
                    } 
                    else
                    {
                        HandleEquipItem(true);
                        break;
                    }
                }
            }
        }
    }

    private void HandleEquipItem(bool isManual = false)
    {
        if (selectedEquipmentSlot == null || selectedItem == null)
        {
            return;
        }

        if (playerInputEventHandler.Select || isManual)
        {
            playerInputEventHandler.useSelectSignal();

            InventoryItem[] equipItems =  selectedEquipmentSlot.EquipItem(selectedItem as EquipmentItem);

            bool successEquipped = equipItems != null;
            if (successEquipped)
            {
                bool isEmptySlotBeforeEquip = equipItems[1] == null;
                if (isEmptySlotBeforeEquip)
                {
                    SetSelectedItem(null);
                }

                PlayEquipItemAudio();
            } else
            {
                SoundManager.Instance.Warning();
            }
        }
    }

    private void HandleUnequipItem()
    {
        if (selectedEquipmentSlot?.currentEquipment == null)
        {
            return;
        }

        if (playerInputEventHandler.Select)
        {
            playerInputEventHandler.useSelectSignal();

            SetPickUpItemFrom(selectedEquipmentSlot.GetComponent<RectTransform>());
            selectedEquipmentSlot.UnequipItem();
        }
    }
    
    public void HandleInventoryClose()
    {
        RestoreLootBox();
        RestorePickUpItem();
    }

    private void RestoreLootBox()
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
                SetSelectedEquipmentSlot(slot);
                HandleEquipItem(true);
            }

            SetSelectedItem(null);
        }
    }

    private void HandleHoverItem()
    {
        if (selectedInventory != null || selectedEquipmentSlot != null)
        {
            InventoryItem item = null;
            if (selectedInventory != null)
            {
                Vector2Int pos = GetInventoryPosition(null);
                item = selectedInventory.GetItem(pos);
            }
            else
            {
                item = selectedEquipmentSlot.currentEquipment;
            }

            if (item == null)
            {
                hoverController.Hide();
            }
            else if (item != hoverController.currentItem)
            {
                RectTransform rect = item.GetComponent<RectTransform>();
                Vector2 position = CalculateHoverPos(rect);
                hoverController.Set(item, position);
            }
        }
        else if (selectedInventory == null && selectedEquipmentSlot == null)
        { 
            hoverController.Hide();
        }
    }
    private Vector2 CalculateHoverPos(RectTransform relative)
    {
        Vector3 hoverPosition = Camera.main.WorldToScreenPoint(relative.position);

        RectTransform hoverRect = hoverController.GetComponent<RectTransform>();
        float x;
        float y;
        x = hoverPosition.x + hoverRect.sizeDelta.x > Screen.width
                ? relative.position.x - relative.sizeDelta.x * scaleUnit / 2 - hoverRect.sizeDelta.x * scaleUnit
                : relative.position.x + relative.sizeDelta.x * scaleUnit / 2;
        y = hoverPosition.y - hoverRect.sizeDelta.y < 0
                ? relative.position.y - relative.sizeDelta.y * scaleUnit / 2 + hoverRect.sizeDelta.y * scaleUnit
                : relative.position.y + relative.sizeDelta.y * scaleUnit / 2;

        return new Vector2(x, y);
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
    public void Test()
    {
        if (playerInputEventHandler.Test)
        {
            playerInputEventHandler.useTestSignal();
            selectedItem = null;
            selectedItemTransform = null;

            int index = UnityEngine.Random.Range(0, 2);
            InventoryItemSO itemData = inventoryItemsData[index];

            InventoryItem item =  CreateItemOnMouse(itemData);

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

    public void PlayEquipItemAudio()
    {
        SoundManager.Instance.PlaySound(equipAudio);
    }

    public InventoryItem CreateItemInInventory(InventoryItemSO itemData, Rarity rarity,  Grid inventory, BonusAttribute[] bonusAttributes = null, Vector2Int? position = null)
    {
        GameObject itemGO = Instantiate(
            itemData.itemType == ItemType.Equipment
            ? equipmentItemPrefab
            : inventoryItemPrefab
            );

        itemGO.GetComponent<InventoryItem>().Set(itemData, inventory.GetComponent<RectTransform>(), rarity, inventory.tileSize);

        EquipmentItem equipment = itemGO.GetComponent<EquipmentItem>();
        if (equipment != null)
        {
            equipment.SetBonusAttribute(bonusAttributes);
        }

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

    public void OnSwitchEquipmentPage()
    {
        isShowEquipmentPage = !isShowEquipmentPage;

        equipmentPage.SetActive(isShowEquipmentPage);
        equipmentDetailPage.SetActive(!isShowEquipmentPage);

        switchEquipemntPageButton.text = isShowEquipmentPage ? "Detailed Stauts" : "Equipment Status";

        SoundManager.Instance.ButtonClick();
    }
}
