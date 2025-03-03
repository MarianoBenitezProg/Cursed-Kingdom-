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
    bool currentCharacter;
    


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
        currentCharacter = _playerRef.isMarkus;
    }

    public void Movement(float hor, float ver, float angle)
    {
        var dir = playerTransform.right * hor;
        dir += playerTransform.up * ver;
        _rb.velocity = dir.normalized * _speed;


        UpdateDirection(angle);
    }

    public P_Movement SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
        return this;
    }

    public void SetSpeedToCero(object param)
    {
        _originalSpeed = _speed;
        _speed = 0;
    }
    public void ResetSpeed(object param)
    {
        _speed = _originalSpeed;
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
        if (newDirection != _currentDirection || currentCharacter != _playerRef.isMarkus)
        {
            _currentDirection = newDirection;
            currentCharacter = _playerRef.isMarkus;
            Vector3 newScale = playerTransform.localScale;

            switch (newDirection)
            {
                case Direction.Up:
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 90);
                    if (_playerRef.isMarkus == true)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.5f, 0);
                    }
                    else if(_playerRef.isMarkus == false)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 2.2f, 0);
                    }
                    break;
                case Direction.Left:
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 180);
                    if(_playerRef.isMarkus == true)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x - 1, playerTransform.position.y - 0.6f, 0);
                    }
                    else if(_playerRef.isMarkus == false)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x - 1.8f, playerTransform.position.y, 0);
                    }
                    break;
                case Direction.Down:
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 270);
                    if(_playerRef.isMarkus == true)
                    { 
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 1.5f, 0);
                    }
                    else if (_playerRef.isMarkus == false)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 2.2f, 0);
                    }
                    break;
                case Direction.Right:
                    //_playerTransform.rotation = Quaternion.Euler(0, 0, 0);
                    if(_playerRef.isMarkus == true)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x + 1, playerTransform.position.y - 0.6f, 0);
                    }
                    else if(_playerRef.isMarkus == false)
                    {
                        _aimPosition.transform.position = new Vector3(playerTransform.position.x + 1.8f, playerTransform.position.y, 0);
                    }
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
