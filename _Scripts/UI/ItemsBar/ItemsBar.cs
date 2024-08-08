using Ginko.PlayerSystem;
using System;
using System.Linq;
using UnityEngine;

public class ItemsBar : MonoBehaviour
{
    [SerializeField] private Grid pockectInventory;
    [SerializeField] private RarityToColor[] raritiesToColor;

    private ItemOnBar[] items;
    private PlayerInputEventHandler playerInputEventHandler;
    private BuffManager playerBuffManager;

    [Serializable]
    private class RarityToColor
    {
        public Rarity rarity;
        public Color color;
    }

    private void Awake()
    {
        items = GetComponentsInChildren<ItemOnBar>();
    }

    private void OnEnable()
    {
        pockectInventory.OnItemChange += UpdateItemsOnBar;
    }

    private void OnDisable()
    {
        pockectInventory.OnItemChange -= UpdateItemsOnBar;
    }

    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        playerBuffManager = Player.Instance.Core.GetCoreComponent<BuffManager>();
    }

    private void Update()
    {
        HandleUseItem();
    }

    private void HandleUseItem()
    {
        if (playerInputEventHandler.ConsumePotion) 
        {
            playerInputEventHandler.UseConsumePotionSignal();

            items[0].item.Ability.Use(true);
        }
    }

    private Color GetColorByRaity(InventoryItem item)
    {
        return raritiesToColor.Where(i => i.rarity == item.rarity).First().color;
    }

    private void UpdateItemsOnBar(bool isAdd, InventoryItem item)
    {
        if (isAdd)
        {
            if (items[0].item == null)
            {
                items[0].Set(item, GetColorByRaity(item));
            }
        } 
        else
        {
            InventoryItem[] leftItems = pockectInventory.GetComponentsInChildren<InventoryItem>().Where(i => i != item).ToArray();

            if (leftItems.Length == 0)
            {
                items[0].Clear();
            } else
            {
                items[0].Set(leftItems[0], GetColorByRaity(leftItems[0]));
            }
        }
    }
}
