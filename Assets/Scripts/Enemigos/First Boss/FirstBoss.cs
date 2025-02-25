using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : MonoBehaviour, ItakeDamage
{
    private int health;
    [SerializeField] private int maxhealth = 200;
    public List<GameObject> Plataformas;
    public GameObject player;
    public float BasicShootTimer;
    public Vector3 ShootPoint;
    public Vector3 originalPos;
    public CapsuleCollider2D colider;

    public Direction lookingDir;
    public Direction currentDir;
    SpriteRenderer sprite;

    public GameObject shootingPoint;

    public Animator _animator;
    private BossState currentState;

    // Cache state instances
    private readonly FaseInicial faseInicial = new FaseInicial();
    private readonly FaseUno faseUno = new FaseUno();
    private readonly FaseDos faseDos = new FaseDos();
    private readonly FaseFinal faseFinal = new FaseFinal();

    protected virtual void Awake()
    {
        colider = gameObject.GetComponent<CapsuleCollider2D>();
        originalPos = transform.position;
        health = maxhealth;
        _animator = GetComponent<Animator>();
        SetState(faseInicial);
        currentDir = lookingDir;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        ShootPoint = transform.GetChild(0).transform.position;
        currentState?.UpdateState(this);
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateShootingPoint();
        UpdateAnimation();
    }

    public void SetState(BossState newState)
    {
        if (currentState == newState) return; // Avoid unnecessary state changes

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        CheckLifeState();
    }

    public void CheckLifeState()
    {
        float healthPercentage = ((float)health / maxhealth) * 100f;

        BossState newState;
        if (healthPercentage > 90)
            newState = faseInicial;
        else if (healthPercentage > 60)
            newState = faseUno;
        else if (healthPercentage > 30)
            newState = faseDos;
        else
            newState = faseFinal;

        SetState(newState);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Direction GetLookDirection(Vector3 playerPosition, Vector3 enemyPosition)
    {
        Vector2 direction = playerPosition - enemyPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // The player is more to the left or right
            return direction.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            // The player is more up or down
            return direction.y > 0 ? Direction.Up : Direction.Down;
        }
    }
    public void UpdateAnimation()
    {
        if (_animator != null)
        {
            if (lookingDir == Direction.Down)
            {
                _animator.SetFloat("Y", -1f);
                _animator.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Left)
            {
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", 1f);
                sprite.flipX = true;
            }
            if (lookingDir == Direction.Right)
            {
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", 1f);
                sprite.flipX = false;
            }
        }
    }

    public void UpdateShootingPoint()
    {
        if (shootingPoint != null)
        {
            if (currentDir != lookingDir)
            {
                if (lookingDir == Direction.Down) shootingPoint.transform.position = new Vector3(this.transform.position.x +.5f, this.transform.position.y + 2, 0);
                else if (lookingDir == Direction.Left) shootingPoint.transform.position = new Vector3(this.transform.position.x - .6f, this.transform.position.y + 2f, 0);
                else if (lookingDir == Direction.Right) shootingPoint.transform.position = new Vector3(this.transform.position.x + .6f, this.transform.position.y + 2f, 0);
            }
            currentDir = lookingDir;
        }
    }
}



