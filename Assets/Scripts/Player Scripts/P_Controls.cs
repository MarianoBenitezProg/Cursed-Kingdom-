using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controls
{
    P_Movement _movement;
    P_View _view;
    KeyCode switchCharacter = KeyCode.LeftShift;
    KeyCode basicAttack = KeyCode.Mouse0;
    KeyCode damageAbility = KeyCode.Mouse1;
    KeyCode ccAbility = KeyCode.Q;
    KeyCode utilityAbility = KeyCode.R;

    MarkusState _markusState;
    FeranaState _feranaState;

    bool isMarkus = false;

    public P_Controls(P_Movement movementRef, P_View viewRef, MarkusState markusSRef, FeranaState feranaSRef)
    {
        _movement = movementRef;
        _view = viewRef;
        _markusState = markusSRef;
        _feranaState = feranaSRef;
    }

    public void ControlsUpdate()
    {
        BasicControls();
        SwitchControls();
        Ability();
    }

    public void BasicControls()
    {
        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetAxisRaw("Vertical");

        _movement.Movement(hor, ver);
    }
    public void SwitchControls()
    {
        if(Input.GetKeyDown(switchCharacter))
        {
            if(isMarkus == true)
            {
                _markusState.SwitchToFerana();
                isMarkus = false;
                return;
            }

            else if(isMarkus == false)
            {
                _feranaState.SwitchToMarkus();
                isMarkus = true;
                return;
            }
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
}
