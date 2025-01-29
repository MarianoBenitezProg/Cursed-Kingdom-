using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PickUpType
{
    Life,
    CooldownReset,
    FeranaPU,
    MarkusPU
}

public class PowerUps
{
    P_Behaviour playerScript;
    AbilityTimers abilityScript;

    #region Effects Scripts
    LifeEffect _lifeEffect;
    CooldownEffect _cdEffect;
    FeranaBuffEffect _feranaBuff;
    MarkusBuffEffect _markusBuff;

    #endregion

    public PowerUps(P_Behaviour playerRef, AbilityTimers abilityRef)
    {
        playerScript = playerRef;
        abilityScript = abilityRef;

        _lifeEffect = new LifeEffect(playerScript);
        _cdEffect = new CooldownEffect(playerScript, abilityScript);
        _feranaBuff = new FeranaBuffEffect(playerScript);
        _markusBuff = new MarkusBuffEffect(playerScript);
    }

    public void SubscribeEffects()
    {
        _lifeEffect.SubscribeEvent();
        _cdEffect.SubscribeEvent();
        _feranaBuff.SubscribeEvent();
        _markusBuff.SubscribeEvent();
    }
    public void UnsubscribeEffects()
    {
        _lifeEffect.UnsubscribeEvent();
        _cdEffect.UnsubscribeEvent();
        _feranaBuff.UnsubscribeEvent();
        _markusBuff.UnsubscribeEvent();
    }
}
