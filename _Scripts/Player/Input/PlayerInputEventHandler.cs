using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerInputEventHandler : MonoBehaviour
{
    public Vector2 MousePosition { get; private set; }
    public Vector2 AimPosition { get; private set; }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        MousePosition = Camera.main.ScreenToWorldPoint((Vector3)context.ReadValue<Vector2>());
        AimPosition = MousePosition - (Vector2)transform.position;
    }

    #region GamePlay
    public Vector2 Direction { get; private set; }
    public bool PrimaryAttack {  get; private set; }
    public bool Dash { get; private set; }
    public bool Interact { get; private set; }
    public bool Switch { get; private set; }
    public bool ConsumePotion { get; private set; }


    public void OnMovement(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
    }
    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PrimaryAttack = true;
        }
        else if (context.performed)
        {
            PrimaryAttack = true;
        }
        else if (context.canceled)
        {
            PrimaryAttack = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Dash = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            Dash = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Interact = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            Interact = false;
        }
    }

    public void UseInteractSignal()
    {
        if (Interact)
        {
            Interact = false;
        }
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Switch = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            Switch = false;
        }
    }

    public void UseSwitchSignal()
    {
        if (Switch)
        {
            Switch = false;
        }
    }

    public void OnConsumePotion(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ConsumePotion = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            ConsumePotion = false;
        }
    }

    public void UseConsumePotionSignal()
    {
        if (ConsumePotion)
        {
            ConsumePotion = false;
        }
    }

    #endregion

    #region UI
    public bool Select { get; private set; }
    public bool DeSelect { get; private set; }
    public bool Test { get; private set; }
    public bool RotateItem { get; private set; }
    public bool PressEsc { get; private set; }
    public bool HoldCombineKey { get; private set; }

    public bool SwitchInventory { get; private set; }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Select = true;

        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            Select = false;
        }
    }

    public void useSelectSignal()
    {
        Select = false;
    }

    public void OnDeSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DeSelect = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            DeSelect = false;
        }
    }
    public void useDeSelectSignal()
    {
        DeSelect = false;
    }

    public void OnTest(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Test = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            Test = false;
        }
    }
    public void useTestSignal()
    {
        Test = false;
    }

    public void OnRotateItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RotateItem = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            RotateItem = false;
        }
    }

    public void UseRotateItemSignal()
    {
        RotateItem = false;
    }

    public void OnPressEsc(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PressEsc = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            PressEsc = false;
        }
    }

    public void UseEscSignal()
    {
        PressEsc = false;
    }

    public void OnSwitchInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SwitchInventory = true;
        }
        else if (context.performed)
        {

        }
        else if (context.canceled)
        {
            SwitchInventory = false;
        }
    }
    public void UseSwitchInventorySignal()
    {
        SwitchInventory = false;
    }

    public void OnHoldCombineKey(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
        else if (context.performed)
        {
            HoldCombineKey = true;
        }
        else if (context.canceled)
        {
            HoldCombineKey = false;
        }
    }
    #endregion
}
