using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentsPageController : MonoBehaviour
{
    [SerializeField]
    private Grid backpackInventory;
    [SerializeField]
    private Grid pocketInventory;

    private InventoryController inventoryController;
    private InventorySoundController soundController;
    private PlayerInputEventHandler playerInputEventHandler;

    public EquipmentSlot selectedEquipmentSlot;
    private EquipmentSlot[] equipmentSlots;

    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        inventoryController = GetComponent<InventoryController>();
        soundController = GetComponent<InventorySoundController>();

        equipmentSlots = transform.Find("Equipment").Find("SlotsPage").GetComponentsInChildren<EquipmentSlot>();
    }

    private void Update()
    {
        HandleEquipItem();
        HandleUnequipItem();

        HandleFastEquipItem();
        HandleFastUnequipItem();
    }

    public void SetSelectedEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        selectedEquipmentSlot = equipmentSlot;
    }
    
    #region Handlers

    private void HandleFastEquipItem()
    {
        if (playerInputEventHandler.DeSelect
            && inventoryController.selectedItem == null
            && inventoryController.selectedInventory != null
            && inventoryController.selectedInventory != pocketInventory)
        {
            playerInputEventHandler.useDeSelectSignal();
            Vector2Int inventoryPosition = inventoryController.GetInventoryPosition(null);

            InventoryItem itemToEquip = inventoryController.selectedInventory.PickUpItem(inventoryPosition, false);

            if (itemToEquip.data.itemType == ItemType.Equipment)
            {
                FastEquipEquipment(itemToEquip as EquipmentItem);
            }
            else if (itemToEquip.data.itemType == ItemType.Consumable)
            {
                FastEquipConsumable(itemToEquip);
            }
        }
    }

    private void HandleFastUnequipItem()
    {
        if (playerInputEventHandler.DeSelect
            && inventoryController.selectedItem == null)
        {
            if (selectedEquipmentSlot != null)
            {
                playerInputEventHandler.useDeSelectSignal();

                FastUnequipEquipment();
            }
            else if (inventoryController.selectedInventory == pocketInventory)
            {
                playerInputEventHandler.useDeSelectSignal();

            }
        }
    }

    public void HandleEquipItem(bool isManual = false)
    {
        if (selectedEquipmentSlot == null || inventoryController.selectedItem == null)
        {
            return;
        }

        if (playerInputEventHandler.Select || isManual)
        {
            playerInputEventHandler.useSelectSignal();

            InventoryItem[] equipItems = selectedEquipmentSlot.EquipItem(inventoryController.selectedItem as EquipmentItem);

            bool successEquipped = equipItems != null;
            if (successEquipped)
            {
                bool isEmptySlotBeforeEquip = equipItems[1] == null;
                if (isEmptySlotBeforeEquip)
                {
                    inventoryController.SetSelectedItem(null);
                }

                soundController.PlayEquipItemAudio();
            }
            else
            {
                SoundManager.Instance.Warning();
            }

            if (isManual)
            {
                SetSelectedEquipmentSlot(null);
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

            inventoryController.SetPickUpItemFrom(selectedEquipmentSlot.GetComponent<RectTransform>());
            selectedEquipmentSlot.UnequipItem(backpackInventory);
        }
    }
    
    #endregion

    private void FastUnequipEquipment()
    {
        Vector2Int? position = backpackInventory.GetSpaceForItem(selectedEquipmentSlot.currentEquipment);
        selectedEquipmentSlot.UnequipItem(backpackInventory, (Vector2Int)position);
    }

    private void FastEquipEquipment(EquipmentItem item)
    {
        inventoryController.SetSelectedItem(item);
        EquipmentType equipmentType = ((EquipmentItemSO)(item.data)).equipmentType;
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

    private void FastEquipConsumable(InventoryItem item)
    {
        Vector2Int pos = (Vector2Int)pocketInventory.GetSpaceForItem(item);

        if (pos != null)
        {
            pocketInventory.PlaceItem(item, pos);
        }
    }
}
