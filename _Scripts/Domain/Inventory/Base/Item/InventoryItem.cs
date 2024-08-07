using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] rectsToSetSize;
    [SerializeField]
    private RectTransform[] rectsToSwitchVisible;
    [SerializeField]
    private RectTransform backgroundTransform;
    [SerializeField]
    private RectTransform itemTransform;

    public InventoryItemSO data {  get; private set; }
    public Vector2Int pivotPositionOnGrid { get; private set; }
    public Rarity rarity { get; private set; }
    public BonusAttribute[] baseAttributes { get; private set; }
    public string buffDesc { get; private set; }

    public BaseBuffDataSO currentBuffData;

    public int width
    {
        get => isRotated ? data.size.y : data.size.x;
    }
    public int height
    {
        get => isRotated ? data.size.x : data.size.y;
    }

    private RectTransform rectTransform;
    private Grid parentGrid;

    private bool isRotated;

    private AttributeHelper attributeHelper;
    private SearchingItem searchingItem;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        attributeHelper = GameObject.Find("Helper").GetComponent<AttributeHelper>();
    }

    private void OnEnable()
    {
        isRotated = false;
    }

    private void OnDestroy()
    {
        if(searchingItem != null)
        {
            searchingItem.OnSearchingDone -= HandleSearchDone;
        }
    }

    public virtual void Set(InventoryItemSO itemSO, RectTransform parent, Rarity rarity, int tileSize)
    {
        data = itemSO;
        this.rarity = rarity;
        rectTransform.SetParent(parent);
        rectTransform.localScale = Vector3.one;

        SetSize(tileSize);

        itemTransform.GetComponent<Image>().sprite = itemSO.sprite;

        backgroundTransform.GetComponent<Image>().material = attributeHelper.GetRarityColorMaterial(rarity);

        SetBaseAttribute(data);
        SetBuff(data);
    }

    public void SetSize(Vector2 size)
    {
        foreach(RectTransform rect in rectsToSetSize)
        {
            rect.sizeDelta = size;
        }
    }
    
    public void SetSize(int tileSize)
    {
        Vector2 size = new Vector2(0, 0);
        size.x = width * tileSize;
        size.y = height * tileSize;

        SetSize(size);
    }

    public void SwitchItemVisible(bool isVisible)
    {
        if (!isVisible && searchingItem == null)
        {
            searchingItem = GetComponent<SearchingItem>();
            searchingItem.OnSearchingDone += HandleSearchDone;
        }

        foreach(RectTransform rect in rectsToSwitchVisible)
        {
            rect.gameObject.SetActive(isVisible);
        }
    }

    public void SetPivotPostion(Grid grid, Vector2Int pos)
    {
        parentGrid = grid;
        pivotPositionOnGrid = pos;
    }

    public void SetBaseAttribute(InventoryItemSO data)
    {
        baseAttributes = new BonusAttribute[0];

        if (data.baseAttributeByRarities.Length == 0)
        {
            return;
        }

        BonusAttribute[] attributes = data.baseAttributeByRarities.Where(attrubuteInfo => attrubuteInfo.rarity == rarity).ToArray()[0].attributes;
        if (attributes.Length > 0)
        {
            baseAttributes = attributes;
        }
    }

    public void SetBuff(InventoryItemSO data)
    {
        if (data.buffPrefab != null && data.buffDataByRarities.Length > 0)
        {
            currentBuffData =  data.buffDataByRarities.Where(item => item.rarity == rarity).First().buffData;
            buffDesc = data.buffPrefab.GetDesc(hasDurationText: true, specificData: currentBuffData);
        }
    }

    public void Rotate()
    {
        isRotated = !isRotated;
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated ? 90f : 0f);
    }

    public virtual void ApplyBuff(BuffManager buffManager, bool destroyOnUse = false)
    {
        if (currentBuffData != null)
        {
            buffManager.Add(data.buffPrefab, currentBuffData);

            parentGrid.RemoveItem(this);

            InventorySound inventorySound = parentGrid.GetComponentInParent<InventorySound>();

            if (data.itemType == ItemType.Consumable)
            {
                inventorySound.PlayConsumePotionAudio();
            }

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }

    private void HandleSearchDone()
    {
        SwitchItemVisible(true);
    }
}

public class BaseLootData
{
    public InventoryItemSO data;
    public Vector2Int pivotPositionOnGrid;
    public Rarity rarity;
    public BonusAttribute[] bonusAttributes;
    public bool needSearch;

    public BaseLootData(InventoryItem item)
    {
        data = item.data;
        pivotPositionOnGrid = item.pivotPositionOnGrid;
        rarity = item.rarity;
        
        if (item.data.itemType == ItemType.Equipment)
        {
            EquipmentItem equipment = item as EquipmentItem;
            bonusAttributes = equipment.bonusAttributes;
        }

        SearchingItem searchingItem = item.GetComponent<SearchingItem>();
        needSearch = searchingItem.needSearch;
    }
}
