using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Movement
{
    Transform _playerTransform;
    float _speed;
    float _originalSpeed;
    Rigidbody2D _rb;
    public P_Movement(Transform playerRef, float speedRef, Rigidbody2D rbRef)
    {
        _speed = speedRef;
        _originalSpeed = speedRef;
        _playerTransform = playerRef;
        _rb = rbRef;
    }

    public void Movement(float hor, float ver)
    {
        var dir = _playerTransform.right * hor;
        dir += _playerTransform.up * ver;
        _rb.velocity = dir.normalized * _speed;
    }

    public void ChangeMovement(float newSpeed)
    {
        _speed = newSpeed;
    }
}
