using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUpEffect
{
    public void PU_Effect(object param);
    public void SubscribeEvent();
    public void UnsubscribeEvent();
}
