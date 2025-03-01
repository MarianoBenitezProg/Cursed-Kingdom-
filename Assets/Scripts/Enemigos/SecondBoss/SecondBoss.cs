using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour, ItakeDamage
{
    public Vector3 originPoint;

    private int health;
    [SerializeField] private int maxhealth = 200;
    public int damage = 10;

    public float speed = 10f;
    public bool needToStop = false;
    public float stopTimer = 0f;
    public float stopTime = 2f;
    public GameObject player;
    public GameObject atack;
    public Vector3 dir;

    public float viewRadius = 10f;
    public float playerDistance;

    public float atackTimer;
    public float atackCountDown = 2f;


    private BoxCollider2D colider;
    private Rigidbody2D rb;
    private SecondBossState currentState;
    public LayerMask playerLayer;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> enemigsToSpawn = new List<GameObject>();
    public List<GameObject> paths = new List<GameObject>();

    protected virtual void Awake()
    {
        colider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originPoint = transform.position;
        SetState(new FaseInicial2());
        health = maxhealth;
    }

    private void Update()
    {
        currentState?.UpdateState(this);
        HasAnEnemy();

        if (health != maxhealth)
        {
            Debug.Log(health);
        }

        if (player != null)
        {
            Move();
        }

    }

    public void SetState(SecondBossState newState)
    {
        if (currentState == null || currentState.GetType() != newState.GetType())
        {
            currentState?.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        CheckLifeState();
    }

    private void CheckLifeState()
    {
        float healthPercentage = ((float)health / maxhealth) * 100f;
        SecondBossState newState = currentState;

        if (healthPercentage > 90)
            newState = new FaseInicial2();
        else if (healthPercentage > 60)
            newState = new FaseUno2();
        else if (healthPercentage > 30)
            newState = new FaseDos2();
        else if (healthPercentage > 0)
            newState = new FaseFinal2();

        if (newState != currentState)
        {
            SetState(newState);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void HasAnEnemy()
    {
        Collider2D[] playersInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        if (playersInRadius.Length > 0)
        {
            player = playersInRadius[0].gameObject;
            playerDistance = Vector3.Distance(player.transform.position, transform.position);
        }
        else
        {
            player = null;
        }
    }

    private void Move()
    {
        if (player == null) return;

        if (needToStop == false)
        {
            colider.enabled = true;


            if (Vector3.Distance(player.transform.position, transform.position) > 5)
            {
                dir = (player.transform.position - transform.position).normalized;
                rb.velocity = dir * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;

                atackTimer += Time.deltaTime;
                if(atackTimer >= atackCountDown)
                {
                var proyetile = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
                var toroProyec = proyetile.GetComponent<ToroProyec>();
                toroProyec.dmg = damage;
                proyetile.transform.position = transform.position + dir * 5;
                    atackTimer = 0;
                }
            }



        }
        else
        {
            colider.enabled = false;

            rb.velocity = Vector2.zero;
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopTime)
            {
                needToStop = false;
                stopTimer = 0;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            needToStop = true;
            var playerHealth = collision.GetComponent<P_Behaviour>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player != null)
            {

            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
