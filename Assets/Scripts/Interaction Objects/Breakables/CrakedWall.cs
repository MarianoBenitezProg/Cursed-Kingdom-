using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrakedWall : BreakableObjects, ItakeDamage
{
    [SerializeField] GameObject _secretRoom;

    public void TakeDamage(int dmg)
    {
        Debug.Log("<color=red>Object Hitted</color>");
        objectLife -= dmg;
        if(objectLife <= 0)
        {
            Destruction();
            Destroy(gameObject);
        }
    }

    protected override void Destruction()
    {
        if(_secretRoom != null)
        {
            SoundManager.instance.PlaySound("Wall Break",3f);
            _secretRoom.SetActive(true);
        }
        //Particles Effects. Trigger Sound.
        Debug.Log("<color=red>Destruction Triggered</color>");

    }
}
