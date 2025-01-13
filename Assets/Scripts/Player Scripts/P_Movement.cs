using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class P_Movement
{
    GameObject _aimPosition;
    P_Behaviour _playerRef;
    Transform playerTransform;
    float _speed;
    float _originalSpeed;
    Rigidbody2D _rb;
    


    // Cache the last direction to avoid unnecessary updates
    private Direction _currentDirection = Direction.Right;

    // Enum to track current direction
    

    public P_Movement(P_Behaviour playerRef, float speedRef, Rigidbody2D rbRef, GameObject aimPositionRef)
    {
        _speed = speedRef;
        _originalSpeed = speedRef;
        _playerRef = playerRef;
        _rb = rbRef;
        
        _aimPosition = aimPositionRef;
        playerTransform = _playerRef.transform;
    }

    public void Movement(float hor, float ver, float angle)
    {
        var dir = playerTransform.right * hor;
        dir += playerTransform.up * ver;
        _rb.velocity = dir.normalized * _speed;


        UpdateDirection(angle);
    }

    public void UpdateDirection(float rotationAngle)
    {
        // Determine new direction
        Direction newDirection;
        if (rotationAngle >= 45 && rotationAngle < 135)
        {
            newDirection = Direction.Up;
            _playerRef.lookingDir = Direction.Up;
        }
        else if (rotationAngle >= 135 && rotationAngle < 225)
        {
            newDirection = Direction.Left;
            _playerRef.lookingDir = Direction.Left;
        }
        else if (rotationAngle >= 225 && rotationAngle < 315)
        {
            newDirection = Direction.Down;
            _playerRef.lookingDir = Direction.Down;
        }
        else
        {
            newDirection = Direction.Right;
            _playerRef.lookingDir = Direction.Right;
        }


        // Only update rotation if direction has changed
        if (newDirection != _currentDirection)
        {
            _currentDirection = newDirection;
            Vector3 newScale = playerTransform.localScale;

            switch (newDirection)
            {
                case Direction.Up:
                    Debug.Log("Looking Up");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 90);
                    _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.5f, 0);
                    break;
                case Direction.Left:
                    Debug.Log("Looking Left");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 180);
                    _aimPosition.transform.position = new Vector3(playerTransform.position.x - 1, playerTransform.position.y, 0);
                    break;
                case Direction.Down:
                    Debug.Log("Looking Down");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 270);
                    _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 1.5f, 0);
                    break;
                case Direction.Right:
                    Debug.Log("Looking Right");
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 0);
                    _aimPosition.transform.position = new Vector3(playerTransform.position.x + 1, playerTransform.position.y, 0);
                    break;
            }

            newScale.x = Mathf.Abs(newScale.x);
            playerTransform.localScale = newScale;
        }
    }

    public void ChangeMovement(float newSpeed)
    {
        _speed = newSpeed;
    }
}
