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

   public bool enemyHasSight = false;
   public bool isAttacking = false;


    IEnemyState currentState;

    protected virtual void Awake()
    {

        SetState(new PatrolState());
    }
    public void Update()
    {
        currentState?.UpdateState(this);

        if (enemyHasSight && !isAttacking)
        {
            isAttacking = true;
            enemyHasSight = false; // Desactiva temporalmente la visión
            SetState(new AtackState());
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
    public bool CanSeeTarget(out Transform target)
    {
        target = null;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, ViewRadius, playerLayer);

        foreach (Collider2D col in targetsInViewRadius )
        {
            Transform potentialTarget = col.transform;

            Vector2 directionToTarget = (potentialTarget.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if (angleToTarget < ViewAngle / 2f && potentialTarget != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, ViewRadius, ObstacleLayer | playerLayer);
                if (hit.collider != null && hit.collider.transform == potentialTarget )
                {
                    target = potentialTarget;
                    enemyHasSight = true;
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

        if (CanSeeTarget(out Transform target))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position); 
        }

        RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, transform.right, ViewRadius, ObstacleLayer);
        if (hitObstacle.collider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, hitObstacle.point); 
        }
    }


}
