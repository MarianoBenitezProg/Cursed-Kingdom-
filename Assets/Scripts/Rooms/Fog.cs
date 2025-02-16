using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    bool isOnFog;
    float timer;
    public int damage;
    public float timerLimit;
    P_Behaviour _playerScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            _playerScript = collision.gameObject.GetComponent<P_Behaviour>();
            isOnFog = true;
            _playerScript.TakeDamage(damage);
            _playerScript.Stunned(3f,2f,false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isOnFog = false;
            timer = 0;
        }
    }

    private void Update()
    {
        if(isOnFog == true)
        {
            DamagePlayer();
        }
    }
    public void DamagePlayer()
    {
        timer += Time.deltaTime;
        if(timer >= timerLimit)
        {
            _playerScript.TakeDamage(damage);
            timer = 0;
        }
    }
}
