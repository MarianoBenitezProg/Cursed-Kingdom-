using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private MaterialTintColor MaterialTintColor;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MaterialTintColor.SetTintColor(new Color(0, 1, 0, 1f));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            MaterialTintColor.SetTintColor(new Color(1, 0, 0, 1f));
        }
    }
}
