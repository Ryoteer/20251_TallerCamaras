using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera")]
    [Range(-90.0f, 0.0f)][SerializeField] private float _minRotation = -80.0f;
    [Range(0.0f, 90.0f)][SerializeField] private float _maxRotation = 80.0f;

    [Header("Headbob")]
    [Range(0.0f, 0.2f)][SerializeField] private float _headbobAmount = 0.075f;
    [Range(1.0f, 50.0f)][SerializeField] private float _headbobSpeed = 12.5f;

    private float _headbobTimer = 0.0f, _mouseY = 0.0f;

    private FirstPersonModel _model;
    private Transform _headTransform;

    private void Start()
    {
        _model = FirstPersonGameManager.Instance.PlayerModel;
        _headTransform = _model.PlayerHead;

        transform.position = _headTransform.position;
    }

    private void LateUpdate()
    {
        Headbob();
    }

    private void Headbob()
    {
        if(Mathf.Abs(_model.MoveDir.x) > 0.0f || Mathf.Abs(_model.MoveDir.y) > 0.0f)
        {
            _headbobTimer += _headbobSpeed * Time.deltaTime;

            if (!_model.IsOnGround)
            {
                transform.position = _headTransform.position;
            }
            else
            {
                transform.position = new Vector3
                (
                    _headTransform.position.x,
                    _headTransform.position.y + (Mathf.Sin(_headbobTimer) * _headbobAmount * _model.MoveDir.magnitude),
                    _headTransform.position.z
                );
            }
        }
    }

    public void Rotate(float x, float y)
    {
        _mouseY += y;
        _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);

        transform.rotation = Quaternion.Euler(-_mouseY, x, 0.0f);
    }
}
