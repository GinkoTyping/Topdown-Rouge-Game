using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private PoolManager poolManager;
    private Transform hoverPropsContainer;
    private RectTransform rectTransform;
    private RectTransform hoverContainer;

    public InventoryItem currentItem;

    private void Awake()
    {
        nameInfo = transform.Find("Name").GetComponent<TextMeshProUGUI>();
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

        rectTransform.SetParent(hoverContainer);
        rectTransform.SetAsLastSibling();
        rectTransform.position = pos;

        ClearProps();

        ItemType type = item.data.itemType;
        if (type == ItemType.Equipment)
        {
            SetEquipmentProps((EquipmentItemSO)item.data);
        }
    }

    public void Hide()
    {
        currentItem = null;
        gameObject.SetActive(false);
    }

    private void ClearProps()
    {
        InventoryItemProp[] itemProps =  hoverPropsContainer.GetComponentsInChildren<InventoryItemProp>();

        foreach (var item in itemProps)
        {
            poolManager.Pool.Release(item.gameObject);
        }
    }

    private void SetEquipmentProps(EquipmentItemSO data)
    {
        for (int i = 0; i < data.bonusAttributes.Length; i++)
        {
            GameObject prop = poolManager.Pool.Get();
            prop.GetComponent<InventoryItemProp>().Set(data.bonusAttributes[i]);

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
