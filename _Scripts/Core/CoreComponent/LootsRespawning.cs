using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class LootsRespawning : CoreComponent
    {
        [SerializeField]
        public LootListDataSO lootSetting;

        private Grid lootInventory;
        private InventoryController inventoryController;
        private Interactable interactable;
        private GameObject inventoryUI;
        private UIManager UIManager;

        private bool looted;

        private List<BaseLootData> lootStorage = new List<BaseLootData>();

        protected override void Awake()
        {
            base.Awake();

            looted = false;

            UIManager = GameObject.Find("UI").GetComponent<UIManager>();
            lootInventory = GameObject.Find("Loot").GetComponentInChildren<Grid>();
            inventoryUI = GameObject.Find("Inventory");
            inventoryController = inventoryUI.GetComponent<InventoryController>();
            interactable = Core.GetCoreComponent<Interactable>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            UIManager.onInventoryUIClose += HandleInventoryClose;
        }

        private void OnDisable()
        {
            UIManager.onInventoryUIClose -= HandleInventoryClose;
        }

        public void OnLoots()
        {
            if (interactable.interactType == InteractType.Lootable)
            {
                OpenInventoryMenu();

                if (looted)
                {
                    RestoreLoots();
                }
                else
                {
                    looted = true;

                    inventoryController.SetLootID(this);
                    SpawnLoots();
                }
            }
        }

        private void OpenInventoryMenu()
        {
            UIManager.SwitchInventory(true, showLoot: true);
        }

        private void SpawnLoots()
        {
            if (lootSetting.lootGroups.Length > 0)
            {
                foreach (LootGroup lootGroup in lootSetting.lootGroups)
                {
                    foreach (LootDetail lootDetail in lootGroup.loots)
                    {
                        float possibility = Random.Range(0.0f, 1.0f);
                        if (possibility <= lootDetail.possibility)
                        {
                            inventoryController.CreateItemInInventory(lootDetail.itemData, lootDetail.rarity, lootInventory);
                        }
                    }
                }
            }
        }
        
        public void HandleInventoryClose()
        {
            if (GetInstanceID() == inventoryController.lootID)
            {
                InventoryItem[] items = lootInventory.GetComponentsInChildren<InventoryItem>();
                foreach (InventoryItem item in items)
                {
                    lootStorage.Add(new BaseLootData(item));
                }

                inventoryController.ResetLootBox();
            }
        }

        private void RestoreLoots()
        {
            if (lootStorage.Count > 0)
            {
                foreach (BaseLootData item in lootStorage)
                {
                    inventoryController.CreateItemInInventory(item.data, item.rarity, lootInventory, item.bonusAttributes, item.pivotPositionOnGrid);
                }

                lootStorage.Clear();
            }
        }


    }
}