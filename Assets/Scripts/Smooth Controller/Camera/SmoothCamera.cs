using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Camera")]
    [Range(0.01f, 0.125f)][SerializeField] private float _smoothSpeed = 0.075f;
    [Range(0.0f, 1.0f)][SerializeField] private float _maxDistance = 0.125f;

    private SmoothPlayerModel _playerModel;

    private Vector3 _offset = new(), _desierdPos = new(), _expandedPos = new(), _smoothedPos = new();

    private void Start()
    {
        _offset = transform.position;

        _playerModel = SmoothGameManager.Instance.PlayerModel;
    }

    private void FixedUpdate()
    {
        _desierdPos = _playerModel.transform.position + _offset;

        _expandedPos = transform.position + _playerModel.MoveDir * _maxDistance;

        _smoothedPos = Vector3.Lerp(_expandedPos, _desierdPos, _smoothSpeed);

        transform.position = _smoothedPos;
    }
}
