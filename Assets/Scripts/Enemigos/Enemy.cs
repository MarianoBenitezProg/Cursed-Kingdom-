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

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public GameObject player;

    public bool enemyHasSight = false;
    public bool needToAtack = false;

    public float timeToLoseSight = 6f;
    private float timeWithoutSight = 0f;

    IEnemyState currentState;

    protected virtual void Awake()
    {
        SetState(new PatrolState());
    }

    public void Update()
    {
        currentState?.UpdateState(this);

        if (health <= 0)
        {
            Die();
        }

        enemyHasSight = CanSeeTarget();
        needToAtack = enemyHasSight && IsPlayerInAttackRadius();

        if (needToAtack)
        {
            SetState(new AtackState());
        }
        else if (!enemyHasSight)
        {
            timeWithoutSight += Time.deltaTime;

            if (timeWithoutSight >= timeToLoseSight)
            {
                needToAtack = false;
                player = null;
                SetState(new PatrolState());
            }
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
    #endregion

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

    #region Field Of View y Attack Radius
    public bool CanSeeTarget()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        foreach (Collider2D col in targetsInViewRadius)
        {
            GameObject potentialTarget = col.gameObject;

            Vector2 directionToTarget = (potentialTarget.transform.position - transform.position).normalized;
             float distanceToTarget = directionToTarget.magnitude;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if ( potentialTarget != null) // Borre un && que quedo vacio y daba error de Compilado
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, viewRadius, obstacleLayer | playerLayer);
                if (hit.collider != null && hit.collider.gameObject == potentialTarget)
                {
                    player = potentialTarget;
                    timeWithoutSight = 0f; 
                    return true;
                }
            }
        }
        player = null;
        return false;
    }

    public bool IsPlayerInAttackRadius()
    {
        if (player == null)
            return false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= attackRadius;
    }

    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        float radians = Mathf.Deg2Rad * (angleInDegrees + transform.eulerAngles.z);
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
    }
    #endregion



    #region OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Vector3 leftBoundary = DirectionFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirectionFromAngle(viewAngle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        if (enemyHasSight && player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
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
