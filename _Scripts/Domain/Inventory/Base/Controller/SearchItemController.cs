using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchItemController : MonoBehaviour
{
    private Grid autoSearchInventory;
    private SearchingItem currentItem;

    private bool isAutoSearch;

    public void SetSearchingItem(InventoryItem item)
    {
        SearchingItem searchingItem = item.GetComponent<SearchingItem>();
        searchingItem.Set(item);
    }

    public void SearchItems(Grid inventory, bool autoSearchNext = true)
    {
        SearchingItem[] searchingItems = inventory.GetComponentsInChildren<SearchingItem>();
        isAutoSearch = autoSearchNext;

        foreach (SearchingItem item in searchingItems)
        {
            if (item.needSearch)
            {
                currentItem = item;
                autoSearchInventory = inventory;

                item.OnSearchingDone += HandleOnSearchDone;
                item.StartSearch();

                break;
            }
        }
    }

    public void StopSearch()
    {
        if (currentItem != null)
        {
            currentItem.ClearSpinner();
        }
    }

    private void HandleOnSearchDone()
    {
        currentItem = null;
        if (isAutoSearch)
        {
            SearchItems(autoSearchInventory, autoSearchNext: true);
        }
    }
}
