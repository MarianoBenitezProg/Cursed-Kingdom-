using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : MonoBehaviour, ItakeDamage
{
    private int health;
    [SerializeField] private int maxhealth = 200;
    public List<GameObject> Plataformas;
    public GameObject player;
    public GameObject warningGB;
    public float BasicShootTimer;
    public Vector3 ShootPoint;
    public Vector3 originalPos;
    public CapsuleCollider2D colider;

    public Animator _animator;
    private BossState currentState;

    // Cache state instances
    private readonly FaseInicial faseInicial = new FaseInicial();
    private readonly FaseUno faseUno = new FaseUno();
    private readonly FaseDos faseDos = new FaseDos();
    private readonly FaseFinal faseFinal = new FaseFinal();

    protected virtual void Awake()
    {
        colider = gameObject.GetComponent<CapsuleCollider2D>();
        originalPos = transform.position;
        health = maxhealth;
        _animator = GetComponent<Animator>();
        SetState(faseInicial);
    }

    public void Update()
    {
        ShootPoint = transform.GetChild(0).transform.position;
        currentState?.UpdateState(this);
    }

    public void SetState(BossState newState)
    {
        if (currentState == newState) return; // Avoid unnecessary state changes

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        CheckLifeState();
    }

    public void CheckLifeState()
    {
        float healthPercentage = ((float)health / maxhealth) * 100f;

        BossState newState;
        if (healthPercentage > 90)
            newState = faseInicial;
        else if (healthPercentage > 60)
            newState = faseUno;
        else if (healthPercentage > 30)
            newState = faseDos;
        else
            newState = faseFinal;

        SetState(newState);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}



