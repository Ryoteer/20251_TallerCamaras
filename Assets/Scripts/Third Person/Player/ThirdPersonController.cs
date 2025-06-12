using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ThirdPersonModel))]
public class ThirdPersonController : MonoBehaviour
{
    private SpringArm _springArm;
    private ThirdPersonModel _model;

    private ThirdPersonAction _inputSystem;

    private void Awake()
    {
        _springArm = Camera.main.GetComponentInParent<SpringArm>();
        _model = GetComponent<ThirdPersonModel>();

        _inputSystem = new ThirdPersonAction();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();

        _inputSystem.Player.Jump.performed += JumpInput;
        _inputSystem.Player.Movement.performed += MovementInput;
        _inputSystem.Player.Movement.canceled += MovementCancel;
        _inputSystem.Player.Rotation.performed += RotationInput;
        _inputSystem.Player.Rotation.canceled += RotationCancel;
    }

    private void OnDisable()
    {
        _inputSystem.Disable();

        _inputSystem.Player.Jump.performed -= JumpInput;
        _inputSystem.Player.Movement.performed -= MovementInput;
        _inputSystem.Player.Movement.canceled -= MovementCancel;
        _inputSystem.Player.Rotation.performed -= RotationInput;
        _inputSystem.Player.Rotation.canceled -= RotationCancel;
    }

    private void JumpInput(InputAction.CallbackContext value)
    {
        _model.Jump();
    }

    private void MovementInput(InputAction.CallbackContext value)
    {
        _model.RawInput = value.ReadValue<Vector2>();
    }

    private void MovementCancel(InputAction.CallbackContext value)
    {
        _model.RawInput = Vector2.zero;
    }

    private void RotationInput(InputAction.CallbackContext value)
    {
        _springArm.MouseInput = value.ReadValue<Vector2>();
    }

    private void RotationCancel(InputAction.CallbackContext value)
    {
        _springArm.MouseInput = Vector2.zero;
    }
}
