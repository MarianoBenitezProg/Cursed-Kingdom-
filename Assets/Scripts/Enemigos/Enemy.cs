using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Transform> path = new List<Transform>();
    public int Health = 100;
    public float Speed = 5f;
    public float RotationSpeed = 5f;

    public float ViewAngle = 45f;
    public float ViewRadius = 10f;

    public LayerMask playerLayer;
    public LayerMask ObstacleLayer;

    public GameObject player;

   public bool enemyHasSight = false;
   public bool isAttacking = false;

    public float timeToLoseSight = 6f; // Tiempo necesario para perder la visión
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
    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Die();
    }
    public virtual void Die()
    {
        Destroy(gameObject);
    }
    public virtual void Atack()
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

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, ViewRadius, playerLayer);

        foreach (Collider2D col in targetsInViewRadius )
        {
            GameObject potentialTarget = col.gameObject;

            Vector2 directionToTarget = (potentialTarget.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if (angleToTarget < ViewAngle / 2f && potentialTarget != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, ViewRadius, ObstacleLayer | playerLayer);
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
        Gizmos.DrawWireSphere(transform.position, ViewRadius);

        Vector3 leftBoundary = DirectionFromAngle(-ViewAngle / 2);
        Vector3 rightBoundary = DirectionFromAngle(ViewAngle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * ViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * ViewRadius);

        if (CanSeeTarget(out GameObject target))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position); 
        }

        RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, transform.right, ViewRadius, ObstacleLayer);
        if (hitObstacle.collider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, hitObstacle.point); 
        }
    }


}
