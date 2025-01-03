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
    [SerializeField]AbilityTimers _timersScript;

    [Header ("Ferana Timers")]
    public float fer_Basic;
    public float fer_Damage;
    public float fer_CC;
    public float fer_Utility;

    [Header ("Markus Timers")]
    public float mar_Basic;
    public float mar_Damage;
    public float mar_CC;
    public float mar_Utility;

    [HideInInspector] public FSM _fsm;

    private void Awake()
    {
        #region Set Timers
        var cooldowns = new Dictionary<string, float>
        {
            {"FeranaBasicAttack", fer_Basic},
            {"FeranaDamage", fer_Damage},
            {"FeranaCC", fer_CC},
            {"FeranaUtility", fer_Utility},

            {"MarkusBasicAttack", mar_Basic},
            {"MarkusDamage", mar_Damage},
            {"MarkusCC", mar_CC},
            {"MarkusUtility", mar_Utility}
        };
        _timersScript.Initialize(cooldowns);

        #endregion


        _rb = GetComponent<Rigidbody2D>();
        #region Scripts Creation
        _view = new P_View(_markusSprite, _feranaSprite);
        _markusState = new MarkusState(this, _view, _timersScript);
        _feranaState = new FeranaState(this, _view, _timersScript);
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
