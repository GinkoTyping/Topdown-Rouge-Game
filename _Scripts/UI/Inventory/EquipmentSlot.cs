using Ginko.CoreSystem;
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
    private GameObject equipmentPrefab;

    private bool isActive;

    private Image backgroundImage;
    private Color defaultBackgroundColor;

    private InventoryController inventoryController;
    private Grid backpackInventory;
    private PlayerStats stats;

    public InventoryItem currentEquipment { get; private set; }

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
        backgroundImage = GetComponent<Image>();
        backpackInventory = inventoryController.transform.Find("Backpack").GetComponentInChildren<Grid>();
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
    public void SetEquipment(InventoryItem item)
    {
        if (item == null)
        {
            UpdatePlayerAttribute(false);
            Destroy(currentEquipment.gameObject);
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
    public InventoryItem[] EquipItem(InventoryItem item)
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

        GameObject itemGO = Instantiate(equipmentPrefab);
        RectTransform rectTransform = itemGO.GetComponent<RectTransform>();

        rectTransform.SetParent(GetComponent<RectTransform>());
        rectTransform.SetAsFirstSibling();

        InventoryItem equippedItem = itemGO.GetComponent<InventoryItem>();
        equippedItem.Set(item.data, item.rarity, GetComponent<RectTransform>().sizeDelta);

        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, -rectTransform.sizeDelta.y / 2);

        SetEquipment(equippedItem);

        InventoryItem[] output = { equippedItem, unequippedItem };
        return output;
    }

    public InventoryItem UnequipItem()
    {
        // TODO: 还有默认是仓库的情况，待补充
        InventoryItem unequipItem =  inventoryController.CreateItemOnMouse(currentEquipment.data, currentEquipment.rarity, backpackInventory);

        SetEquipment(null);

        return unequipItem;
    }

    private void UpdatePlayerAttribute(bool isEquip)
    {
        EquipmentItemSO equipmentData = (EquipmentItemSO)currentEquipment.data;
        BonusAttribute[] bonusAttribute = equipmentData.bonusAttribute;

        foreach (BonusAttribute attribute in bonusAttribute)
        {
            if(attribute.value != 0)
            {
                AttributeStat playerAttribute = stats.GetAttribute(attribute.type);
                if (isEquip)
                {
                    playerAttribute.Increase(attribute.value);
                } else
                {
                    playerAttribute.Decrease(attribute.value);
                }
            }
        }
    }
}
