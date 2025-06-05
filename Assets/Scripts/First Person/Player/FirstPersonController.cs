using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    private FirstPersonModel _model;

    private FirstPersonAction _inputAction;

    private void Awake()
    {
        _model = GetComponent<FirstPersonModel>();

        _inputAction = new FirstPersonAction();
    }

    private void OnEnable()
    {
        _inputAction.Enable();

        _inputAction.Player.Jump.performed += JumpAction;
        _inputAction.Player.Movement.performed += MovementAction;
        _inputAction.Player.Movement.canceled += MovementCancel;
        _inputAction.Player.Rotation.performed += RotationAction;
        _inputAction.Player.Rotation.canceled += RotationCancel;
    }

    private void OnDisable()
    {
        _inputAction.Disable();

        _inputAction.Player.Jump.performed -= JumpAction;
        _inputAction.Player.Movement.performed -= MovementAction;
        _inputAction.Player.Movement.canceled -= MovementCancel;
        _inputAction.Player.Rotation.performed -= RotationAction;
        _inputAction.Player.Rotation.canceled -= RotationCancel;
    }

    private void JumpAction(InputAction.CallbackContext value)
    {
        _model.Jump();
    }

    private void MovementAction(InputAction.CallbackContext value)
    {
        _model.RawInput = value.ReadValue<Vector2>();
    }

    private void MovementCancel(InputAction.CallbackContext value)
    {
        _model.RawInput = Vector2.zero;
    }

    private void RotationAction(InputAction.CallbackContext value)
    {
        _model.MouseInput = value.ReadValue<Vector2>();
    }

    private void RotationCancel(InputAction.CallbackContext value)
    {
        _model.MouseInput = Vector2.zero;
    }
}
