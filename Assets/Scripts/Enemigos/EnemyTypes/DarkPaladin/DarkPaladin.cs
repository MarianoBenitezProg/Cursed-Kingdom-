using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPaladin : MonoBehaviour, ItakeDamage
{
    public List<Transform> path = new List<Transform>();
    public int health = 100;
    public float speed = 5f;
    public int dmg = 40;
    private float rotationSpeed = 5f;
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


    private GameObject player;
    public List<GameObject> obstaculos = new List<GameObject>();


    private Transform ShootPoint;

    float OutOfSigth;

    private void Awake()
    {

        ShootPoint = transform.GetChild(0);
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

        if (player != null)
        {
            playerDist = Vector2.Distance(transform.position, player.transform.position);
            var playersc = player.GetComponent<P_Behaviour>();


            //esto es hace cuando estas en el area roja
            if (playerDist <= attackRadius)
            {
                Atacktimer += Time.deltaTime;
                if (Atacktimer >= 1.5)
                {
                    playersc.TakeDamage(dmg);
                    Atacktimer = 0;
                }
            }//esto es hace cuando estas en el area amarilla
            else if (playerDist <= viewRadius && playerDist > attackRadius)
            {

                //con esto roto hacia el pj
                directionToPlayer = (player.transform.position - transform.position).normalized;
                angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);

                //con esto me muevo  hacia el pj
                Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
                transform.position += FowardDirection * speed * Time.deltaTime;

                //con esto le disparo al pj

                shootTimer += Time.deltaTime;
                if (shootTimer >= CoolDownShotTimer)
                {
                    Shoot();
                    shootTimer = 0f;
                }

            }
            else
            {
                lostTimer += Time.deltaTime;

                if (lostTimer > 4)
                {
                    player = null;
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
    #endregion



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


    private void Shoot()
    {

        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.DarkPaladinAtack);
        if (disparo != null)
        {
            var tiro = disparo.GetComponent<darkPaladinPROYEC>();
            tiro.Father = gameObject;
            disparo.transform.position = ShootPoint.position;
            disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
        }

    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }

}
