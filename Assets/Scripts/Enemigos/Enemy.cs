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

        // Actualizar los estados de detección y ataque
        enemyHasSight = CanSeeTarget();
        needToAtack = IsPlayerInAttackRadius();

        if (needToAtack)
        {
            SetState(new AtackState());
        }
        else if (!enemyHasSight)
        {
            timeWithoutSight += Time.deltaTime;

            if (timeWithoutSight >= timeToLoseSight)
            {
                ResetState();
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

    private void ResetState()
    {
        needToAtack = false;
        enemyHasSight = false;
        player = null;
        timeWithoutSight = 0f;
        SetState(new PatrolState());
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

            Vector2 directionToTarget = potentialTarget.transform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            // Si el jugador está muy cerca, asumir que está visible
            if (distanceToTarget < 1f || IsPlayerInAttackRadius())
            {
                player = potentialTarget;
                timeWithoutSight = 0f;
                return true;
            }

            directionToTarget.Normalize();

            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if (angleToTarget < viewAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer | playerLayer);
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

        return Vector3.Distance(transform.position, player.transform.position) <= attackRadius;
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
        // Visualizar radio de visión
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Visualizar radio de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Límites del ángulo de visión
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
