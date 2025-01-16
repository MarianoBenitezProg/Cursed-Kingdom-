using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MaterialTintColor : MonoBehaviour
{
    private Material _Material;
    private Color materialColor;
    private float tintFadeSpeed;

    private void Awake()
    {
        materialColor = new Color(0, 0, 0, 1f);
        setMaterial(GetComponent<SpriteRenderer>().material);
        tintFadeSpeed = 6f;
    }

    private void Update()
    {
        if(materialColor.a > 0)
        {
            materialColor.a = Mathf.Clamp01(materialColor.a - tintFadeSpeed * Time.deltaTime);
            _Material.SetColor("_Tint", materialColor);
        }
    }

    public void setMaterial(Material material)
    {
        this._Material = material;
    }

    public void SetTintColor(Color color)
    {
        materialColor = color;
        _Material.SetColor("_Tint", materialColor);
    }
    public void SetTintFadeSpeed(float tintFadeSpeed)
    {
        this.tintFadeSpeed = tintFadeSpeed;
    }
}
