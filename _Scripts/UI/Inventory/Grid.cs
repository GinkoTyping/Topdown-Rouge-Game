using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private PlayerInputEventHandler inputHandler;
    private InventoryController inventoryController;

    [SerializeField]
    public int tileSize;
    [SerializeField]
    public Vector2Int inventorySize;

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
        positionOnGrid.x = mousePosition.x - gridRectTransform.position.x;
        positionOnGrid.y = mousePosition.y - gridRectTransform.position.y;

        tileGridPosition.x = (int)Math.Floor(positionOnGrid.x / (inventoryController.scaleParam * tileSize));
        tileGridPosition.y = (int)-Math.Floor(positionOnGrid.y / (inventoryController.scaleParam * tileSize)) - 1;

        return tileGridPosition;
    }

    private Vector2 GetGridObsolutePosition(InventoryItem item)
    {
        Vector2 output = Vector2.zero;
        Vector2Int position = item.pivotPositionOnGrid;

        // int的除法不会保留小数部分
        output.x = position.x * tileSize + (float)tileSize * item.width / 2;
        output.y = -position.y * tileSize - (float)tileSize * item.height / 2;

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

        for (int x = 0; x < item.width; x++)
        {
            for(int y = 0; y < item.height; y++)
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
        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
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
    public InventoryItem RemoveItem(Vector2Int pos)
    {
        InventoryItem item = inventoryItemSlot[pos.x, pos.y];
        if (item != null)
        {
            RemoveItem(item);
        }

        return item;
    }

    public InventoryItem RemoveItem(InventoryItem item, bool? isClear = false)
    {
        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                inventoryItemSlot[item.pivotPositionOnGrid.x + x, item.pivotPositionOnGrid.y + y] = null;
            }
        }

        if ((bool)isClear)
        {
            // TODO: 对象池
            Destroy(item.gameObject);
        }

        return item;
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

    public Vector2Int? GetSpaceForItem(InventoryItem item)
    {
        int maxWidth = inventorySize.x - item.width + 1;
        int maxHeight = inventorySize.y - item.height + 1;
        
        for (int y = 0; y < maxHeight; y++)
        {
            for (int x = 0; x < maxWidth; x++)
            {
                Vector2Int output = new Vector2Int(x, y);
                if (CheckItemOverlap(item, output))
                {
                    return output;
                }
            }
        }

        return null;
    }
}
