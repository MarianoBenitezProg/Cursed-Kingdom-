using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void Events(params object[] parameters);

    static Dictionary<TypeEvent, Events> _currentEvent = new();

    public static void Subscribe(TypeEvent type, Events method)
    {
        if (_currentEvent.ContainsKey(type))
        {
            _currentEvent[type] += method;
        }
        else
        {
            _currentEvent.Add(type, method);
        }
    }
    public static void Unsubscribe(TypeEvent type, Events method)
    {
        if (_currentEvent.ContainsKey(type))
        {
            _currentEvent[type] -= method;

            if (_currentEvent[type] == null)
            {
                _currentEvent.Remove(type);
            }
        }

    }
    public static void Trigger(TypeEvent type, params object[] parameters)
    {
        if (_currentEvent.ContainsKey(type))
            _currentEvent[type](parameters);
    }
}

public enum TypeEvent
{
    PowerUpLife,
    PowerUpCooldown,
    PowerUpFerana,
    PowerUpMarkus,
    DamageTaken
}

