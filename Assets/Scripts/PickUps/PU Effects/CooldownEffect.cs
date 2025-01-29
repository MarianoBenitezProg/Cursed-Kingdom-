using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownEffect : IPowerUpEffect
{
    AbilityTimers abilityScript;
    P_Behaviour playerScript;

    public CooldownEffect(P_Behaviour playerRef, AbilityTimers abilityRef)
    {
        playerScript = playerRef;
        abilityScript = abilityRef;
    }
    public void PU_Effect(object param)
    {
        abilityScript.ResetAllCooldowns();
        playerScript._view.TintCharacter(new Color(0, 0.48f, 1, 1f)); ;
    }

    public void SubscribeEvent()
    {
        EventManager.Subscribe(TypeEvent.PowerUpCooldown, PU_Effect);
    }

    public void UnsubscribeEvent()
    {
        EventManager.Unsubscribe(TypeEvent.PowerUpCooldown, PU_Effect);
    }
}
