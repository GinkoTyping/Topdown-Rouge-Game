using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private float border;
    [Header("Inventory")]
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private float menuHoldTime;
    [SerializeField]
    private AudioClip openMenuAudio;
    [SerializeField]
    private AudioClip closeMenuAudio;

    public event Action onInventoryUIClose;

    private PlayerInputEventHandler playerInputEventHandler;
    private PlayerInput inputAction;

    private float menuOpenTime;
    private RectTransform BackpackInventoryUI;
    private RectTransform LootInventoryUI;

    private void Awake()
    {
        BackpackInventoryUI = inventoryUI.transform.Find("Backpack").GetComponent<RectTransform>();
        LootInventoryUI = inventoryUI.transform.Find("Loot").GetComponent<RectTransform>();

    }
    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        inputAction = Player.Instance.InputAction;
        
        SetInventoryUI();
    }

    private void Update()
    {
        HandleSwitchInventory();
    }

    public void HandleSwitchInventory()
    {

        if (playerInputEventHandler.PressEsc && inventoryUI.activeSelf)
        {
            SwitchInventory(false);
        }
        else if (playerInputEventHandler.SwitchInventory)
        {
            if (inventoryUI.activeSelf && Time.time >= menuOpenTime + menuHoldTime)
            {
                SwitchInventory(false);
            }
            else
            {
                menuOpenTime = Time.time;
                SwitchInventory(true);
            }
        }
    }

    public void SwitchInventory(bool isOpen, bool? showLoot = false)
    {
        if (isOpen)
        {
            playerInputEventHandler.UseSwitchInventorySignal();

            inputAction.SwitchCurrentActionMap("UI");
            inventoryUI.SetActive(true);

            inventoryUI.transform.Find("Loot").gameObject.SetActive((bool)showLoot);

            SoundManager.Instance.PlaySound(openMenuAudio);
        } else
        {
            onInventoryUIClose?.Invoke();

            playerInputEventHandler.UseEscSignal();
            playerInputEventHandler.UseSwitchInventorySignal();

            inputAction.SwitchCurrentActionMap("Gameplay");
            inventoryUI.SetActive(false);

            SoundManager.Instance.PlaySound(closeMenuAudio);
        }
    }

    public void SetInventoryUI()
    {
        BackpackInventoryUI.anchoredPosition = new Vector2(Screen.width / 2 - BackpackInventoryUI.sizeDelta.x / 2, -border);
        LootInventoryUI.anchoredPosition = new Vector2(Screen.width - LootInventoryUI.sizeDelta.x - border, -border);
    }
}
