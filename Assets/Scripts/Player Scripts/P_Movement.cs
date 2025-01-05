using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Movement
{
    GameObject _aimPosition;
    Transform _playerTransform;
    float _speed;
    float _originalSpeed;
    Rigidbody2D _rb;
    Camera _mainCamera;

    // Cache the last direction to avoid unnecessary updates
    private Direction _currentDirection = Direction.Right;

    // Enum to track current direction
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public P_Movement(Transform playerRef, float speedRef, Rigidbody2D rbRef, GameObject aimPositionRef)
    {
        _speed = speedRef;
        _originalSpeed = speedRef;
        _playerTransform = playerRef;
        _rb = rbRef;
        _mainCamera = Camera.main;
        _aimPosition = aimPositionRef;
    }

    public void Movement(float hor, float ver)
    {
        var dir = _playerTransform.right * hor;
        dir += _playerTransform.up * ver;
        _rb.velocity = dir.normalized * _speed;

        UpdateDirection();
    }

    public void UpdateDirection()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)_playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;

        // Determine new direction
        Direction newDirection;
        if (normalizedAngle >= 45 && normalizedAngle < 135)
            newDirection = Direction.Up;
        else if (normalizedAngle >= 135 && normalizedAngle < 225)
            newDirection = Direction.Left;
        else if (normalizedAngle >= 225 && normalizedAngle < 315)
            newDirection = Direction.Down;
        else
            newDirection = Direction.Right;

        // Only update rotation if direction has changed
        if (newDirection != _currentDirection)
        {
            _currentDirection = newDirection;
            Vector3 newScale = _playerTransform.localScale;

            switch (newDirection)
            {
                case Direction.Up:
                    Debug.Log("Looking Up");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 90);
                    _aimPosition.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1.5f, 0);
                    break;
                case Direction.Left:
                    Debug.Log("Looking Left");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 180);
                    _aimPosition.transform.position = new Vector3(_playerTransform.position.x - 1, _playerTransform.position.y, 0);
                    break;
                case Direction.Down:
                    Debug.Log("Looking Down");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 270);
                    _aimPosition.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y - 1.5f, 0);
                    break;
                case Direction.Right:
                    Debug.Log("Looking Right");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 0);
                    _aimPosition.transform.position = new Vector3(_playerTransform.position.x + 1, _playerTransform.position.y, 0);
                    break;
            }

            newScale.x = Mathf.Abs(newScale.x);
            _playerTransform.localScale = newScale;
        }
    }

    public void ChangeMovement(float newSpeed)
    {
        _speed = newSpeed;
    }
}
