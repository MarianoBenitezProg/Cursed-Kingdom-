using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

public class AbilityTimers : MonoBehaviour
{
    public static AbilityTimers instance;
    
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> currentCooldowns = new Dictionary<string, float>();
    private Dictionary<string, Action> cooldownCallbacks = new Dictionary<string, Action>();

    // Added UnityEvent for cooldown updates
    public UnityEvent<string, float> OnCooldownUpdated = new UnityEvent<string, float>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
                
                // Trigger event when cooldown updates
                float progress = GetCooldownProgress(key);
                OnCooldownUpdated.Invoke(key, progress);

                if (currentCooldowns[key] <= 0)
                {
                    currentCooldowns[key] = 0;
                    if (cooldownCallbacks.ContainsKey(key))
                    {
                        cooldownCallbacks[key]?.Invoke();
                        Debug.Log($"Cooldown finished for {key}");
                    }
                    // Final update when cooldown ends
                    OnCooldownUpdated.Invoke(key, 1f);
                }
            }
        }
    }

    public float GetCooldownProgress(string abilityName)
    {
        if (!cooldowns.ContainsKey(abilityName) || !currentCooldowns.ContainsKey(abilityName))
            return 1f; // Return 1 if ability not found (means no cooldown)

        float totalCooldown = cooldowns[abilityName];
        float currentCooldown = currentCooldowns[abilityName];
        
        return 1f - (currentCooldown / totalCooldown);
    }

    public void StartCooldown(string abilityName, Action onCooldownComplete)
    {
        if (cooldowns.ContainsKey(abilityName))
        {
            currentCooldowns[abilityName] = cooldowns[abilityName];
            cooldownCallbacks[abilityName] = onCooldownComplete;
            Debug.Log($"Started cooldown for {abilityName}: {cooldowns[abilityName]} seconds");
            OnCooldownUpdated.Invoke(abilityName, 0f); // Trigger UI update immediately
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
