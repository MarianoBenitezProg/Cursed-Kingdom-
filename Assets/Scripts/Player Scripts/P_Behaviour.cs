using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Behaviour : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] GameObject _markusSprite;
    [SerializeField] GameObject _feranaSprite;

    P_Movement _movement;
    P_Controls _controls;
    P_View _view;

    MarkusState _markusState;
    FeranaState _feranaState;
    public FSM _fsm;

    private void Awake()
    {
        
        #region States
        _markusState = new MarkusState(this);
        _feranaState = new FeranaState(this);
        _fsm = new FSM(_feranaState);

        _markusState.AddTransition("SwitchToFerana", _feranaState);
        _feranaState.AddTransition("SwitchToMarkus", _markusState);
        #endregion

        #region MVC
        _movement = new P_Movement(this.transform, _speed);
        _view = new P_View(_markusSprite, _feranaSprite);
        _controls = new P_Controls(_movement, _view, _markusState, _feranaState);
        #endregion

    }
    private void Update()
    {
        _controls.ControlsUpdate();
        _fsm.FsmUpdate(Time.deltaTime);
    }
}
