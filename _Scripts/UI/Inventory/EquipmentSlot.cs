using Ginko.PlayerSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private EquipmentType type;
    [SerializeField]
    public int tileSize;

    private bool isEmpty;
    private bool isActive;

    private InventoryItem currentEquipment;
    private InventoryController inventoryController;
    private Grid backpackInventory;
    private PlayerInputEventHandler inputHandler;
    private Image backgroundImage;
    private Color defaultBackgroundColor;

    public event Action OnEquip;
    public event Action OnUnequip;

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
        backgroundImage = GetComponent<Image>();
        backpackInventory = inventoryController.transform.Find("Backpack").GetComponentInChildren<Grid>();
        defaultBackgroundColor = backgroundImage.color;
    }

    private void Start()
    {
        inputHandler = Player.Instance.InputHandler;
    }

    private void Update()
    {
        HandleEquipItem();
        HandleUnequipItem();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isActive = true;
        SwitchHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentEquipment == null && isActive)
        {
            SwitchHighlight(false);
        }

        isActive = false;
    }

    private void SwitchHighlight(bool isHighlight)
    {
        if (isHighlight)
        {
            backgroundImage.color = new Color(255,255,255, 0);
        } else
        {
            backgroundImage.color = defaultBackgroundColor;
        }
    }

    private void HandleEquipItem()
    {

        if (isActive && 
            inventoryController.selectedItem != null && 
            inputHandler.Select
            )
        {
            inputHandler.useSelectSignal();


            EquipmentItemSO data = (EquipmentItemSO)inventoryController.selectedItem.data;

            if (data.equipmentType == type)
            {
                if (currentEquipment == null)
                {
                    currentEquipment = inventoryController.EquipSelectedItem(this);
                } else
                {
                    // TODO: 切换装备
                }
            }
        }
    }

    private void HandleUnequipItem()
    {
        if (isActive && 
            inventoryController.selectedItem == null &&
            currentEquipment != null &&
            inputHandler.Select
            )
        {
            // TODO: 还有默认是仓库的情况，待补充
            inventoryController.SetSelectedInventory(backpackInventory);
            Debug.Log(inventoryController.selectedInventory);
            inventoryController.CreateItemOnMouse(currentEquipment.data, currentEquipment.rarity);

            Destroy(currentEquipment.gameObject);
            currentEquipment = null;
        }
    }
}
