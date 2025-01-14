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

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public GameObject player;

   public bool enemyHasSight = false;
   public bool isAttacking = false;

    public float timeToLoseSight = 6f;
    private float timeWithoutSight = 0f;


    IEnemyState currentState;

    protected virtual void Awake()
    {

        SetState(new PatrolState());
    }
    public void Update()
    {
        // Actualizar lógica del estado actual
        currentState?.UpdateState(this);

        // Verificar si el jugador está dentro del campo de visión
        enemyHasSight = CanSeeTarget(out GameObject target);

        if (enemyHasSight)
        {
            timeWithoutSight = 0f; // Reinicia el contador si puede ver al jugador

            if (!isAttacking)
            {
                isAttacking = true;
                SetState(new AtackState());
            }
        }
        else
        {
            timeWithoutSight += Time.deltaTime; // Incrementa el tiempo sin visión

            if (timeWithoutSight >= timeToLoseSight)
            {
                // Volver al estado de patrullaje si no ve al jugador por un tiempo
                isAttacking = false;
                SetState(new PatrolState());
            }
        }
    }

    #region metodos basicos
    public virtual void Die()
    {
        Destroy(gameObject);
    }
    public virtual void Attack()
    {
        Debug.Log("este es el ataque base aca irian los cambios");
    }
    #endregion

    public void SetState(IEnemyState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType())
        {
            return; // Si el tipo es el mismo, no cambiar el estado
        }

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    #region Field Of View
    public bool CanSeeTarget(out GameObject target)
    {
        target = null;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        foreach (Collider2D col in targetsInViewRadius )
        {
            GameObject potentialTarget = col.gameObject;

            Vector2 directionToTarget = (potentialTarget.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if (angleToTarget < viewAngle / 2f && potentialTarget != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, viewRadius, obstacleLayer | playerLayer);
                if (hit.collider != null && hit.collider.gameObject == potentialTarget)
                {
                    target = potentialTarget;
                    enemyHasSight = true;
                    player = target;
                    return true;
                }
            }
        }
        enemyHasSight = false;
        return false;
    }
    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        float radians = Mathf.Deg2Rad * (angleInDegrees + transform.eulerAngles.z);
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = DirectionFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirectionFromAngle(viewAngle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        if (CanSeeTarget(out GameObject target))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position); 
        }

        RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, transform.right, viewRadius, obstacleLayer);
        if (hitObstacle.collider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, hitObstacle.point); 
        }
    }

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
}
