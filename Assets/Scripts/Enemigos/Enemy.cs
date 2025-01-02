using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Transform> path = new List<Transform>();
    public int Health = 100;
    public float Speed = 5f;

    IEnemyState currentState;

    public void SetState(IEnemyState newState)
    {
        currentState?.ExitState(this); 
        currentState = newState; 
        currentState.EnterState(this);
    }

    protected virtual void Awake()
    {

        SetState(new PatrolState());
    }

    public void Update()
    {
        currentState?.UpdateState(this);
    }
    public virtual void Patrol()
    {

    }

    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
