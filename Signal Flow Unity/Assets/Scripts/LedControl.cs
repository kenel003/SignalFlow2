using UnityEngine;

public class LedControl : MonoBehaviour
{

    Color finalColor;
    [SerializeField]private bool isOnByDefault = false;
    private Renderer rend;
    private float intensity = 0;
    private bool toggledOn = false;
    [SerializeField] private float intensityWiggle = 0;
    [SerializeField] private float intensityWiggleRange = 0;
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (isOnByDefault)
        {
            toggledOn = true;
            SetIntensity(1);
            UpdateColor(rend.material.color);

        }

    }

    // Update is called once per frame
    void Update()
    {
       if(toggledOn && intensityWiggle != 0)
        {
            SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time))*intensityWiggleRange ) + 1);
            UpdateColor(rend.material.color);
        }
       else if (toggledOn && intensityWiggle == 0)
        {
            SetIntensity(1f);
            UpdateColor(rend.material.color);
        }
    }

    
    public void Toggle(bool toggleValue)
    {
        toggledOn = toggleValue;
        if (!toggleValue)
        {
            UpdateColor(Color.clear);
        }
    }
   
    public void SetIntensity(float value)
    {
        intensity = value;
    }

    public void UpdateColor(Color setColor)
    {
        Material mat = rend.material;
        finalColor = setColor * Mathf.LinearToGammaSpace(intensity);
        mat.SetColor("_EmissionColor", finalColor);
    }
}
