using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackInventoryItemPos : MonoBehaviour
{
    private InventoryItem currentItem;

    public void Set(InventoryItem item)
    {
        currentItem = item;
    }

    void Update()
    {
        if (currentItem != null && transform.position != currentItem.transform.position)
        {
            transform.position = currentItem.transform.position;
        }
    }
}
