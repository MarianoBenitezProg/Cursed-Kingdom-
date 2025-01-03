using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AbilityTimers : MonoBehaviour
{
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> currentCooldowns = new Dictionary<string, float>();
    private Dictionary<string, Action> cooldownCallbacks = new Dictionary<string, Action>();

    public void Initialize(Dictionary<string, float> abilityCooldowns)
    {
        cooldowns = new Dictionary<string, float>(abilityCooldowns);
        foreach (var ability in cooldowns)
        {
            currentCooldowns[ability.Key] = 0f;
        }
    }

    private void Update()
    {
        var keys = currentCooldowns.Keys.ToList();
        foreach (var key in keys)
        {
            if (currentCooldowns[key] > 0)
            {
                currentCooldowns[key] -= Time.deltaTime;

                if (currentCooldowns[key] <= 0)
                {
                    currentCooldowns[key] = 0;
                    if (cooldownCallbacks.ContainsKey(key))
                    {
                        cooldownCallbacks[key].Invoke();
                        Debug.Log($"Cooldown finished for {key}");
                    }
                }
            }
        }
    }

    public void StartCooldown(string abilityName, Action onCooldownComplete)
    {
        if (cooldowns.ContainsKey(abilityName))
        {
            currentCooldowns[abilityName] = cooldowns[abilityName];
            cooldownCallbacks[abilityName] = onCooldownComplete;
            Debug.Log($"Started cooldown for {abilityName}: {cooldowns[abilityName]} seconds");
        }
    }

    public bool IsOnCooldown(string abilityName)
    {
        return currentCooldowns.ContainsKey(abilityName) && currentCooldowns[abilityName] > 0;
    }

    public float GetRemainingCooldown(string abilityName)
    {
        return currentCooldowns.ContainsKey(abilityName) ? currentCooldowns[abilityName] : 0f;
    }
}
