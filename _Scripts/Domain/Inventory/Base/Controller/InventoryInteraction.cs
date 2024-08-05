using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    [Header("Other Inventories")]
    [SerializeField]
    private Grid backpackInventory;
    [SerializeField]
    private Grid pocketInventory;
    [SerializeField]
    private Grid lootInventory;
    [SerializeField]
    private EquipmentInventory equipmentInventory;

    private InventoryController inventoryController;
    private InventorySound soundController;
    private PlayerInputEventHandler playerInputEventHandler;


    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        inventoryController = GetComponent<InventoryController>();
        soundController = GetComponent<InventorySound>();
    }

    private void Update()
    {
        HandleEquipItem();
        HandleUnequipItem();

        HandleFastEquipItem();
        HandleFastUnequipItem();
    }

    //public void SetSelectedEquipmentSlot(EquipmentSlot equipmentSlot)
    //{
    //    selectedEquipmentSlot = equipmentSlot;
    //}

    #region Handlers

    public void HandleEquipItem(bool isManual = false)
    {
        if (equipmentInventory.selectedEquipmentSlot == null || inventoryController.selectedItem == null)
        {
            return;
        }

        if (playerInputEventHandler.Select || isManual)
        {
            playerInputEventHandler.useSelectSignal();

            InventoryItem[] equipItems = equipmentInventory.selectedEquipmentSlot.EquipItem(inventoryController.selectedItem as EquipmentItem);

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
                equipmentInventory.SetSelectedEquipmentSlot(null);
            }
        }
    }

    private void HandleUnequipItem()
    {
        if (equipmentInventory.selectedEquipmentSlot?.currentEquipment == null)
        {
            return;
        }

        if (playerInputEventHandler.Select)
        {
            playerInputEventHandler.useSelectSignal();

            inventoryController.SetPickUpItemFrom(equipmentInventory.selectedEquipmentSlot.GetComponent<RectTransform>());
            equipmentInventory.selectedEquipmentSlot.UnequipItem(backpackInventory);
        }
    }
    
    private void HandleFastEquipItem()
    {
        if (playerInputEventHandler.DeSelect
            && inventoryController.selectedItem == null
            && inventoryController.selectedInventory != null
            && inventoryController.selectedInventory != pocketInventory)
        {
            playerInputEventHandler.useDeSelectSignal();
            Vector2Int inventoryPosition = inventoryController.GetInventoryPosition(null);

            InventoryItem itemToEquip = inventoryController.selectedInventory.GetItem(inventoryPosition);

            if (itemToEquip.data.itemType == ItemType.Equipment)
            {
                FastEquipEquipment(itemToEquip as EquipmentItem);
            }
            else if (itemToEquip.data.itemType == ItemType.Consumable)
            {
                FastEquipConsumable(itemToEquip);
            } else if (itemToEquip.data.itemType == ItemType.Treasure)
            {
                FastEquipTreasure(itemToEquip);
            }
        }
    }

    private void HandleFastUnequipItem()
    {
        if (playerInputEventHandler.DeSelect
            && inventoryController.selectedItem == null)
        {
            if (equipmentInventory.selectedEquipmentSlot != null)
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

    #endregion
    private void FastUnequipEquipment()
    {
        Vector2Int? position = backpackInventory.GetSpaceToPlaceItem(equipmentInventory.selectedEquipmentSlot.currentEquipment);
        equipmentInventory.selectedEquipmentSlot.UnequipItem(backpackInventory, (Vector2Int)position);
    }

    private void FastEquipEquipment(EquipmentItem item)
    {
        EquipmentType equipmentType = ((EquipmentItemSO)(item.data)).equipmentType;
        bool hasEquipmentSlot = equipmentInventory.equipmentSlots
            .Where(slot => 
                slot.type == equipmentType 
                && slot.currentEquipment == null
                )
            .ToArray().Length > 0;

        if (!hasEquipmentSlot && inventoryController.selectedInventory == lootInventory)
        {
            Vector2Int? pos = backpackInventory.GetSpaceToPlaceItem(item);
            if (pos != null)
            {
                inventoryController.selectedInventory.RemoveItem(item);
            }
        } else
        {
            inventoryController.SetSelectedItem(item);
            inventoryController.selectedInventory.RemoveItem(item);

            int ringSlotIndex = 0;
            foreach (EquipmentSlot slot in equipmentInventory.equipmentSlots)
            {
                if (slot.type == equipmentType)
                {
                    equipmentInventory.SetSelectedEquipmentSlot(slot);

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

    private void FastEquipConsumable(InventoryItem item)
    {
        Vector2Int? pocketPos = pocketInventory.GetSpaceToPlaceItem(item, false);

        if(pocketPos == null && inventoryController.selectedInventory == lootInventory)
        {
            Vector2Int? backpackPos = backpackInventory.GetSpaceToPlaceItem(item);
            if (backpackPos != null)
            {
                inventoryController.selectedInventory.RemoveItem(item);
            }
        } else
        {
            if (pocketPos != null)
            {
                inventoryController.selectedInventory.RemoveItem(item);
            }
            pocketInventory.PlaceItem(item, pocketPos);
        }
    }
    
    private void FastEquipTreasure(InventoryItem item)
    {
        if (inventoryController.selectedInventory == lootInventory)
        {
            Vector2Int? pos = backpackInventory.GetSpaceToPlaceItem(item);
            if (pos != null)
            {
                inventoryController.selectedInventory.RemoveItem(item);
            }
        }
    }
}
