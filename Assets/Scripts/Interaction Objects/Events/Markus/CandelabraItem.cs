using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabraItem : CharactersEvent
{
    public bool isActive;
    public bool actionTrigger;
    float timer;
    public float timerLimit;
    [SerializeField] CandelabraEvt mainScript;
    public float distance;
    [SerializeField]GameObject[] effects;

    private void Update()
    {
        if(mainScript.eventCompleted == false)
        {
            if (isActive == true)
            {
                Action();
            }
        }
        else if(mainScript.eventCompleted == true)
        {
            TurnOnCandles();
            isActive = true;
        }
        if(actionTrigger == true)
        {
            mainScript.Action();
            actionTrigger = false;
        }

        if(mainScript.isActive == true && isActive == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && player.isMarkus == true)
            {
                distance = Vector3.Distance(player.transform.position, this.transform.position);
                if (distance <= distanceToAction)
                {
                    isActive = true;
                    actionTrigger = true;
                }
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("MarkusDamage"))
        {
            isActive = true;
            actionTrigger = true;
        }
    }

    public override void Action()
    {
        TurnOnCandles();
        timer += Time.deltaTime;

        if(timer >= timerLimit)
        {
            TurnOffCandles();
            isActive = false;
            mainScript.itemsActiveCount--;
            timer = 0;
            
        }
    }

    public void TurnOnCandles()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(true);
        }
    }
    public void TurnOffCandles()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
    }
}
