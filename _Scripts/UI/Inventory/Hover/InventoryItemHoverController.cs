using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryItemHoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject propPrefab;
    [SerializeField]
    private float attributePadding;

    const float TOP_PADDING = 40.0f;
    const float BOTTOM_PADDING = 90.0f;
    const float DEFAULT_HEIGHT = 250.0f;

    private Image nameBGImage;
    private RectTransform typeInfoLine;
    private TextMeshProUGUI nameInfo;
    private TextMeshProUGUI typeInfo;
    private TextMeshProUGUI rarityInfo;
    private TextMeshProUGUI priceInfo;

    private PoolManager poolManager;
    private AttributeHelper attributeHelper;

    private Transform bonusAttributeContainer;
    private Transform baseAttributeContainer;
    private RectTransform rectTransform;
    private RectTransform hoverContainer;
    private RectTransform backgroundTransform;
    private RectTransform borderTransform;


    private float scaleUnit;

    public InventoryItem currentItem { get; private set; }

    private void Awake()
    {
        nameInfo = transform.Find("Name").GetComponentInChildren<TextMeshProUGUI>();
        nameBGImage = transform.Find("Name").Find("Background").GetComponent<Image>();
        typeInfo = transform.Find("TypeInfo").Find("Type").GetComponent<TextMeshProUGUI>();
        rarityInfo = transform.Find("TypeInfo").Find("Rarity").GetComponent<TextMeshProUGUI>();
        typeInfoLine = transform.Find("TypeInfo").Find("Line").GetComponent<RectTransform>();
        priceInfo = transform.Find("Price").GetComponentInChildren<TextMeshProUGUI>();

        baseAttributeContainer = transform.Find("BaseAttribute").transform;
        bonusAttributeContainer = transform.Find("BonusAttribute").transform;

        rectTransform = GetComponent<RectTransform>();
        backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        borderTransform = transform.Find("Border").GetComponent<RectTransform>();
        hoverContainer = GameObject.Find("HoverContainer").GetComponent<RectTransform>();

        attributeHelper = GameObject.Find("Helper").GetComponent<AttributeHelper>();

        scaleUnit = Camera.main.orthographicSize / Screen.height * 2;
    }
    
    private void Start()
    {
        Hide();

        poolManager = GetComponent<PoolManager>();
        poolManager.SetCurrrentObject(propPrefab);
    }

    public void Set(InventoryItem item, Vector2 pos)
    {
        currentItem = item;
        gameObject.SetActive(true);

        ClearProps();
        if (pos != Vector2.zero)
        {
            SetPosition(pos);
        }
        SetBasicInfo(item);
        SetSellingPrice(item);

        ItemType type = item.data.itemType;
        if (type == ItemType.Equipment)
        {
            SetBaseAttributes(item);
            SetBonusAttributes((EquipmentItem)item);
        } else if (type == ItemType.Consumable)
        {
            SetBaseAttributes(item);
        }

        SetHeight(item);
    }
    
    public void Set(InventoryItem item, RectTransform relativeTransform)
    {
        Set(item, Vector2.zero);
        SetPosition(CalculateHoverPos(relativeTransform));
    }

    private void SetHeight(InventoryItem item)
    {
        float height;
        if (item.data.itemType == ItemType.Equipment)
        {
            EquipmentItem equipmentItem = (EquipmentItem)item;
            float attributeUI = GetAttributeUIHeight(equipmentItem.bonusAttributes.Concat(equipmentItem.baseAttributes).ToArray());
            height = TOP_PADDING + attributeUI + BOTTOM_PADDING;
        } 
        else if (item.data.itemType == ItemType.Treasure)
        {
            height = TOP_PADDING + BOTTOM_PADDING;
        }
        else if (item.data.itemType == ItemType.Consumable)
        {
            float attributeUI = GetAttributeUIHeight(item.baseAttributes);
            height = TOP_PADDING + attributeUI + BOTTOM_PADDING;
        }
        else
        {
            height = DEFAULT_HEIGHT;
        }

        Vector2 size = new Vector2(rectTransform.sizeDelta.x, height);

        rectTransform.sizeDelta = size;
        backgroundTransform.sizeDelta = size;
        borderTransform.sizeDelta = size;
    }

    private Vector2 CalculateHoverPos(RectTransform relative)
    {
        Vector3 hoverPosition = Camera.main.WorldToScreenPoint(relative.position);

        float x;
        float y;
        x = hoverPosition.x + rectTransform.sizeDelta.x > Screen.width
        ? relative.position.x - relative.sizeDelta.x * scaleUnit / 2 - rectTransform.sizeDelta.x * scaleUnit
                : relative.position.x + relative.sizeDelta.x * scaleUnit / 2;
        y = hoverPosition.y - rectTransform.sizeDelta.y < 0
        ? relative.position.y - relative.sizeDelta.y * scaleUnit / 2 + rectTransform.sizeDelta.y * scaleUnit
        : relative.position.y + relative.sizeDelta.y * scaleUnit / 2;

        return new Vector2(x, y);
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            currentItem = null;
            gameObject.SetActive(false);
        }
    }

    private void ClearProps()
    {
        InventoryItemHoverAttribute[] bonusProps =  bonusAttributeContainer.GetComponentsInChildren<InventoryItemHoverAttribute>();
        InventoryItemHoverAttribute[] baseProps =  baseAttributeContainer.GetComponentsInChildren<InventoryItemHoverAttribute>();

        InventoryItemHoverAttribute[] ToClear = bonusProps.Concat(baseProps).ToArray();

        foreach (var item in ToClear)
        {
            poolManager.Pool.Release(item.gameObject);
        }
    }

    private void SetPosition(Vector2 pos)
    {
        rectTransform.SetParent(hoverContainer);
        rectTransform.SetAsLastSibling();
        rectTransform.position = pos;
    }
    
    private void SetBasicInfo(InventoryItem item)
    {
        InventoryItemSO data = item.data;
        nameInfo.text = data.itemName;
        Color color = attributeHelper.GetAttributeColor(item.rarity, false);
        nameInfo.color = color;
        nameBGImage.color = color;

        rarityInfo.text = $"Rarity: <#{ColorToHex(color)}>{item.rarity}</color>";

        typeInfoLine.gameObject.SetActive(data.itemType != ItemType.Treasure);

        if (data.itemType == ItemType.Equipment)
        {
            EquipmentItemSO equipmentData = (EquipmentItemSO)data;
            typeInfo.text = $"Type: {equipmentData.equipmentType}";
        }
        else
        {
            typeInfo.text = $"Type: {data.itemType}";
        }

    }

    private void SetBaseAttributes(InventoryItem item)
    {
        poolManager.SetCurrentParrent(baseAttributeContainer);

        for (int i = 0; i < item.baseAttributes.Length; i++)
        {
            GameObject prop = poolManager.Pool.Get();
            prop.GetComponent<InventoryItemHoverAttribute>().Set(item.baseAttributes[i], true);

            RectTransform rect = prop.GetComponent<RectTransform>();

            rect.localPosition = new Vector2(0, -TOP_PADDING - i * attributePadding);
        }
    }

    private void SetBonusAttributes(EquipmentItem item)
    {
        poolManager.SetCurrentParrent(bonusAttributeContainer);
        int baseAttributeCount = item.baseAttributes.Length;

        for (int i = 0; i < item.bonusAttributes.Length; i++)
        {
            GameObject prop = poolManager.Pool.Get();
            prop.GetComponent<InventoryItemHoverAttribute>().Set(item.bonusAttributes[i]);

            RectTransform rect = prop.GetComponent<RectTransform>();

            rect.localPosition = new Vector2(0, -TOP_PADDING - (i + baseAttributeCount) * attributePadding);
        }
    }

    private void SetSellingPrice(InventoryItem item)
    {
        LootPriceDataSO lootPrices = item.data.priceSetting;
        int price = lootPrices.priceList[(int)item.rarity].price;

        priceInfo.text = $": {price}g";
    }

    private float GetAttributeUIHeight(BonusAttribute[] attributes)
    {
        return attributes.Length * attributePadding;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hide();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    #region Utils
    private string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        return hex;
    }
    #endregion
}
