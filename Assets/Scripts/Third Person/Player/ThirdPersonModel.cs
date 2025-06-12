using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ThirdPersonController))]
public class ThirdPersonModel : MonoBehaviour
{
    private ThirdPersonController _controller;
    private ThirdPersonView _view;

    [Header("Camera")]
    [SerializeField] private Transform _headTransform;
    public Transform HeadTransform { get { return _headTransform; } }

    [Header("Inputs")]
    [Range(0.0f, 1.0f)][SerializeField] private float _smoothInputSpeed = 0.2f;

    [Header("Physics")]
    [Range(0.125f, 0.75f)][SerializeField] private float _groundRayDistance = 0.2f;
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private float _jumpForce = 6.25f;
    [SerializeField] private float _moveSpeed = 3.5f;

    private bool _isGrounded = false;

    private Vector2 _rawInput = new(), _smoothedInput = new(), _smoothedVelocity = new();
    public Vector2 RawInput { get { return _rawInput; }  set { _rawInput = value; } }
    private Vector3 _groundPosFix = new(), _moveDir = new(), _dirFix = new(), _camForwardFix = new(), _camRightFix = new();
    public Vector3 MoveDir { get { return _moveDir; } }

    private Rigidbody _rb;
    private SpringArm _springArm;
    private Transform _camTransform;

    private Ray _groundRay;

    private void Awake()
    {
        ThirdPersonGameManager.Instance.PlayerModel = this;

        _controller = GetComponent<ThirdPersonController>();

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        _view = GetComponentInChildren<ThirdPersonView>();

        _springArm = Camera.main.GetComponentInParent<SpringArm>();
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        _smoothedInput = Vector2.SmoothDamp(_smoothedInput, _rawInput, ref _smoothedVelocity, _smoothInputSpeed);

        _view.SetMoveValue(_smoothedInput);
        _view.SetGroundBool(_isGrounded);
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();

        if(_rawInput.x != 0.0f || _rawInput.y != 0.0f)
        {
            Movement(_smoothedInput);
        }
    }

    private bool IsGrounded()
    {
        _groundPosFix = new Vector3
                        (
                            transform.position.x,
                            transform.position.y + 0.1f,
                            transform.position.z
                        );

        _groundRay = new Ray(_groundPosFix, -transform.up);

        return Physics.Raycast(_groundRay, _groundRayDistance, _groundRayMask);
    }

    public void Jump()
    {
        if (!_isGrounded) return;

        _view.SetJumpTrigger();
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(Vector2 input)
    {
        _camForwardFix = _camTransform.forward;
        _camRightFix = _camTransform.right;

        _camForwardFix.y = 0.0f;
        _camRightFix.y = 0.0f;        

        Rotate(_camForwardFix);

        _camForwardFix.Normalize();
        _camRightFix.Normalize();

        _dirFix = _camRightFix * input.x + _camForwardFix * input.y;

        _rb.MovePosition(_rb.position + _dirFix * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate(Vector3 dir)
    {
        transform.forward = dir;
    }
}
