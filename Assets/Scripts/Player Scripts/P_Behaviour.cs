using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Behaviour : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] GameObject _markusSprite;
    [SerializeField] GameObject _feranaSprite;

    P_Movement _movement;
    P_Controls _controls;
    P_View _view;

    MarkusState _markusState;
    FeranaState _feranaState;

    [HideInInspector] public FSM _fsm;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        #region Scripts Creation

        _view = new P_View(_markusSprite, _feranaSprite);
        _markusState = new MarkusState(this, _view);
        _feranaState = new FeranaState(this, _view);
        _fsm = new FSM(_feranaState);
        _movement = new P_Movement(this.transform, _speed, _rb);
        _controls = new P_Controls(_movement, _view, _markusState, _feranaState);
        _markusState.AddTransition("SwitchToFerana", _feranaState);
        _feranaState.AddTransition("SwitchToMarkus", _markusState);

        #endregion

    }
    private void Update()
    {
        _controls.ControlsUpdate();
        _fsm.FsmUpdate(Time.deltaTime);
    }
}
