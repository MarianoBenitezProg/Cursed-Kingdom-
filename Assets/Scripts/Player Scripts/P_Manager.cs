using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class P_Manager : MonoBehaviour
{
    public static P_Manager Instance { get; private set; }
    public bool isMarkusBuff;
    public bool isFeranaBuff;
    public bool isCCAbilityUnlocked;
    public bool isUtilityAbilityUnlocked;
    public bool isTutorialFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void RunMarkusBuff(float waitSeconds)
    {
        isMarkusBuff = true;
        StartCoroutine(MarkusBuff(waitSeconds));
    }

    public IEnumerator MarkusBuff(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        isMarkusBuff = false;
    }
    public void RunFeranaBuff(float waitSeconds)
    {
        isFeranaBuff = true;
        StartCoroutine(FeranaBuff(waitSeconds));
    }

    public IEnumerator FeranaBuff(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        isFeranaBuff = false;
    }
}

