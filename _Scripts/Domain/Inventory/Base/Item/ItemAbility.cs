using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class ItemAbility : MonoBehaviour
{
    private InventoryItem item;
    private BuffManager playerBuffManager;
    private Stats playerStats;
    private InventoryController inventoryController;

    private void Awake()
    {
        item = GetComponent<InventoryItem>();
    }

    public virtual void Use(bool destroyOnUse = false)
    {
        RemoveItemOnGrid();
        UpdateBuff(true, destroyOnUse);
    }

    public virtual void Equip()
    {
        RemoveItemOnGrid();

        UpdateBuff(true, false);
        UpdatePlayerAttribute(true);
    }

    public virtual void Unequip(Grid inventoryToUnequip, Vector2Int? position = null)
    {
        UnequipEquipmentOnGrid(inventoryToUnequip, position);

        UpdateBuff(false, false);
        UpdatePlayerAttribute(false);
    }

    public virtual void UpdateBuff(bool isEquip, bool destroyOnUse = false)
    {
        if (item.currentBuffData != null)
        {

            if (playerBuffManager == null)
            {
                playerBuffManager = Player.Instance.Core.GetCoreComponent<BuffManager>();
            }

            if (isEquip)
            {
                playerBuffManager.Add(item.data.buffPrefab, item.currentBuffData);

                InventorySound inventorySound = item.parentGrid.GetComponentInParent<InventorySound>(true);

                if (item.data.itemType == ItemType.Consumable)
                {
                    inventorySound.PlayConsumePotionAudio();
                }

                if (destroyOnUse)
                {
                    Destroy(item.gameObject);
                }
            }
            else
            {
                Buff buffToDisable = playerBuffManager.buffList.Where(i => i.gameObject.name == item.data.buffPrefab.name).FirstOrDefault();
                if (buffToDisable != null)
                {
                    buffToDisable.gameObject.SetActive(false);
                }
            }
        }
    }

    private void RemoveItemOnGrid()
    {
        item.parentGrid.RemoveItem(item);
    }

    private void UnequipEquipmentOnGrid(Grid inventoryToUnequip, Vector2Int? position = null)
    {
        item.SetSize(inventoryToUnequip.tileSize);

        if (inventoryController == null)
        {
            inventoryController = GameObject.Find("UI").transform.Find("InventoryCanvas").GetComponentInChildren<InventoryController>();
        }
        
        if (position == null)
        {
            item.GetComponent<RectTransform>().SetParent(inventoryToUnequip.transform);
            inventoryController.SetSelectedItem(item);
        }
        else
        {
            inventoryToUnequip.PlaceItem(item, (Vector2Int)position);
            inventoryController.SetSelectedItem(null);
        }
    }

    private void UpdatePlayerAttribute(bool isEquip)
    {
        EquipmentItem equipmentItem = item as EquipmentItem;
        BonusAttribute[] attributes = equipmentItem.bonusAttributes.Concat(equipmentItem.baseAttributes).ToArray();

        if (playerStats == null)
        {
            playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
        }

        foreach (BonusAttribute attribute in attributes)
        {
            if (attribute.value != 0)
            {
                AttributeStat playerAttribute = playerStats.GetAttribute(attribute.type);

                if (isEquip)
                {
                    playerAttribute.Increase(attribute.value);
                    if (attribute.type == AttributeType.MaxHealth)
                    {
                        playerStats.GetAttribute(ResourceType.Health).ChangeMaxValue(attribute.value);
                    }
                }
                else
                {
                    playerAttribute.Decrease(attribute.value);
                    if (attribute.type == AttributeType.MaxHealth)
                    {
                        playerStats.GetAttribute(ResourceType.Health).ChangeMaxValue(-attribute.value);
                    }
                }
            }
        }
    }
}
