using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controls
{
    P_Movement _movement;
    P_View _view;
    P_Behaviour _playerScript;
    P_Inventory _inventory;

    Camera _mainCamera;
    KeyCode switchCharacter = KeyCode.LeftShift;
    KeyCode basicAttack = KeyCode.Mouse0;
    KeyCode damageAbility = KeyCode.Mouse1;
    KeyCode ccAbility = KeyCode.Q;
    KeyCode utilityAbility = KeyCode.R;

    KeyCode lifePU = KeyCode.Alpha1;
    KeyCode feranaPU = KeyCode.Alpha2;
    KeyCode markusPU = KeyCode.Alpha3;

    float dirX;
    float dirY;

    MarkusState _markusState;
    FeranaState _feranaState;

    bool isRunning;
    bool isMarkus = false;

    public P_Controls(P_Movement movementRef, P_View viewRef, MarkusState markusSRef, FeranaState feranaSRef, P_Behaviour playerRef, P_Inventory inventoryRef)
    {
        _movement = movementRef;
        _view = viewRef;
        _markusState = markusSRef;
        _feranaState = feranaSRef;
        _playerScript = playerRef;
        _inventory = inventoryRef;

        _mainCamera = Camera.main;
    }

    public void ControlsUpdate()
    {
        BasicControls();
        SwitchControls();
        Ability();
        UseInventory();
    }

    public void BasicControls()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)_playerScript.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;

        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetAxisRaw("Vertical");

        if(hor != 0 || ver != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        _movement.Movement(hor, ver, normalizedAngle);
        LookingDirection(normalizedAngle);
        _view.SetAnimations(isRunning, dirX, dirY);
    }
    public void SwitchControls()
    {
        if(Input.GetKeyDown(switchCharacter))
        {
            if(isMarkus == true)
            {
                _markusState.SwitchToFerana();
                _playerScript.isMarkus = false;
                isMarkus = false;
                return;
            }

            else if(isMarkus == false)
            {
                _feranaState.SwitchToMarkus();
                _playerScript.isMarkus = true;
                isMarkus = true;
                return;
            }
        }
    }

    void LookingDirection(float rotationAngle)
    {
        if (rotationAngle >= 45 && rotationAngle < 135)
        {
            dirX = 0;
            dirY = 1;
        }
        else if (rotationAngle >= 135 && rotationAngle < 225)
        {
            dirX = -1;
            dirY = 0;
        }
        else if (rotationAngle >= 225 && rotationAngle < 315)
        {
            
            dirX = 0;
            dirY = -1;
        }
        else
        {
            dirX = 1;
            dirY = 0;
        }
    }

    public void Ability()
    {
        if(Input.GetKeyDown(basicAttack))
        {
            if(isMarkus == true)
            {
                _markusState.BasicAttack();
            }
            else
            {
                _feranaState.BasicAttack();
            }
        }
        if (Input.GetKeyDown(damageAbility))
        {
            if (isMarkus == true)
            {
                _markusState.DamageAbility();
            }
            else
            {
                _feranaState.DamageAbility();
            }
        }
        if (Input.GetKeyDown(ccAbility))
        {
            if (isMarkus == true)
            {
                _markusState.CCAbility();
            }
            else
            {
                _feranaState.CCAbility();
            }
        }
        if (Input.GetKeyDown(utilityAbility))
        {
            if (isMarkus == true)
            {
                _markusState.UtilityAbility();
            }
            else
            {
                _feranaState.UtilityAbility();
            }
        }
    }

    public void UseInventory()
    {
        if (Input.GetKeyDown(lifePU))
        {
            _inventory.RunEffect(PickUpType.Life);
        }
        if (Input.GetKeyDown(markusPU))
        {
            _inventory.RunEffect(PickUpType.MarkusPU);
        }
        if (Input.GetKeyDown(feranaPU))
        {
            _inventory.RunEffect(PickUpType.FeranaPU);
        }
    }
}
