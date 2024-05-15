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

    private Transform shaderTransform;

    private void Awake()
    {
        shaderTransform = transform.Find("BackgroundColor");
    }

    public void Set(InventoryItemSO itemSO, int tileSize)
    {
        data = itemSO;

        // 设置自身
        Vector2 size = new Vector2(0, 0);
        size.x = data.size.x * tileSize;
        size.y = data.size.y * tileSize;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        rectTransform.localScale = Vector3.one;
        GetComponent<Image>().sprite = itemSO.sprite;

        // 设置背景
        shaderTransform.GetComponent<RectTransform>().sizeDelta = size;
        shaderTransform.GetComponent<Image>().sprite = itemSO.sprite;

        // 设置shader material
        foreach (Material mat in rarityMaterials)
        {
            if (mat.GetFloat("_Rarity") == (float)itemSO.defaultRarity)
            {
                shaderTransform.GetComponent<Image>().material = mat;
                break;
            }
        }
    }
}
