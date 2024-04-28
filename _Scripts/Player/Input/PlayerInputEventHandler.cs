using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEventHandler : MonoBehaviour
{
    public Vector2 Direction;
    public bool PrimaryAttack;

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
}
