using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class Grid : MonoBehaviour
{
    private PlayerInputEventHandler inputHandler;
    private InventoryController inventoryController;

    [SerializeField]
    public int tileSize;
    [SerializeField]
    public Vector2Int inventorySize;
    [SerializeField]
    public GameObject testItem;

    private RectTransform inventoryRectTransform;
    private RectTransform gridRectTransform;
    private Vector2 positionOnGrid = Vector2.zero;
    private Vector2Int tileGridPosition = new Vector2Int();
    private InventoryItem[,] inventoryItemSlot;

    void Awake()
    {
        inventoryRectTransform = GetComponentInParent<RectTransform>();
        gridRectTransform = GetComponent<RectTransform>();
        inventoryController = GetComponentInParent<InventoryController>();

        Init(inventorySize.x, inventorySize.y);
    }

    private void Start()
    {
        inputHandler = Player.Instance.InputHandler;
        inventoryController.onInventoryChange += HandleInventoryChange;
    }

    private void OnDestroy()
    {
        inventoryController.onInventoryChange -= HandleInventoryChange;
    }
    private void HandleInventoryChange()
    {
        if (inventoryController.selectedItem != null)
        {
            RectTransform itemTransform = inventoryController.selectedItem.GetComponent<RectTransform>();
            RectTransform gridTransform = inventoryController.selectedInventory.GetComponent<RectTransform>();
            itemTransform.SetParent(gridTransform);
        }
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSize, height * tileSize);

        gridRectTransform.sizeDelta = size;
        inventoryRectTransform.sizeDelta = size;
    }
    public Vector2Int GetGridRelativePosition(Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - inventoryRectTransform.position.x;
        positionOnGrid.y = mousePosition.y - inventoryRectTransform.position.y;

        tileGridPosition.x = Mathf.FloorToInt(positionOnGrid.x / tileSize);
        tileGridPosition.y = -Mathf.FloorToInt(positionOnGrid.y / tileSize) - 1;

        return tileGridPosition;
    }

    private Vector2 GetGridObsolutePosition(InventoryItem item)
    {
        Vector2 output = Vector2.zero;
        Vector2Int position = item.pivotPositionOnGrid;

        // int的除法不会保留小数部分
        output.x = position.x * tileSize + (float)tileSize * item.data.size.x / 2;
        output.y = -position.y * tileSize - (float)tileSize * item.data.size.y / 2;

        return output;
    }

    public bool PlaceItem(InventoryItem item, Vector2Int pos)
    {
        item.pivotPositionOnGrid = pos;

        if (!CheckItemInBoundary(item))
        {
            Debug.Log("No room to place item.");
            return false;
        }

        if (!CheckItemOverlap(item, pos))
        {
            Debug.Log("No room to place item.");
            return false;
        }

        for (int x = 0; x < item.data.size.x; x++)
        {
            for(int y = 0; y < item.data.size.y; y++)
            {
                inventoryItemSlot[pos.x + x, pos.y + y] = item;
            }
        }

        RectTransform itemTransform = item.GetComponent<RectTransform>();
        itemTransform.SetParent(gridRectTransform);

        itemTransform.localPosition = GetGridObsolutePosition(item);

        return true;
    }

    private bool CheckItemOverlap(InventoryItem item, Vector2Int pos)
    {
        for (int x = 0; x < item.data.size.x; x++)
        {
            for (int y = 0; y < item.data.size.y; y++)
            {
                InventoryItem currentItem = inventoryItemSlot[pos.x + x, pos.y + y];
                if (currentItem != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public InventoryItem PickUpItem(Vector2Int pos)
    {
        InventoryItem item = inventoryItemSlot[pos.x, pos.y];
        if (item == null)
        {
            return null;
        }

        RemoveItem(item);

        return item;
    }

    private void RemoveItem(InventoryItem item)
    {
        for (int x = 0; x < item.data.size.x; x++)
        {
            for (int y = 0; y < item.data.size.y; y++)
            {
                inventoryItemSlot[item.pivotPositionOnGrid.x + x, item.pivotPositionOnGrid.y + y] = null;
            }
        }
    }

    private bool CheckPositionInGrid(Vector2 position)
    {
        if (position.x < 0 || position.y <0 || position.x > inventorySize.x - 1 || position.y > inventorySize.y - 1)
        {
            return false;
        }
        return true;
    }

    private bool CheckItemInBoundary(InventoryItem item)
    {
        Vector2 position = item.pivotPositionOnGrid;
        Vector2 size = item.data.size;

        if (!CheckPositionInGrid(position))
        {
            return false;
        }

        Vector2 bottomRightPosition = new Vector2(position.x + size.x - 1, position.y + size.y - 1);
        if (!CheckPositionInGrid(bottomRightPosition))
        {
            return false;
        }

        return true;
    }
}
