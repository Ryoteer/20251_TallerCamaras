using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    [Header("Camera")]
    [Range(10.0f, 5000.0f)][SerializeField] private float _mouseSensitivity = 500.0f;
    [Range(0.125f, 1.0f)][SerializeField] private float _minDistance = 0.25f;
    [Range(1.0f, 10.0f)][SerializeField] private float _maxDistance = 3.0f;
    [Range(-90.0f, 0.0f)][SerializeField] private float _minRotation = -80.0f;
    [Range(0.0f, 90.0f)][SerializeField] private float _maxRotation = 80.0f;

    [Header("Cursor")]
    [SerializeField] private CursorLockMode _lockState = CursorLockMode.Locked;
    [SerializeField] private bool _isCursorVisible = false;

    [Header("Physics")]
    [Range(0.05f, 1.0f)][SerializeField] private float _detectRadius = 0.1f;
    [SerializeField] private float _hitOffset = 0.25f;

    private bool _isBlocked = false;
    private float _mouseX = 0.0f, _mouseY = 0.0f;

    private Vector2 _mouseInput = new();
    public Vector2 MouseInput { get { return _mouseInput; } set { _mouseInput = value; } }
    private Vector3 _dir = new(), _dirTest = new(), _cameraPos = new();

    private Camera _camera;
    private Transform _target;

    private Ray _cameraRay;
    private RaycastHit _cameraHit;

    private void Start()
    {
        _target = ThirdPersonGameManager.Instance.PlayerModel.HeadTransform;

        _camera = Camera.main;

        Cursor.lockState = _lockState;
        Cursor.visible = _isCursorVisible;

        transform.forward = _target.forward;

        _mouseX = transform.eulerAngles.y;
        _mouseY = transform.eulerAngles.x;
    }

    private void FixedUpdate()
    {
        _cameraRay = new Ray(transform.position, _dir);

        _isBlocked = Physics.SphereCast(_cameraRay, _detectRadius, out _cameraHit, _maxDistance);
    }

    private void LateUpdate()
    {
        UpdateCameraRotation(_mouseInput);
        UpdateCameraPosition();
    }

    private void UpdateCameraRotation(Vector2 input)
    {
        transform.position = _target.position;

        if (input.x == 0.0f && input.y == 0.0f) return;

        if(input.x != 0.0f)
        {
            _mouseX += input.x * _mouseSensitivity * Time.deltaTime;

            if(_mouseX > 360.0f || _mouseX < -360.0f)
            {
                _mouseX -= 360.0f * Mathf.Sign(_mouseX);
            }
        }

        if(input.y != 0.0f)
        {
            _mouseY += input.y * _mouseSensitivity * Time.deltaTime;

            _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);
        }

        transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0.0f);
    }

    private void UpdateCameraPosition()
    {
        _dir = -transform.forward;

        if (_isBlocked)
        {
            _dirTest = (_cameraHit.point - transform.position) + (_cameraHit.normal * _hitOffset);

            if(_dirTest.sqrMagnitude <= _minDistance * _minDistance)
            {
                _cameraPos = transform.position + _dir * _minDistance;
            }
            else
            {
                _cameraPos = transform.position + _dirTest;
            }
        }
        else
        {
            _cameraPos = transform.position + _dir * _maxDistance;
        }

        _camera.transform.position = _cameraPos;
        _camera.transform.LookAt(transform.position);
    }
}
