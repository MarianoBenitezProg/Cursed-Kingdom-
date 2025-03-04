using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IStunned
{
    public List<Transform> path = new List<Transform>();
    public int health = 100;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public bool isStunned;

    public float viewRadius = 10f;
    public float attackRadius = 5f;

    public float lostTimer;


    public float playerDist;

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public GameObject player;
    public GameObject shootingPoint;

    public Material thisMaterial;
    protected bool isDying;
    private float dissolveAmount = 1f;
    public float dissolveDuration = 0.4f;

    [SerializeField] public bool needToAtack = false;
    [SerializeField] public bool needToSeek = false;

    public Direction lookingDir;
    Direction currentDir;
    public Animator _animator;

    protected MaterialTintColor _tintMaterial;

    PatrolState _patrol;
    AtackState _attackState;
    SeekState _seekState;

    float OutOfSigth;
    public IEnemyState currentState;

    protected virtual void Awake()
    {
        _patrol = new PatrolState();
        _attackState = new AtackState();
        _seekState = new SeekState();

        SetState(_patrol);
        if(GetComponent<Animator>() != null)
        {
            _animator = GetComponent<Animator>();
            lookingDir = Direction.Down;
            UpdateAnimation();
        }

        _tintMaterial = GetComponent<MaterialTintColor>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        thisMaterial = new Material(renderer.material);
        renderer.material = thisMaterial;

        _tintMaterial.setMaterial(thisMaterial);

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

        UpdateShootingPoint();
        CanSeeTarget();

        if (needToAtack && player != null)
        {
            SetState(_attackState);
        }
        else if (needToSeek && player != null)
        {
            SetState(_seekState);
        }
    }


    #region Métodos básicos
    public virtual void Die()
    {
        isDying = true;
        StartCoroutine(DissolveCoroutine());

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
            }
            else if (playerDist <= viewRadius && playerDist > attackRadius)
            {
                needToSeek = true;
                needToAtack = false;
                lostTimer = 0; // Reiniciar el temporizador
            }
            else
            {
                lostTimer += Time.deltaTime;

                if (lostTimer > 2)
                {
                    needToSeek = false;
                    needToAtack = false;
                    player = null;
                    SetState(new PatrolState());
                    lookingDir = Direction.Down;
                    UpdateAnimation();
                    lostTimer = 0;
                }
            }
        }
    }

    public Direction GetLookDirection(Vector3 playerPosition, Vector3 enemyPosition)
    {
        Vector2 direction = playerPosition - enemyPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return direction.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    public void UpdateShootingPoint()
    {
        if(shootingPoint != null)
        {
            if (currentDir != lookingDir)
            {
                if (lookingDir == Direction.Up) shootingPoint.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, 0);
                else if (lookingDir == Direction.Down) shootingPoint.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1, 0);
                else if (lookingDir == Direction.Left) shootingPoint.transform.position = new Vector3(this.transform.position.x - 1.1f, this.transform.position.y -.5f, 0);
                else if (lookingDir == Direction.Right) shootingPoint.transform.position = new Vector3(this.transform.position.x + 1.1f, this.transform.position.y-.5f, 0);
            }
            currentDir = lookingDir;
        }
    }

    public void UpdateAnimation()
    {
        if(_animator != null)
        {
            if (lookingDir == Direction.Up)
            {
                _animator.SetFloat("Y", 1f);
                _animator.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Down)
            {
                _animator.SetFloat("Y", -1f);
                _animator.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Left)
            {
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", -1f);
            }
            if (lookingDir == Direction.Right)
            {
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", 1f);
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


    private IEnumerator DissolveCoroutine()
    {
        float elapsedTime = 0f;
        float startValue = dissolveAmount;

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveAmount = Mathf.Lerp(startValue, 0f, elapsedTime / dissolveDuration);

            // Update the shader parameter
            thisMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        }

        // Ensure we reach exactly 0
        EventManager.Trigger(TypeEvent.EnemyKilled);
        Destroy(gameObject);
        dissolveAmount = 0f;
        thisMaterial.SetFloat("_DissolveAmount", dissolveAmount);

    }
}
