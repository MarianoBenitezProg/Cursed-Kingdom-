using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerranaRockPush : MonoBehaviour
{
    public float activableArea;
    public LayerMask playerLayer;
    public float pushForce; 
    public float allowDistance = 3f; 
    public Rigidbody2D rb;

    private bool limitReached = false;
    private bool rockCouldBeMove = false;
    private bool rockIsPushing = false;
    private P_Behaviour playerB;
    private Vector3 startPosition; // Posición inicial de la roca
    public GameObject rock;
    CircleCollider2D boxCollider;

    private void Start()
    {
        startPosition = transform.position;
        boxCollider = GetComponent<CircleCollider2D>();
        boxCollider.radius = activableArea;
    }

    private void Update()
    {

        if (playerB != null && !playerB.isMarkus)
        {
            if (rockCouldBeMove && Input.GetKey(KeyCode.E) && limitReached == false)
            {
                if (playerB.gameObject.transform.position.y > rock.transform.position.y)
                {
                    pushForce = -2;
                }
                else
                {
                    pushForce = 2;
                }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            rockCouldBeMove = true;
            playerB = collision.GetComponent<P_Behaviour>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == rock)
        {
            limitReached = true;
            rockCouldBeMove = false;
            Debug.Log("Entro");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activableArea);
    }
}
