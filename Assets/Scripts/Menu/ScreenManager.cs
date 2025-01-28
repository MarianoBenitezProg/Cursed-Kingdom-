using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;
    Stack<IScreen> _screens = new Stack<IScreen>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }
    public void Push(IScreen screen)
    {
        if (_screens.Count > 0)
        {
            _screens.Peek().Deactivate();
        }
        _screens.Push(screen);
        screen.Activate();
    }
    public void Push(string screenName)
    {
        var screenGameObject = Instantiate(Resources.Load(screenName));

        Push(screenGameObject.GetComponent<IScreen>());
    }
    public void Pop()
    {
        if (_screens.Count < 1) return;

        _screens.Pop().Free();

        if (_screens.Count > 0)
        {
            _screens.Peek().Activate();
        }
    }
}
