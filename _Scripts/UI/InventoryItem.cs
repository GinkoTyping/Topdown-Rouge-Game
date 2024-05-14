using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemSO data;
    public Vector2Int pivotPositionOnGrid;

    public void Set(InventoryItemSO itemSO, int tileSize)
    {
        data = itemSO;

        // 设置自身
        GetComponent<Image>().sprite = itemSO.sprite;

        Vector2 size = new Vector2(0, 0);
        size.x = data.size.x * tileSize;
        size.y = data.size.y * tileSize;
        GetComponent<RectTransform>().sizeDelta = size;

        // 设置背景shader属性
        Transform bgTransform = transform.Find("BackgroundColor");
        Material bgMaterial = bgTransform.GetComponent<Image>().material;
        Texture texture = transform.GetComponent<Image>().mainTexture;
        bgMaterial.SetTexture("_MainTex", texture);
        bgTransform.GetComponent<RectTransform>().sizeDelta = size;
    }
}
