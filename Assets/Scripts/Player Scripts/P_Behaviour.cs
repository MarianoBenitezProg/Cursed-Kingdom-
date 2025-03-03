using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Behaviour : MonoBehaviour, ItakeDamage, ICanPickUp, IStunned
{
    public bool isCinematicPlaying = false;
    Rigidbody2D _rb;
    public int life = 100;
    public int maxLife = 100;
    public Direction lookingDir;
    [SerializeField] float _speed;
    [SerializeField] GameObject _markusSprite;
    [SerializeField] GameObject _feranaSprite;
    [SerializeField] GameObject _aimPosition;
    public bool isMarkus;
    [SerializeField] bool isStunned;

    #region Scripts Variables
    P_Movement _movement;
    P_Controls _controls;
    public P_View _view;
    P_Inventory _inventory;
    MarkusState _markusState;
    FeranaState _feranaState;
    [SerializeField] AbilityTimers _timersScript;
    #endregion

    #region Ability variables

    [Header("Ferana Timers")]
    public float fer_Basic;
    public float fer_Damage;
    public float fer_CC;
    public float fer_Utility;

    [Header("Markus Timers")]
    public float mar_Basic;
    public float mar_Damage;
    public float mar_CC;
    public float mar_Utility;
    #endregion

    #region Power Up Effects
    PowerUps _powerUpScripts;
    [Header("Power Ups Variables")]
    public int buffTime;
    public int addLife;

    #endregion

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
        //RestoreData();

        _inventory = new P_Inventory();
        _rb = GetComponent<Rigidbody2D>();
        #region Scripts Creation
        _view = new P_View(this, _markusSprite, _feranaSprite);
        _markusState = new MarkusState(this, _view, _timersScript);
        _feranaState = new FeranaState(this, _view, _timersScript);
        _fsm = new FSM(_feranaState);
        _movement = new P_Movement(this, _speed, _rb, _aimPosition);
        _controls = new P_Controls(_movement, _view, _markusState, _feranaState, this,_inventory);

        _powerUpScripts = new PowerUps(this, _timersScript);
        _powerUpScripts.SubscribeEffects();//Subscribe all the effects to the Event Manager

        _markusState.AddTransition("SwitchToFerana", _feranaState);
        _feranaState.AddTransition("SwitchToMarkus", _markusState);

        _markusSprite.SetActive(false);
        #endregion

        EventManager.Subscribe(TypeEvent.CinematicOn, StartingCinematic);
        EventManager.Subscribe(TypeEvent.CinematicOff, StopCinematic);
        EventManager.Subscribe(TypeEvent.AddLife, AddLife);

        _inventory.RestoreInventory(); //Sets the inventory == to the saved File
    }
    private void Update()
    {
        _controls.ControlsUpdate();
        _fsm.FsmUpdate(Time.deltaTime);
        _view.FlipRenderer(lookingDir);
    }

    public void TakeDamage(int dmg)
    {
        life -= dmg;
        Debug.Log("Recibi daño");
        _view.TintCharacter(new Color (1, 0, 0, 1f));
        StartCoroutine(Cam_Manager.instance.ShakeCamera());
        EventManager.Trigger(TypeEvent.DamageTaken);

        if(life <= 0)
        {
            LevelsManager.instance.RestartLevel();
        }
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEvent.CinematicOn, StartingCinematic);
        EventManager.Unsubscribe(TypeEvent.CinematicOff, StopCinematic);
        EventManager.Unsubscribe(TypeEvent.AddLife, AddLife);
        _inventory.SaveInventory();
        //SaveData();
        _powerUpScripts.UnsubscribeEffects();//Unsubscribe all the effects to the Event Manager
    }
    
    public void StoreObject(ItemStored pickedObject)
    {
        _inventory.StoreItem(pickedObject);
    }

    public void AddLife(object param)
    {
        life += addLife;
        if(life > maxLife)
        {
            life = maxLife;
        }
        EventManager.Trigger(TypeEvent.HealthUpdate);
    }

    //public void SaveData()
    //{
    //    if (SavedGameManager.instance != null)
    //    {
    //        for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
    //        {
    //            if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
    //            {
    //                SavedGameData UpdateLifeData = SavedGameManager.instance.saveSlots[i]; //You can´t just change the Life from the save slot, you gotta change the whole SaveSlot
    //                UpdateLifeData.life = life;
    //                SavedGameManager.instance.saveSlots[i] = UpdateLifeData;
    //            }
    //        }
    //    }
    //}
    //public void RestoreData()
    //{
    //    if (SavedGameManager.instance != null)
    //    {
    //        for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
    //        {
    //            if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
    //            {
    //                life = SavedGameManager.instance.saveSlots[i].life;
    //            }
    //        }
    //    }
    //}

    #region Efectos de CC
    public void Stunned(float timeToWait, float slowSpeed, bool stunned)
    {
        StartCoroutine(CCEffect(timeToWait, slowSpeed, stunned));
    }

    public IEnumerator CCEffect(float timeToWait, float slowSpeed, bool stunned)
    {
        isStunned = stunned;
        float originalSpeed = _speed;
        _speed = slowSpeed;
        _movement.SetSpeed(_speed);
        yield return new WaitForSeconds(timeToWait);
        _speed = originalSpeed;
        isStunned = false;
        _movement.SetSpeed(_speed);
    }
    #endregion

    #region Cinematic
    public void StartingCinematic(object param)
    {
        isCinematicPlaying = true;
    }
    public void StopCinematic(object param)
    {
        isCinematicPlaying = false;
    }
    #endregion
}
