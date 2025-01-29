using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeranaBuffEffect : IPowerUpEffect
{
    P_Behaviour playerScript;

    public FeranaBuffEffect(P_Behaviour playerRef)
    {
        playerScript = playerRef;
    }
    public void PU_Effect(object param)
    {
        P_Manager.Instance.RunFeranaBuff(playerScript.buffTime);
    }

    public void SubscribeEvent()
    {
        EventManager.Subscribe(TypeEvent.PowerUpFerana, PU_Effect);
    }

    public void UnsubscribeEvent()
    {
        EventManager.Unsubscribe(TypeEvent.PowerUpFerana, PU_Effect);
    }
}
