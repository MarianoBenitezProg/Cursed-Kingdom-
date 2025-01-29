using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkusBuffEffect : IPowerUpEffect
{

    P_Behaviour playerScript;

    public MarkusBuffEffect(P_Behaviour playerRef)
    {
        playerScript = playerRef;
    }

    public void PU_Effect(object param)
    {
        P_Manager.Instance.RunMarkusBuff(playerScript.buffTime);
    }

    public void SubscribeEvent()
    {
        EventManager.Subscribe(TypeEvent.PowerUpMarkus, PU_Effect);
    }

    public void UnsubscribeEvent()
    {
        EventManager.Unsubscribe(TypeEvent.PowerUpMarkus, PU_Effect);
    }
}
