using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private Grid backpackInventory;
    private PlayerStats stats;

    private List<AttributeType> plusAttributes = new List<AttributeType> { AttributeType.Intelligence, AttributeType.Strength, AttributeType.Agility};
    private List<AttributeType> multiAttributes = new List<AttributeType> { AttributeType.CriticalChance, AttributeType.CriticalDamage };

    public InventoryItem currentEquipment { get; private set; }

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
        backgroundImage = GetComponent<Image>();
        backpackInventory = inventoryController.transform.Find("Luggage").Find("Backpack").GetComponent<Grid>();
        defaultBackgroundColor = backgroundImage.color;
    }

    private void Start()
    {
        stats = Player.Instance.Core.GetCoreComponent <PlayerStats>();
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
    public void UpdateEquipmentStat(InventoryItem item)
    {
        if (item == null)
        {
            UpdatePlayerAttribute(false);
            currentEquipment = null;
        }
        else
        {
            currentEquipment = item;
            UpdatePlayerAttribute(true);
        }
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
            unequippedItem = UnequipItem();
        }

        item.SetSize(GetComponent<RectTransform>().sizeDelta);
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(GetComponent<RectTransform>());
        rectTransform.SetAsFirstSibling();
        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, -rectTransform.sizeDelta.y / 2);

        UpdateEquipmentStat(item);

        InventoryItem[] output = { item, unequippedItem };
        return output;
    }

    public EquipmentItem UnequipItem()
    {
        currentEquipment.SetSize(backpackInventory.tileSize);

        // TODO: 还有默认是仓库的情况，待补充
        currentEquipment.GetComponent<RectTransform>().SetParent(backpackInventory.GetComponent<RectTransform>());
        inventoryController.SetSelectedItem(currentEquipment);

        EquipmentItem output = currentEquipment as EquipmentItem;
        UpdateEquipmentStat(null);

        return output;
    }

    private void UpdatePlayerAttribute(bool isEquip)
    {
        EquipmentItem equipmentItem = currentEquipment as EquipmentItem;
        BonusAttribute[] attributes = equipmentItem.bonusAttributes.Concat(equipmentItem.baseAttributes).ToArray();

        foreach (BonusAttribute attribute in attributes)
        {
            if(attribute.value != 0)
            {
                AttributeStat playerAttribute = stats.GetAttribute(attribute.type);
                if (isEquip)
                {
                    playerAttribute.Increase(attribute.value);
                    if (attribute.type == AttributeType.MaxHealth)
                    {
                        stats.GetAttribute(ResourceType.Health).ChangeMaxValue(attribute.value);
                    }
                } else
                {
                    playerAttribute.Decrease(attribute.value);
                    if (attribute.type == AttributeType.MaxHealth)
                    {
                        stats.GetAttribute(ResourceType.Health).ChangeMaxValue(-attribute.value);
                    }
                }
            }
        }
    }
}
