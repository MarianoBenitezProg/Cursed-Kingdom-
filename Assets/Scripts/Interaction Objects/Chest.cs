using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interaction
{
    [SerializeField] SpriteRenderer closedChest;
    [SerializeField] SpriteRenderer openChest;
    [SerializeField] GameObject[] loot;
    [SerializeField] GameObject lootSpawnPoint;

    private void Update()
    {
        if(canInteract == true && Input.GetKeyDown(interactionKey))
        {
            Action();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer ==3)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            canInteract = false;
        }
    }

    public override void Action()
    {
        closedChest.enabled = false;
        openChest.enabled = true;

        Instantiate(loot[Random.Range(0,loot.Length)], lootSpawnPoint.transform.position, Quaternion.identity);
    }
}
