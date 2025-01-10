using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePROYEC : MonoBehaviour
{
    private void Update()
    {
        if(gameObject.active == true)
        {
            Debug.Log("creee un diparo");
        gameObject.transform.position += Vector3.forward * 2f;
        }

    }
}
