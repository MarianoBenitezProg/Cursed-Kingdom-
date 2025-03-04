using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPaladin : MonoBehaviour, ItakeDamage
{
    public List<Transform> path = new List<Transform>();
    public int health = 100;
    public float speed = 5f;
    public int dmg = 40;
    public bool isStunned;

    float angleToPlayer;
    Vector3 directionToPlayer;

    public float viewRadius = 10f;
    public float attackRadius = 5f;
    private float Atacktimer;
    private float lostTimer;

    public float shootTimer;
    public float CoolDownShotTimer = 6f;

    public float playerDist;

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public Direction lookingDir;
    Direction currentDir;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject shootingPoint;
    [SerializeField] Animator _animator;

    [SerializeField]Material thisMaterial;
    MaterialTintColor _tintMaterial;
    bool isDying;
    bool isShooting;


    [SerializeField] GameObject player;
    public List<GameObject> obstaculos = new List<GameObject>();


    private Transform ShootPoint;

    float OutOfSigth;

    private void Awake()
    {
        ShootPoint = transform.GetChild(0);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        hasATarget();
        hasAnObstacle();
        GetObstacleDirections();
    }


    #region Field Of View 
    public void hasATarget()
    {

        // aca consigo los gameobects
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);
        foreach (Collider2D col in targetsInViewRadius)
        {
            if (((1 << col.gameObject.layer) & playerLayer) != 0)
            {
                player = col.gameObject;
                break;
            }
        }

        if (player != null && isDying == false)
        {
            playerDist = Vector2.Distance(transform.position, player.transform.position);
            var playersc = player.GetComponent<P_Behaviour>();

            //esto es hace cuando estas en el area roja
            if (playerDist <= attackRadius)
            {
                
                Atacktimer += Time.deltaTime;
                if (Atacktimer >= 1.5)
                {
                    _animator.SetTrigger("IsMelee");
                    playersc.TakeDamage(dmg);
                    Atacktimer = 0;
                }
            }//esto es hace cuando estas en el area amarilla
            else if (playerDist <= viewRadius && playerDist > attackRadius && isShooting == false)
            {
                lookingDir = GetLookDirection(player.transform.position, this.transform.position);
                UpdateAnimation();
                UpdateShootingPoint();

                
                _animator.SetBool("IsRunning", true);
                //con esto me muevo  hacia el pj
                Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
                transform.position += FowardDirection * speed * Time.deltaTime;

                //con esto le disparo al pj

                shootTimer += Time.deltaTime;
                if (shootTimer >= CoolDownShotTimer)
                {
                    isShooting = true;
                    StartCoroutine(ShootingCoroutine());
                    shootTimer = 0f;
                }

            }
            else
            {
                lostTimer += Time.deltaTime;
                _animator.SetBool("IsRunning", false);

                if (lostTimer > 4)
                {
                    Debug.Log("Perdí al jugador, vuelvo a patrullar");
                    lostTimer = 0;
                }
            }
        }
    }

    void hasAnObstacle()
    {
        Collider2D[] obstaclesInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, obstacleLayer);
        List<GameObject> detectedObstacles = new List<GameObject>();

        foreach (Collider2D col in obstaclesInViewRadius)
        {
            if (!obstaculos.Contains(col.gameObject))
            {
                obstaculos.Add(col.gameObject);
            }

            detectedObstacles.Add(col.gameObject);
        }

        obstaculos.RemoveAll(obstaculo => !detectedObstacles.Contains(obstaculo));
    }

    List<Vector2> GetObstacleDirections()
    {

        List<Vector2> obstacleDirections = new List<Vector2>();

        foreach (GameObject obstaculo in obstaculos)
        {
            Vector2 direction = (obstaculo.transform.position - transform.position).normalized;
            obstacleDirections.Add(direction);
        }

        return obstacleDirections;
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

    public void UpdateShootingPoint()
    {
        if (shootingPoint != null)
        {
            if (currentDir != lookingDir)
            {
                if (lookingDir == Direction.Up) shootingPoint.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 4, 0);
                else if (lookingDir == Direction.Down) shootingPoint.transform.position = new Vector3(this.transform.position.x - .1f, this.transform.position.y - 1, 0);
                else if (lookingDir == Direction.Left) shootingPoint.transform.position = new Vector3(this.transform.position.x - 1.1f, this.transform.position.y - .5f, 0);
                else if (lookingDir == Direction.Right) shootingPoint.transform.position = new Vector3(this.transform.position.x + 1.1f, this.transform.position.y - .5f, 0);
            }
            currentDir = lookingDir;
        }
    }

    public void UpdateAnimation()
    {
        if (_animator != null)
        {
            if (lookingDir == Direction.Up)
            {
                _spriteRenderer.flipX = false;
                _animator.SetFloat("Y", 1f);
                _animator.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Down)
            {
                _spriteRenderer.flipX = false;
                _animator.SetFloat("Y", -1f);
                _animator.SetFloat("X", 0f);
            }
            if (lookingDir == Direction.Left)
            {
                _spriteRenderer.flipX = false;
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", -1f);
            }
            if (lookingDir == Direction.Right)
            {
                _spriteRenderer.flipX = true;
                _animator.SetFloat("Y", 0f);
                _animator.SetFloat("X", 1f);
            }
        }
    }
    #endregion

    public IEnumerator ShootingCoroutine()
    {
        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.DarkPaladinAtack);
        disparo.transform.position = ShootPoint.position;
        _animator.SetTrigger("IsShooting");
        yield return new WaitForSeconds(1f);
        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        if (disparo != null)
        {
            var tiro = disparo.GetComponent<darkPaladinPROYEC>();
            tiro.Father = gameObject;
            disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
        }
        isShooting = false;
    }
    #region OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (obstaculos != null)
        {
            Gizmos.color = Color.green;

            foreach (GameObject obstaculo in obstaculos)
            {
                if (obstaculo != null)
                {
                    Vector2 direction = (obstaculo.transform.position - transform.position).normalized;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction * 2);
                }
            }
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

    
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    } 
    public void Die()
    {
        isDying = true;
        //StartCoroutine(DissolveCoroutine());
        Destroy(gameObject);
        EventManager.Trigger(TypeEvent.EnemyKilled);
    }

    private IEnumerator DissolveCoroutine()
    {
        float dissolveAmount = 1f;
        float elapsedTime = 0f;
        float startValue = dissolveAmount;
        float duration = .40f; // 1 second duration

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            dissolveAmount = Mathf.Lerp(startValue, 0f, elapsedTime / duration);

            // Update the shader parameter
            thisMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        }

        // Ensure we reach exactly 0
        Destroy(gameObject);
        dissolveAmount = 0f;
        thisMaterial.SetFloat("_DissolveAmount", dissolveAmount);

        // Optional: Destroy the game object after dissolution
        // Uncomment the next line if you want the object to be destroyed
        // Destroy(gameObject);
    }
}
