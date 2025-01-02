using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_View
{
    GameObject _markus;
    GameObject _ferana;

    public P_View(GameObject markusRef, GameObject feranaRef)
    {
        _markus = markusRef;
        _ferana = feranaRef;
    }

    public void SwitchToMarkus()
    {
        _markus.SetActive(true);
        _ferana.SetActive(false);
    }
    public void SwitchToFerana()
    {
        _markus.SetActive(false);
        _ferana.SetActive(true);
    }
}
