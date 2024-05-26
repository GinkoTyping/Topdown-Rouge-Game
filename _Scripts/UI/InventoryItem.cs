using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    public Material[] rarityMaterials;

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

    private Transform shaderTransform;
    private RectTransform rectTransform;
    private bool isRotated;

    private void Awake()
    {
        shaderTransform = transform.Find("BackgroundColor");
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        isRotated = false;
    }

    public void Set(InventoryItemSO itemSO, Rarity rarity, int tileSize)
    {
        data = itemSO;
        this.rarity = rarity;

        // 设置自身
        Vector2 size = new Vector2(0, 0);
        size.x = width * tileSize;
        size.y = height * tileSize;
        rectTransform.sizeDelta = size;
        rectTransform.localScale = Vector3.one;
        GetComponent<Image>().sprite = itemSO.sprite;

        // 设置背景
        shaderTransform.GetComponent<RectTransform>().sizeDelta = size;
        shaderTransform.GetComponent<Image>().sprite = itemSO.sprite;

        // 设置shader material
        foreach (Material mat in rarityMaterials)
        {
            if (mat.GetFloat("_Rarity") == (float)rarity)
            {
                shaderTransform.GetComponent<Image>().material = mat;
                break;
            }
        }
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

    public BaseLootData(InventoryItem item)
    {
        data = item.data;
        pivotPositionOnGrid = item.pivotPositionOnGrid;
        rarity = item.rarity;
    }
}
