using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class LootsRespawning : CoreComponent
    {
        [SerializeField]
        public LootListDataSO lootSetting;

        private LoopBar loopBar;
        private Grid lootInventory;
        private InventoryController inventoryController;
        private Interactable interactable;
        private GameObject inventoryUI;
        private UIManager UIManager;

        protected override void Awake()
        {
            base.Awake();

            UIManager = GameObject.Find("UI").GetComponent<UIManager>();
            lootInventory = GameObject.Find("Loot").GetComponentInChildren<Grid>();
            inventoryUI = GameObject.Find("Inventory");
            inventoryController = inventoryUI.GetComponent<InventoryController>();
            interactable = Core.GetCoreComponent<Interactable>();
        }

        public void OnLoots()
        {
            if (interactable.interactType == InteractType.Lootable)
            {
                OpenInventoryMenu();
                SpawnLoots();
            }
        }

        private void OpenInventoryMenu()
        {
            UIManager.SwitchInventory(true, showLoot:true);
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
                            InventoryItem item =  inventoryController.CreateItemInInventory(lootDetail.itemData, lootDetail.rarity, lootInventory);
                            Vector2Int? pos = lootInventory.GetSpaceForItem(item);
                            if (pos != null)
                            {
                                lootInventory.PlaceItem(item, pos.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}