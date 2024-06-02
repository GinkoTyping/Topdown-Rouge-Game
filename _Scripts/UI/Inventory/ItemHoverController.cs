using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject propPrefab;
    [SerializeField]
    private float padding;

    private TextMeshProUGUI nameInfo;
    private TextMeshProUGUI typeInfo;
    private PoolManager poolManager;
    private Transform hoverPropsContainer;
    private RectTransform rectTransform;
    private RectTransform hoverContainer;

    public InventoryItem currentItem;

    private void Awake()
    {
        nameInfo = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        typeInfo = transform.Find("Type").GetComponent<TextMeshProUGUI>();

        hoverPropsContainer = transform.Find("Props").transform;
        rectTransform = GetComponent<RectTransform>();
        hoverContainer = GameObject.Find("HoverContainer").GetComponent<RectTransform>();
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
        SetPosition(pos);
        SetBasicInfo(item.data);

        ItemType type = item.data.itemType;
        if (type == ItemType.Equipment)
        {
            SetEquipmentDetailInfo((EquipmentItem)item);
        }
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
        InventoryItemProp[] itemProps =  hoverPropsContainer.GetComponentsInChildren<InventoryItemProp>();

        foreach (var item in itemProps)
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
    private void SetBasicInfo(InventoryItemSO data)
    {
        nameInfo.text = data.itemName;

        if (data.itemType == ItemType.Equipment)
        {
            EquipmentItemSO equipmentData = (EquipmentItemSO)data;
            typeInfo.text = equipmentData.equipmentType.ToString();
        }
        else
        {
            typeInfo.text = data.itemName.ToString();
        }

    }
    private void SetEquipmentDetailInfo(EquipmentItem item)
    {
        for (int i = 0; i < item.bonusAttributes.Length; i++)
        {
            GameObject prop = poolManager.Pool.Get();
            prop.GetComponent<InventoryItemProp>().Set(item.bonusAttributes[i]);

            RectTransform rect = prop.GetComponent<RectTransform>();

            rect.localPosition = new Vector2(0, -60 - i * padding);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hide();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
