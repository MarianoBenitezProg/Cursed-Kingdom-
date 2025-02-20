using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabraItem : MarkusEvent
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
        if(isActive == true)
        {
            Action();
        }
        if(actionTrigger == true)
        {
            mainScript.itemsActiveCount++;
            actionTrigger = false;
        }

        if(mainScript.isActive == true && isActive == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(true);
        }
        timer += Time.deltaTime;

        if(timer >= timerLimit)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].SetActive(false);
            }
            isActive = false;
            mainScript.itemsActiveCount--;
            timer = 0;
            
        }
    }
}
