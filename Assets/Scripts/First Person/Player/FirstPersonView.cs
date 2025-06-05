using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FirstPersonView : MonoBehaviour
{
    private Animator _animator;

    [Header("Animator")]
    [SerializeField] private string _groundBoolName = "isGrounded";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _moveFloatName = "moveMagnitude";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetGroundBool(bool state)
    {
        _animator.SetBool(_groundBoolName, state);
    }

    public void CallJumpTrigger()
    {
        _animator.SetTrigger(_jumpTriggerName);
    }

    public void SetMovementValue(float magnitude)
    {
        _animator.SetFloat(_moveFloatName, magnitude);
    }
}
