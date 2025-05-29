using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SmoothPlayerModel : MonoBehaviour
{
    private SmoothPlayerController _controller;
    private SmoothPlayerView _view;

    [Header("Inputs")]
    [Range(0.0f, 1.0f)][SerializeField] private float _smoothInputSpeed = 0.2f;

    [Header("Physics")]
    [SerializeField] private float _moveSpeed = 3.5f;

    private Rigidbody _rb;

    private Vector2 _inputDir = new(), _smoothedInputDir = new(), _smoothedInputVelocity = new();
    public Vector2 InputDir 
    { 
        get { return _inputDir; } 
        set { _inputDir = value; }
    }

    private Vector3 _moveDir = new();
    public Vector3 MoveDir
    {
        get { return _moveDir; }
        private set { _moveDir = value; }
    }

    private void Awake()
    {
        SmoothGameManager.Instance.PlayerModel = this;

        _controller = GetComponent<SmoothPlayerController>();
        _view = GetComponentInChildren<SmoothPlayerView>();

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        _smoothedInputDir = Vector2.SmoothDamp(_smoothedInputDir, _inputDir, ref _smoothedInputVelocity, _smoothInputSpeed);

        _view.UpdateMovementAxis(_smoothedInputDir);
    }

    private void FixedUpdate()
    {
        if(_smoothedInputDir.x != 0.0f || _smoothedInputDir.y != 0.0f)
        {
            Movement(_smoothedInputDir);
        }
    }

    private void Movement(Vector2 dir)
    {
        _moveDir = transform.right * dir.x + transform.forward * dir.y;

        _rb.MovePosition(transform.position + _moveDir * _moveSpeed * Time.fixedDeltaTime);
    }
}
