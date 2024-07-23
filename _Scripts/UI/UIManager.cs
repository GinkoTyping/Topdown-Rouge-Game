using Ginko.PlayerSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
    [SerializeField]
    private Light2D globalLight2D;

    public event Action onInventoryUIClose;

    private PlayerInputEventHandler playerInputEventHandler;
    private PlayerInput inputAction;

    private float menuOpenTime;
    private RectTransform luggageInventoryUI;
    private RectTransform lootInventoryUI;
    private RectTransform equipmentInventoryUI;

    private void Awake()
    {
        luggageInventoryUI = inventoryUI.transform.Find("Luggage").GetComponent<RectTransform>();
        lootInventoryUI = inventoryUI.transform.Find("Loot").GetComponent<RectTransform>();
        equipmentInventoryUI = inventoryUI.transform.Find("Equipment").GetComponent<RectTransform>();

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

            SwitchGameRunning(false);
        }
        else
        {
            onInventoryUIClose?.Invoke();

            playerInputEventHandler.UseEscSignal();
            playerInputEventHandler.UseSwitchInventorySignal();

            inputAction.SwitchCurrentActionMap("Gameplay");
            inventoryUI.SetActive(false);

            SoundManager.Instance.PlaySound(closeMenuAudio);

            SwitchGameRunning(true);
        }
    }

    public void SetInventoryUI()
    {
        lootInventoryUI.anchoredPosition = new Vector2(Screen.width - lootInventoryUI.sizeDelta.x - border, -border);
        equipmentInventoryUI.anchoredPosition = new Vector2(border, -border);

        // 装备栏和掠夺栏左右布局，物品栏在剩余宽度居中
        float widthLeft = Screen.width - border * 2 - lootInventoryUI.sizeDelta.x - equipmentInventoryUI.sizeDelta.x; 
        luggageInventoryUI.anchoredPosition = new Vector2(widthLeft / 2 - luggageInventoryUI.sizeDelta.x / 2 + equipmentInventoryUI.sizeDelta.x + border, -border);
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
