using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SmoothPlayerView : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private string _xFloatName = "xAxis";
    [SerializeField] private string _yFloatName = "yAxis";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateMovementAxis(Vector2 input)
    {
        _animator.SetFloat(_xFloatName, input.x);
        _animator.SetFloat(_yFloatName, input.y);
    }
}
