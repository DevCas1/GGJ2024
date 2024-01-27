using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerTimeHelper : MonoBehaviour
{
    public PlayerInput playerInput;

    public void OnEnable()
    {
        playerInput.actions.Enable();
    }

    public void Update()
    {
        Vector2 input = Gamepad.current.leftStick.ReadValue();
        transform.rotation = Quaternion.LookRotation(new(input.x, 0, input.y));
    }

    public void OnDirection(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        float angle = Mathf.Atan2(input.x, input.y);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}