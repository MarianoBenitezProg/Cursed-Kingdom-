using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour, ItakeDamage
{
    public Vector3 originPoint;

    [Header("variables tint")]
    MaterialTintColor _tintMaterial;
    public Material thisMaterial;
    protected bool isDying;
    private float dissolveAmount = 1f;
    public float dissolveDuration = 0.4f;


    [Header("variables basicas")]
    private int health;
    [SerializeField] private int maxhealth = 200;
    public int damage = 10;
    public float speed = 10f;
    public Vector3 dir;

    public Direction lookingDir;



    [Header("variables ataques")]
    public GameObject player;
    public Animator playerAnimator;
    public float atackTimer = 0f;
    public float atackCountDown = 2f;
    public float viewRadius = 10f;


    [Header("variables vision ")]
    public float obstacleRadius = 5f;
    public float playerDistance;
    public float allowRunDistance = 3f;
    public float directionTime;



    private BoxCollider2D colider;
    public Rigidbody2D rb;
    private SecondBossState currentState;
    public Animator animatorToro;

    SpriteRenderer _sprite;

    [Header("variables enemigos  y layers")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> rejas = new List<GameObject>();
    public List<GameObject> enemigsToSpawn = new List<GameObject>();
    public List<GameObject> paths = new List<GameObject>();



    protected virtual void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _tintMaterial = GetComponent<MaterialTintColor>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        thisMaterial = new Material(renderer.material);
        renderer.material = thisMaterial;
        _tintMaterial.setMaterial(thisMaterial);


        animatorToro = GetComponent<Animator>();
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

        if(isDying)
        {
            transform.position = originPoint;
            rb.velocity = Vector2.zero;
            damage = 0;
            colider.isTrigger = false;
            viewRadius = 0.1f;
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
        _tintMaterial.SetTintColor(color: Color.red);
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
            isDying = true;
            StartCoroutine(DissolveCoroutine());
        }
    }
    private void HasAnEnemy()
    {
        Collider2D[] playersInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        if (playersInRadius.Length > 0)
        {
            player = playersInRadius[0].gameObject;
            playerDistance = Vector3.Distance(player.transform.position, transform.position);

            lookingDir = GetLookDirection(player.transform.position, this.transform.position);
            UpdateAnimation();
        }
        else
        {
            player = null;
     
        }
    }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var playerHealth = collision.GetComponent<P_Behaviour>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
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
    public void UpdateAnimation()
    {
        if (animatorToro != null)
        {
            if (lookingDir == Direction.Up)
            {
                animatorToro.SetFloat("Y", 1f);
                animatorToro.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Down)
            {
                animatorToro.SetFloat("Y", -1f);
                animatorToro.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Left)
            {
                animatorToro.SetFloat("Y", 0f);
                animatorToro.SetFloat("X", -1f);
                _sprite.flipX = false;

            }
            if (lookingDir == Direction.Right)
            {
                animatorToro.SetFloat("Y", 0f);
                animatorToro.SetFloat("X", 1f);
                _sprite.flipX = true;
            }
        }
    }


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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, obstacleRadius);

    }
}
