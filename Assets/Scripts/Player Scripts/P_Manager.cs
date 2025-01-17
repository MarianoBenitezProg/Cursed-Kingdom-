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

    private void Awake()
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
    
    public void RunMarkusBuff()
    {
        if(isMarkusBuff == true)
        {
            StartCoroutine(MarkusBuff(10));
        }
    }

    public IEnumerator MarkusBuff(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        isMarkusBuff = false;
    }
}

