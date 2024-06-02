using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    public Color[] rarityColors;

    public InventoryItemSO data;
    public Vector2Int pivotPositionOnGrid;
    public Rarity rarity;
    public int width
    {
        get => isRotated ? data.size.y : data.size.x;
    }
    public int height
    {
        get => isRotated ? data.size.x : data.size.y;
    }

    private RectTransform backgroundTransform;
    private RectTransform itemTransform;
    private RectTransform rectTransform;
    private bool isRotated;

    protected virtual void Awake()
    {
        backgroundTransform = transform.Find("BackgroundColor").GetComponent<RectTransform>();
        itemTransform = transform.Find("Item").GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        isRotated = false;
    }
     
    public virtual void Set(InventoryItemSO itemSO, RectTransform parent, Rarity rarity, int tileSize)
    {
        data = itemSO;
        this.rarity = rarity;
        rectTransform.SetParent(parent);
        rectTransform.localScale = Vector3.one;

        SetSize(tileSize);

        itemTransform.GetComponent<Image>().sprite = itemSO.sprite;

        for(int i = 0; i < rarityColors.Length; i++)
        {
            if (i == (int)rarity)
            {
                backgroundTransform.GetComponent<Image>().color = rarityColors[i];

                break;
            }
        }
    }

    public void SetSize(Vector2 size)
    {
        rectTransform.sizeDelta = size;
        itemTransform.sizeDelta = size;
        backgroundTransform.sizeDelta = size;
    }
    public void SetSize(int tileSize)
    {
        Vector2 size = new Vector2(0, 0);
        size.x = width * tileSize;
        size.y = height * tileSize;

        SetSize(size);
    }

    public void Rotate()
    {
        isRotated = !isRotated;
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated ? 90f : 0f);
    }
}

public class BaseLootData
{
    public InventoryItemSO data;
    public Vector2Int pivotPositionOnGrid;
    public Rarity rarity;
    public BonusAttribute[] bonusAttributes;

    public BaseLootData(InventoryItem item)
    {
        data = item.data;
        pivotPositionOnGrid = item.pivotPositionOnGrid;
        rarity = item.rarity;
        
        if (item.data.itemType == ItemType.Equipment)
        {
            EquipmentItem equipment = item as EquipmentItem;
            bonusAttributes = equipment.bonusAttributes;
        }
    }
}
