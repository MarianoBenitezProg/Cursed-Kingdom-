using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Transform> path = new List<Transform>();
    public int health = 100;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public bool isStunned;

    public float viewAngle = 45f;
    public float viewRadius = 10f;
    public float attackRadius = 5f;

    public float lostTimer;


    public float playerDist;

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public GameObject player;

    [SerializeField] public bool needToAtack = false;
    [SerializeField] public bool needToSeek = false;

    float OutOfSigth;
    IEnemyState currentState;

    protected virtual void Awake()
    {
        SetState(new PatrolState());
    }

    public void Update()
    {
        currentState?.UpdateState(this);

        Vector3 position = transform.position;
        position.z = 0;
        transform.position = position;

        if (health <= 0)
        {
            Die();
        }

        CanSeeTarget();

        if (needToAtack && player != null)
        {
            SetState(new AtackState());
        }
        else if (needToSeek && player != null)
        {
            SetState(new SeekState());
        }
    }


    #region Métodos básicos
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        Debug.Log("Ejecutando ataque base");
    }

    public virtual void Seek()
    {
        Debug.Log("Ejecutando ataque base");
    }
    public void SetState(IEnemyState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType())
        {
            return;
        }

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    #endregion



    #region Field Of View 
    public void CanSeeTarget()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        // Detectar al jugador dentro del rango
        foreach (Collider2D col in targetsInViewRadius)
        {
            if (((1 << col.gameObject.layer) & playerLayer) != 0)
            {
                player = col.gameObject;
                break;
            }
        }

        if (player != null)
        {
            playerDist = Vector2.Distance(transform.position, player.transform.position);

            if (playerDist <= attackRadius)
            {
                needToAtack = true;
                needToSeek = false;
                lostTimer = 0; // Reiniciar el temporizador
                Debug.Log("Jugador está en radio de ataque");
            }
            else if (playerDist <= viewRadius && playerDist > attackRadius)
            {
                needToSeek = true;
                needToAtack = false;
                lostTimer = 0; // Reiniciar el temporizador
                Debug.Log("Jugador está en radio de Seek");
            }
            else
            {
                lostTimer += Time.deltaTime;

                // Si se pierde al jugador por mucho tiempo, volver a patrulla
                if (lostTimer > 4)
                {
                    needToSeek = false;
                    needToAtack = false;
                    player = null;
                    SetState(new PatrolState());
                    Debug.Log("Perdí al jugador, vuelvo a patrullar");
                    lostTimer = 0;
                }
            }
        }
    }

    #endregion



    #region OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    #endregion



    #region Efectos de CC
    public void Stunned(float timeToWait, float slowSpeed, bool stunned)
    {
        StartCoroutine(CCEffect(timeToWait, slowSpeed, stunned));
    }

    public IEnumerator CCEffect(float timeToWait, float slowSpeed, bool stunned)
    {
        isStunned = stunned;
        float originalSpeed = speed;
        speed = slowSpeed;
        yield return new WaitForSeconds(timeToWait);
        speed = originalSpeed;
        isStunned = false;
    }
    #endregion
}
