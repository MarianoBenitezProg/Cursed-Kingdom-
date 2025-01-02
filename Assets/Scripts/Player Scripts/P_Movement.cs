using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Movement
{
    Transform _playerTransform;
    float _speed;
    float _originalSpeed;
    public P_Movement(Transform playerRef, float speedRef)
    {
        _speed = speedRef;
        _originalSpeed = speedRef;
        _playerTransform = playerRef;
    }

    public void Movement(float hor, float ver)
    {
        var dir = _playerTransform.right * hor;
        dir += _playerTransform.up * ver;

        _playerTransform.position += dir.normalized * _speed * Time.deltaTime;
    }

    public void ChangeMovement(float newSpeed)
    {
        _speed = newSpeed;
    }
}
