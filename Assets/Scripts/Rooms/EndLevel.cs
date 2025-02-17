using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public string nextLevelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            if(LevelsManager.instance != null)
            {
                LevelsManager.instance.ChangeLevel(nextLevelName);
            }
        }
    }
}
