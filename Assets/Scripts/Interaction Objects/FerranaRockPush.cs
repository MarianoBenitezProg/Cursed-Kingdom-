using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerranaRockPush : MonoBehaviour
{
    public float activableArea;
    public LayerMask playerLayer;
    public float pushForce; // Fuerza del empuje en Y
    public float allowDistance = 3f; // Límite de distancia en Y
    public Rigidbody2D rb; // Rigidbody2D de la roca

    private bool rockCouldBeMove = false;
    private bool rockIsPushing = false;
    private P_Behaviour playerB;
    private Vector3 startPosition; // Posición inicial de la roca

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D de la roca
        startPosition = transform.position; // Guarda la posición inicial de la roca
    }

    private void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, activableArea, playerLayer);

        if (playerCollider != null)
        {
            playerB = playerCollider.GetComponent<P_Behaviour>();
            if(playerCollider.transform.position.y > transform.position.y)
            {
                pushForce = -2;
            }else
            {
                pushForce = 2;

            }

            if (playerB != null && !playerB.isMarkus)
            {
                float distanceMoved = transform.position.y - startPosition.y;

                rockCouldBeMove = distanceMoved < allowDistance;

                if (rockCouldBeMove && Input.GetKey(KeyCode.E))
                {
                    rockIsPushing = true;
                    Debug.Log("El jugador está empujando la roca.");
                }
                else
                {
                    rockIsPushing = false;
                }
            }
            else
            {
                rockCouldBeMove = false;
                rockIsPushing = false;
            }
        }
        else
        {
            rockCouldBeMove = false;
            rockIsPushing = false;
        }
    }

    private void FixedUpdate()
    {
        if (rockIsPushing && rockCouldBeMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, pushForce); // Mueve la roca en Y
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Detiene la roca si no puede moverse más
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activableArea);
    }
}
