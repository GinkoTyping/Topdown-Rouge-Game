using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerInputEventHandler : MonoBehaviour
{
    public Vector2 Direction { get; private set; }
    public bool PrimaryAttack {  get; private set; }
    public bool Dash { get; private set; }
    public bool Interact { get; private set; }

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
}
