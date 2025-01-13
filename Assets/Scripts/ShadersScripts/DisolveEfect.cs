using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisolveEfect : MonoBehaviour
{
    [SerializeField] private Material _material;

    private float dissolveAmount;
    private bool isDisolving;

    private void Update()
    {
        if (isDisolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
            _material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        else
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
            _material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        if (Input.GetKeyDown(KeyCode.T))
            isDisolving = true;
        if(Input.GetKeyDown(KeyCode.Y))
            isDisolving= false;
    }
}
