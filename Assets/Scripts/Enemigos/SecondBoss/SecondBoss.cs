using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour, ItakeDamage
{
    private int health;
    [SerializeField] private int maxhealth = 200;
    [SerializeField] private int damage = 10;

    public float speed = 10f;
    public Vector3 originPoint;


    public CapsuleCollider2D colider;
    public List<GameObject> Points1;
    public List<GameObject> Points2;

    public GameObject toroFake;


    SecondBossState currentState;

    protected virtual void Awake()
    {
        colider = gameObject.GetComponent<CapsuleCollider2D>();
        originPoint = transform.position;
        SetState(new FaseInicial2()); 
        health = maxhealth;
    }

    public void Update()
    {
        currentState?.UpdateState(this);

        if (health != maxhealth)
        {
            Debug.Log(health);
        }
    }


    public void SetState(SecondBossState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType())
        {
            return;
        }

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
        float healthPercentage = ((float)health / maxhealth) * 100f; 

        if (healthPercentage > 90)
        {
            SetState(new FaseInicial2());
        }
        else if (healthPercentage > 60)
        {
            SetState(new FaseUno2());
        }
        else if (healthPercentage > 30)
        {
            SetState(new FaseDos2());
        }
        else if (healthPercentage > 0)
        {
            SetState(new FaseFinal2());
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var playerHealth = collision.GetComponent<P_Behaviour>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); 
            }
        }
    }

}




