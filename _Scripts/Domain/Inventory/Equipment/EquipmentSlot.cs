using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public EquipmentType type;
    [SerializeField]
    private GameObject equipmentPrefab;

    private bool isActive;

    private Image backgroundImage;
    private Color defaultBackgroundColor;

    private InventoryController inventoryController;
    private EquipmentInventory equipmentsPageController;

    public InventoryItem currentEquipment { get; private set; }

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
        equipmentsPageController = GetComponentInParent<EquipmentInventory>();

        backgroundImage = GetComponent<Image>();
        defaultBackgroundColor = backgroundImage.color;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isActive = true;
        SwitchHighlight(true);
        equipmentsPageController.SetSelectedEquipmentSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentEquipment == null && isActive)
        {
            SwitchHighlight(false);
        }

        isActive = false;
        equipmentsPageController.SetSelectedEquipmentSlot(null);
    }
   
    private void SwitchHighlight(bool isHighlight)
    {
        if (isHighlight)
        {
            backgroundImage.color = new Color(255,255,255, 0);
        } else
        {
            backgroundImage.color = defaultBackgroundColor;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns>[equippedItem, unequippedItem]</returns>
    public InventoryItem[] EquipItem(EquipmentItem item)
    {
        EquipmentItemSO data = (EquipmentItemSO)item.data;
        InventoryItem unequippedItem = null;
        if (data.equipmentType != type)
        {
            return null;
        }

        if (currentEquipment != null)
        {
            unequippedItem = UnequipItem(item.GetComponentInParent<Grid>(), item.pivotPositionOnGrid);
        }

        item.SetSize(GetComponent<RectTransform>().sizeDelta);
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(GetComponent<RectTransform>());
        rectTransform.SetAsFirstSibling();
        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, -rectTransform.sizeDelta.y / 2);

        currentEquipment = item;
        item.Ability.Equip(true);

        InventoryItem[] output = { item, unequippedItem };
        return output;
    }

    public EquipmentItem UnequipItem(Grid inventory, Vector2Int? postion = null)
    {
        currentEquipment.SetSize(inventory.tileSize);

        if (postion == null)
        {
            currentEquipment.GetComponent<RectTransform>().SetParent(inventory.transform);
            inventoryController.SetSelectedItem(currentEquipment);
        } else
        {
            inventory.PlaceItem(currentEquipment, (Vector2Int)postion);
            inventoryController.SetSelectedItem(null);
        }

        EquipmentItem output = currentEquipment as EquipmentItem;

        currentEquipment.Ability.Equip(false);
        currentEquipment = null;

        return output;
    }
}
