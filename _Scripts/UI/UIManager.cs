using Ginko.PlayerSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private float menuHoldTime;
    [SerializeField] private AudioClip openMenuAudio;
    [SerializeField] private AudioClip closeMenuAudio;
    [SerializeField] private Light2D globalLight2D;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private RectTransform inventoryContainer;
    [SerializeField] private GameObject lootInventoryGO;

    public event Action onInventoryUIClose;

    private PlayerInputEventHandler playerInputEventHandler;
    private PlayerInput inputAction;

    private float menuOpenTime;

   
    private void Start()
    {
        playerInputEventHandler = Player.Instance.InputHandler;
        inputAction = Player.Instance.InputAction;

        SwitchInventory(false);
    }

    private void Update()
    {
        HandleSwitchInventory();
    }

    public void HandleSwitchInventory()
    {

        if (playerInputEventHandler.PressEsc && inventoryCanvas.activeSelf)
        {
            SwitchInventory(false);
        }
        else if (playerInputEventHandler.SwitchInventory)
        {
            if (inventoryCanvas.activeSelf && Time.time >= menuOpenTime + menuHoldTime)
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
            inventoryCanvas.SetActive(true);

            lootInventoryGO.SetActive((bool)showLoot);

            SetInventoryUI();

            SoundManager.Instance.PlaySound(openMenuAudio);
        }
        else
        {
            onInventoryUIClose?.Invoke();

            playerInputEventHandler.UseEscSignal();
            playerInputEventHandler.UseSwitchInventorySignal();

            inputAction.SwitchCurrentActionMap("Gameplay");
            inventoryCanvas.SetActive(false);

            SoundManager.Instance.PlaySound(closeMenuAudio);
        }
    }

    public void SetInventoryUI()
    {
        Grid[] activeGrids = inventoryContainer.GetComponentsInChildren<Grid>();
        inventoryContainer.sizeDelta = new Vector2(activeGrids.Length == 2 ? 1000 : 1100, Screen.height);
    }

    private void SwitchGameRunning(bool isRunning)
    {
        if (isRunning)
        {
            Time.timeScale = 1f;
            globalLight2D.intensity = 0.55f;
        } else
        {
            Time.timeScale = 0f;
            globalLight2D.intensity = 0.2f;
        }
    }
}
