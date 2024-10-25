using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggledLED : MonoBehaviour
{
    public float intensity = 2;
    private Renderer rend;
    private Color finalColor, originalColor;
    private Material originalMat;
    public bool toggledOnStart = false;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        
    }
    public void Toggled(bool toggled)
    {
        if (toggled)
        {
            originalMat = rend.material;
            finalColor = originalColor * Mathf.LinearToGammaSpace(intensity);
            originalMat.SetColor("_EmissionColor", finalColor);
        }
        else
        {
            originalMat = rend.material;
            finalColor = Color.clear * Mathf.LinearToGammaSpace(intensity);
            originalMat.SetColor("_EmissionColor", finalColor);
        }
    }
}
