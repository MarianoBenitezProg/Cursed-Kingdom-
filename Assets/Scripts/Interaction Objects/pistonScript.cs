using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistonScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        var player = collision.gameObject.GetComponent<P_Behaviour>();
        var piedra = collision.gameObject.GetComponent<FerranaRockPush>();


        if(piedra != null)
        {
            Debug.Log("me esta pisando una piedra");
        }

        if(player != null)
        {
            Debug.Log("me esta pisando un player");
        }
        

    }
}
