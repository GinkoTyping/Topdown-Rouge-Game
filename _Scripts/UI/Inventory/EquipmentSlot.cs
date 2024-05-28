using Ginko.PlayerSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public EquipmentType type;
    [SerializeField]
    public int tileSize;
    [SerializeField]
    private GameObject equipmentPrefab;

    private bool isEmpty;
    private bool isActive;

    private InventoryController inventoryController;
    private Grid backpackInventory;
    private Image backgroundImage;
    private Color defaultBackgroundColor;

    public InventoryItem currentEquipment { get; private set; }

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
        backgroundImage = GetComponent<Image>();
        backpackInventory = inventoryController.transform.Find("Backpack").GetComponentInChildren<Grid>();
        defaultBackgroundColor = backgroundImage.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isActive = true;
        SwitchHighlight(true);
        inventoryController.SetSelectedEquipmentSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentEquipment == null && isActive)
        {
            SwitchHighlight(false);
        }

        isActive = false;
        inventoryController.SetSelectedEquipmentSlot(null);
    }
    public void SetEquipment(InventoryItem item)
    {
        currentEquipment = item;
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

    public InventoryItem EquipItem(InventoryItem item)
    {
        EquipmentItemSO data = (EquipmentItemSO)item.data;
        if (data.equipmentType != type)
        {
            return null;
        }

        GameObject itemGO = Instantiate(equipmentPrefab);
        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();
        rectTransform.SetParent(GetComponent<RectTransform>());
        InventoryItem newItem = itemGO.GetComponent<InventoryItem>();

        newItem.Set(item.data, item.rarity, tileSize);

        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, -rectTransform.sizeDelta.y / 2);

        if (currentEquipment == null)
        {
            SetEquipment(newItem);
        } else
        {

        }

        return newItem;
    }

    public InventoryItem UnequipItem()
    {
        // TODO: 还有默认是仓库的情况，待补充
        InventoryItem unequipItem =  inventoryController.CreateItemOnMouse(currentEquipment.data, currentEquipment.rarity, backpackInventory);

        Destroy(currentEquipment.gameObject);
        currentEquipment = null;

        return unequipItem;
    }
}
