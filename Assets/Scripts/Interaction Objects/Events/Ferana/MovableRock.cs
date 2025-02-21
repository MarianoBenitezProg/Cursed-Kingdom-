using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableRock : CharactersEvent
{
    [SerializeField]bool playerIsClose;
    float distance;
    Direction playerDir;
    Rigidbody2D _rb;
    [SerializeField] float speed;
    bool isPushing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(playerIsClose == true)
        {
            playerDir = GetLookDirection(player.transform.position, this.transform.position);
            if(Input.GetKeyDown(KeyCode.E))
            {
                MoveRock();
            }
            else
            {
                _rb.velocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            playerIsClose = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            playerIsClose = false;
        }
    }


   

    public Direction GetLookDirection(Vector3 playerPosition, Vector3 rockPosition)
    {
        Vector2 direction = playerPosition - rockPosition;
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

    public void MoveRock()
    {
            Vector2 movement = Vector2.zero;

            if (playerDir == Direction.Up)
                movement = -transform.up;
            else if (playerDir == Direction.Down)
                movement = transform.up;
            else if (playerDir == Direction.Left)
                movement = transform.right;
            else if (playerDir == Direction.Right)
                movement = -transform.right;

            // Method 1: Using velocity (good for constant movement)
            //_rb.velocity = movement * speed;

            // OR Method 2: Using MovePosition (more controlled movement)
            _rb.MovePosition(_rb.position + movement * speed * Time.deltaTime);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
