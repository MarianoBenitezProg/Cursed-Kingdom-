using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabraEvt : CharactersEvent
{
    public bool eventCompleted;
    [SerializeField]CandelabraItem[] items;
    public int itemsActiveCount;
    public bool isActive;
    public GameObject lockedHallway;

    [SerializeField] Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isActive = false;
        }
    }
    private void Update()
    {
        
    }

    public override void Action()
    {
        itemsActiveCount++;
        if (itemsActiveCount >= items.Length)
        {
            eventCompleted = true;
        }
        if (eventCompleted == true)
        {
            _animator.SetTrigger("IsActive");
            BoxCollider2D fogCollider = lockedHallway.GetComponent<BoxCollider2D>();
            fogCollider.enabled = false;
            Debug.Log("<color=green>Event Completed</color>");
        }
    }
}
