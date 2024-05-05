using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerInputEventHandler : MonoBehaviour
{
    public Vector2 Direction;
    public bool PrimaryAttack;
    public bool Dash;
    public bool Interact;

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
}
