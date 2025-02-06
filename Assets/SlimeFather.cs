using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeFather : MonoBehaviour, ItakeDamage
{
    [Header("Slime Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int damage = 20;
    [SerializeField] private int generationLevel = 0;  
    [SerializeField] private float slimeScale = 4f;    

    [Header("Split Settings")]
    [SerializeField] private GameObject slimePrefab;   

    [Header("Movement Settings")]
    [SerializeField] private float moveStep = 5f;
    [SerializeField] private float moveCooldown = 2f;
    [SerializeField] private float detectionRange = 5f;
    private float jumpTimer = 0f;

    public Transform player;

    private float currentHealth;



    private void Start()
    {
        currentHealth = maxHealth;
        transform.localScale = Vector3.one * slimeScale;
    }

    private void Update()
    {
        if(currentHealth <= 0 )
        {
            Die();
        }

        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= moveCooldown)
            {
                MoveTowardsPlayer();
                jumpTimer = 0f; // Reiniciar el temporizador
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcular dirección normalizada hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mover al slime en pasos de 1f en esa dirección
        transform.position += direction * moveStep;
    }

    private void Die()
    {
        if (generationLevel < 2) 
        {
            SpawnChildren();
        }
        Destroy(gameObject);
    }

    private void SpawnChildren()
    {
        //esto le da forma de triangulo cuando mueren 
        Vector3[] spawnPositions = new Vector3[3];
        spawnPositions[0] = transform.position + new Vector3(2,0); 
        spawnPositions[1] = transform.position + new Vector3(-2,0);
        spawnPositions[2] = transform.position + new Vector3(0, 2);

        //con esto se setea que sean mas chicos, con menos ataque y menos vida 
        for (int i = 0; i < 3; i++)
        {
            GameObject child = Instantiate(slimePrefab, spawnPositions[i], Quaternion.identity);
            SlimeFather childSlime = child.GetComponent<SlimeFather>();

            if (childSlime != null)
            {
                // mitad de las estadísticas 
                childSlime.maxHealth = maxHealth / 2f;
                childSlime.currentHealth = childSlime.maxHealth;
                childSlime.damage = damage / 2;
                childSlime.generationLevel = generationLevel + 1;
                childSlime.slimeScale = slimeScale / 2f;

                //mitad de la escala 
                child.transform.localScale = Vector3.one * childSlime.slimeScale;

            }
        }
    }
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log(currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            var player = collision.gameObject.GetComponent<P_Behaviour>();
            player.TakeDamage(damage);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
