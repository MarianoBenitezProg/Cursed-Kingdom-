using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controls
{
    P_Movement _movement;
    P_View _view;
    KeyCode switchCharacter = KeyCode.LeftShift;

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
                _view.SwitchToFerana();
                isMarkus = false;
                Debug.Log("Entro To Ferana");
                return;
            }

            else if(isMarkus == false)
            {
                _feranaState.SwitchToMarkus();
                _view.SwitchToMarkus();
                isMarkus = true;
                Debug.Log("Entro To Markus");
                return;
            }
        }
    }
}
