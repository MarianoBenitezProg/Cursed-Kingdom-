using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : MonoBehaviour, ItakeDamage
{
    private int health;
    [SerializeField] private int maxhealth = 200;


    public new List<GameObject> Plataformas;
    public GameObject player;
    public GameObject warningGB;


    public float BasicShootTimer;

    public Vector3 ShootPoint;
    
    public Vector3 originalPos;

    public CapsuleCollider2D colider;



    BossState currentState;

    protected virtual void Awake()
    {
        colider = gameObject.GetComponent<CapsuleCollider2D>();
        SetState(new FaseInicial());
        originalPos = transform.position;
        health = maxhealth;
    }

    public void Update()
    {
        ShootPoint = transform.GetChild(0).transform.position;
        currentState?.UpdateState(this);

        if (health != maxhealth)
        {
            Debug.Log(health);
        }
    }



    public void SetState(BossState newState)
    {

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        checkLifeState();
    }
    public void checkLifeState()
    {
        float healthPercentage = ((float)health / maxhealth) * 100f; // Conversión correcta

        if (healthPercentage > 90)
        {
            SetState(new FaseInicial());
        }
        else if (healthPercentage > 60)
        {
            SetState(new FaseUno());
        }
        else if (healthPercentage > 30)
        {
            SetState(new FaseDos());
        }
        else if (healthPercentage > 0)
        {
            SetState(new FaseFinal());
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}



