using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabraEvt : MarkusEvent
{
    bool eventCompleted;
    [SerializeField]CandelabraItem[] items;
    public int itemsActiveCount;
    public bool isActive;

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
        if(isActive == true)
        {
            if(itemsActiveCount >= items.Length)
            {
                eventCompleted = true;
            }
            if(eventCompleted == true)
            {
                Debug.Log("<color=green>Event Completed</color>");
            }
        }
    }

    public override void Action()
    {
        itemsActiveCount++;
    }
}
