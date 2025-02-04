using Ginko.PlayerSystem;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private InventoryController inventoryController;
    private InventorySound soundController;
    private DroppedItemController droppedItemController;

    [SerializeField]
    public int tileSize;
    [SerializeField]
    public Vector2Int inventorySize;
    [SerializeField]
    public ItemType[] allowedItemTypes;

    private RectTransform inventoryRectTransform;
    private RectTransform gridRectTransform;
    private Vector2 positionOnGrid = Vector2.zero;
    private Vector2Int tileGridPosition = new Vector2Int();
    private InventoryItem[,] inventoryItemSlot;

    public event Action<bool, InventoryItem> OnItemChange;
    

    void Awake()
    {
        inventoryRectTransform = GetComponentInParent<RectTransform>();
        gridRectTransform = GetComponent<RectTransform>();
        inventoryController = GetComponentInParent<InventoryController>();
        soundController = GetComponentInParent<InventorySound>();

        Init(inventorySize.x, inventorySize.y);
    }

    private void Start()
    {
        inventoryController.onInventoryChange += HandleInventoryChange;

        droppedItemController = GameObject.Find("Drops").GetComponent<DroppedItemController>();
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

    public Vector2 GetGridObsolutePosition(Vector2Int position, float width, float height)
    {
        Vector2 output = Vector2.zero;

        // int的除法不会保留小数部分
        output.x = position.x * tileSize + (float)tileSize * width / 2;
        output.y = -position.y * tileSize - (float)tileSize * height / 2;

        return output;
    }

    public Vector2 GetGridObsolutePosition(InventoryItem item)
    {
        return GetGridObsolutePosition(item.pivotPositionOnGrid, item.width, item.height);
    }

    public bool PlaceItem(InventoryItem item, Vector2Int? position, bool playAudio = true)
    {
        if (position == null)
        {
            Debug.Log("No room to place item.");
            return false;
        }

        Vector2Int pos = (Vector2Int)position;

        item.SetPivotPostion(this, pos);

        if (!CheckItemAllowed(item))
        {
            Debug.Log("Not allowed.");
            return false;
        }
        
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

        if (playAudio)
        {
            soundController.PlayPlaceItemAudio(item);
        }

        OnItemChange?.Invoke(true, item);

        return true;
    }

    public InventoryItem GetItem(Vector2Int pos)
    {
        if (pos.x < 0  ||  pos.y < 0)
        {
            return null;
        }
        return inventoryItemSlot[pos.x, pos.y];
    }
    
    public InventoryItem PickUpItem(Vector2Int pos, bool? playSound = true)
    {
        InventoryItem item = inventoryItemSlot[pos.x, pos.y];
        if (item == null)
        {
            return null;
        }

        RemoveItem(item);

        if ((bool)playSound)
        {
            soundController.PlayPickUpItemAudio();
        }

        return item;
    }
    
    public InventoryItem RemoveItem(Vector2Int pos, bool? isClear = false)
    {
        InventoryItem item = inventoryItemSlot[pos.x, pos.y];
        if (item != null)
        {
            RemoveItem(item, isClear);
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
            droppedItemController.CreateDroppedItem(item);
            soundController.PlayRemoveItemAudio();
        }

        OnItemChange?.Invoke(false, item);

        return item;
    }
    
    public Vector2Int? GetSpaceToPlaceItem(InventoryItem item, bool autoPlace = true, bool playAudio = true)
    {
        int maxWidth = inventorySize.x - item.width + 1;
        int maxHeight = inventorySize.y - item.height + 1;

        Vector2Int? postion = null;

        for (int y = 0; y < maxHeight; y++)
        {
            if (postion != null)
            {
                break;
            }

            for (int x = 0; x < maxWidth; x++)
            {
                Vector2Int output = new Vector2Int(x, y);
                if (CheckItemOverlap(item, output))
                {
                    postion = output;
                    break;
                }
            }
        }

        if (postion != null && autoPlace)
        {
            PlaceItem(item, postion, playAudio);
        }

        return postion;
    }

    #region Checks
    public bool CheckItemOverlap(InventoryItem item, Vector2Int pos)
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
    
    private bool CheckPositionInGrid(Vector2 position)
    {
        if (position.x < 0 || position.y <0 || position.x > inventorySize.x - 1 || position.y > inventorySize.y - 1)
        {
            return false;
        }
        return true;
    }

    public bool CheckItemInBoundary(Vector2Int position, Vector2 size)
    {
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
    
    public bool CheckItemInBoundary(InventoryItem item)
    {
        return CheckItemInBoundary(item.pivotPositionOnGrid, item.data.size);
    }

    public bool CheckItemAllowed(InventoryItem item)
    {
        return allowedItemTypes.Contains(item.data.itemType);
    }
    #endregion
}
