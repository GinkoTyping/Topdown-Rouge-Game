using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Grid))]
public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryController inventoryController;
    private Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        inventoryController = GetComponentInParent<InventoryController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SetSelectedInventory(grid);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SetSelectedInventory(null) ;
    }
}
