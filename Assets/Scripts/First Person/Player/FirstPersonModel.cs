using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController), typeof(Rigidbody))]
public class FirstPersonModel : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform _playerHead;
    public Transform PlayerHead { get { return _playerHead; } private set { _playerHead = value; } }
    [Range(10.0f, 1000.0f)][SerializeField] private float _mouseSensitivity = 100.0f;

    [Header("Cursor")]
    [SerializeField] private CursorLockMode _lockMode = CursorLockMode.Locked;
    [SerializeField] private bool _isCursorVisible = false;

    [Header("Inputs")]
    [Range(0.0f, 1.0f)][SerializeField] private float _smoothInputSpeed = 0.2f;

    [Header("Physics")]
    [Range(0.01f, 0.75f)][SerializeField] private float _groundRayDistance = 0.375f;
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private float _jumpForce = 7.0f;
    [SerializeField] private float _moveSpeed = 3.5f;

    private bool _isGrounded = false;
    public bool IsOnGround { get { return _isGrounded; } }
    private float _mouseX = 0.0f;

    private FirstPersonCamera _camera;
    private FirstPersonView _view;
    private Rigidbody _rb;

    private Vector2 _rawInput = new(), _mouseInput = new(), _smoothedInput = new(), _smoothedVelocity = new();
    public Vector2 RawInput { get { return _rawInput; } set { _rawInput = value; } }
    public Vector2 MouseInput { get { return _mouseInput; } set { _mouseInput = value; } }
    private Vector3 _groundPosFix = new(), _moveDir = new();
    public Vector3 MoveDir { get { return _moveDir; } set { _moveDir = value; } }

    private Ray _groundRay;

    private void Awake()
    {
        FirstPersonGameManager.Instance.PlayerModel = this;

        Cursor.lockState = _lockMode;
        Cursor.visible = _isCursorVisible;

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        _camera = Camera.main.GetComponent<FirstPersonCamera>();
        _view = Camera.main.GetComponentInChildren<FirstPersonView>();
    }

    private void Update()
    {
        if(_mouseInput.x != 0.0f || _mouseInput.y != 0.0f)
        {
            Rotate(_mouseInput);
        }

        _smoothedInput = Vector2.SmoothDamp(_smoothedInput, _rawInput, ref _smoothedVelocity, _smoothInputSpeed);

        _view.SetMovementValue(_smoothedInput.magnitude);
        _view.SetGroundBool(_isGrounded);
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();

        if(_smoothedInput.x != 0.0f || _smoothedInput.y != 0.0f)
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

        _view.CallJumpTrigger();
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(Vector2 input)
    {
        _moveDir = transform.right * input.x + transform.forward * input.y;

        _rb.MovePosition(transform.position + _moveDir * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate(Vector2 input)
    {
        _mouseX += input.x * _mouseSensitivity * Time.deltaTime;

        if(_mouseX >= 360.0f || _mouseX <= -360.0f)
        {
            _mouseX -= 360.0f * Mathf.Sign(_mouseX);
        }

        input.y *= _mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0.0f, _mouseX, 0.0f);

        _camera.Rotate(_mouseX, input.y);
    }
}
