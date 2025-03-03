using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeEffect : IPowerUpEffect
{
    P_Behaviour playerScript;
    int addLife = 15;
    public LifeEffect(P_Behaviour playerRef)
    {
        playerScript = playerRef;
    }
    public void PU_Effect(object param)
    {
        Debug.Log(playerScript);
        if (playerScript != null)
        {
            playerScript.life += playerScript.lifeAddedEffect;
            if (playerScript.life > 100)
            {
                playerScript.life = 100;
            }
            EventManager.Trigger(TypeEvent.HealthUpdate);
            playerScript._view.TintCharacter(new Color(0, 1, 0, 1f));
        }
    }

    public void SubscribeEvent()
    {
        EventManager.Subscribe(TypeEvent.PowerUpLife, PU_Effect);
    }

    public void UnsubscribeEvent()
    {
        EventManager.Unsubscribe(TypeEvent.PowerUpLife, PU_Effect);
    }
}
