using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemIndicatorController : MonoBehaviour
{
    [SerializeField]
    public GameObject placeItemIndicatorPrefab;
    
    private InventoryController inventoryController;

    private RectTransform indicatorRect;
    private PlaceItemIndicator placeItemIndicator;
    private Vector2Int? lastIndicatorPos;
    private int indicatorIndex;

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }
    public void ShowIndicator(Vector2Int pos, InventoryItem itemToPlace)
    {
        if (pos == null || lastIndicatorPos == pos)
        {
            return;
        }
        lastIndicatorPos = pos;
        Grid selectedInventory = inventoryController.selectedInventory;

        if (indicatorRect == null)
        {
            indicatorRect = Instantiate(placeItemIndicatorPrefab, selectedInventory.transform).GetComponent<RectTransform>();

            placeItemIndicator = indicatorRect.GetComponent<PlaceItemIndicator>();
        }
        else if (!indicatorRect.gameObject.activeSelf)
        {
            indicatorRect.gameObject.SetActive(true);
            if (indicatorRect.parent != selectedInventory.transform)
            {
                indicatorRect.SetParent(selectedInventory.transform);
            }
        }

        RectTransform itemRect = itemToPlace.GetComponent<RectTransform>();

        int index = itemRect.GetSiblingIndex();
        if (indicatorIndex != index)
        {
            indicatorIndex = index;
            indicatorRect.SetSiblingIndex(index - 1 < 0 ? 0 : index - 1);
        }

        bool isSafeToPlace = 
            selectedInventory.CheckItemInBoundary(pos, itemToPlace.data.size) 
            && selectedInventory.CheckItemOverlap(itemToPlace, pos) 
            && selectedInventory.CheckItemAllowed(itemToPlace);
        placeItemIndicator.SwitchStatus(isSafeToPlace);

        indicatorRect.sizeDelta = itemRect.sizeDelta;
        indicatorRect.localPosition = selectedInventory.GetGridObsolutePosition(pos, itemToPlace.width, itemToPlace.height);
    }

    public void HideIndicator()
    {
        if (indicatorRect != null)
        {
            indicatorIndex = -1;
            lastIndicatorPos = null;
            indicatorRect.gameObject.SetActive(false);
        }
    }
}
