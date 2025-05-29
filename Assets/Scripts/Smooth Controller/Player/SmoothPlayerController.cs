using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothPlayerController : MonoBehaviour
{
    private SmoothPlayerModel _model;

    private MyInputSystem _inputSystem;

    private void Awake()
    {
        _model = GetComponent<SmoothPlayerModel>();

        _inputSystem = new MyInputSystem();
    }

    #region Enabling
    private void OnEnable()
    {
        _inputSystem.Enable();

        _inputSystem.Player.Move.performed += MovementInput;
        _inputSystem.Player.Move.canceled += MovementCancel;
    }

    private void OnDisable()
    {
        _inputSystem.Disable();

        _inputSystem.Player.Move.performed -= MovementInput;
        _inputSystem.Player.Move.canceled -= MovementCancel;
    }
    #endregion

    #region Movement Inputs
    private void MovementInput(InputAction.CallbackContext value)
    {
        _model.InputDir = value.ReadValue<Vector2>();
    }

    private void MovementCancel(InputAction.CallbackContext value)
    {
        _model.InputDir = Vector2.zero;
    }
    #endregion
}
