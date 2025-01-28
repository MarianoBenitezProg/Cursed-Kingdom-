using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour, IScreen
{
    public void BTN_CallScreen(string screenName)
    {
        ScreenManager.Instance.Push(screenName);
    }
    public void BTN_Back()
    {
        ScreenManager.Instance.Pop();
    }
    public void BTN_ExitGame()
    {

    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void Free()
    {
        Destroy(gameObject);
    }
}
