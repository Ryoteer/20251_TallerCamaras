using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ThirdPersonView : MonoBehaviour
{
    private Animator _animator;

    [Header("Animator")]
    [SerializeField] private string _groundBoolName = "isGrounded";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _xFloatName = "xAxis";
    [SerializeField] private string _yFloatName = "yAxis";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetGroundBool(bool state)
    {
        _animator.SetBool(_groundBoolName, state);
    }

    public void SetJumpTrigger()
    {
        _animator.SetTrigger(_jumpTriggerName);
    }

    public void SetMoveValue(Vector2 input)
    {
        _animator.SetFloat(_xFloatName, input.x);
        _animator.SetFloat(_yFloatName, input.y);
    }
}
