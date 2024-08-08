using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemOnBar : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private Image rarityImage;
    [SerializeField] private Image itemImage;

    public InventoryItem item { get; private set; }
    
    public void Set(InventoryItem item, Color color)
    {
        if (item != null)
        {
            this.item = item;

            rarityImage.color = color;
            itemImage.sprite = item.data.sprite;
            itemImage.color = new Color(255, 255, 255, 255);
        }
    }

    public void Clear()
    {
        item = null;

        rarityImage.color = new Color(0 ,0, 0, 255);
        itemImage.sprite = null;
        itemImage.color = new Color(255, 255, 255, 0);
    }
}
