using UnityEngine;

public class LedControl : MonoBehaviour
{

    Color finalColor;
    [SerializeField]private bool isOnByDefault = false;
    private Renderer rend;
    private float intensity = 0;
    [SerializeField] private bool toggledOn = false;
    public bool currentlyMixed = false;
    [SerializeField] private float intensityWiggle = 0;
    [SerializeField] private float intensityWiggleRange = 0;
    [SerializeField] private Texture originalTexture;
    [SerializeField] private Texture mixedColorTexture;
    [SerializeField] private float flowSpeed = 10;
    [SerializeField] private Color originalColor;
    public bool isOutputCable;
    public LedControl[] outputConnectedToLEDs;
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_Color");
        if (isOnByDefault)
        {
            toggledOn = true;
            SetIntensity(1);
            UpdateColorOriginal(rend.material.color);

        }

    }

    // Update is called once per frame
    void Update()
    {

        if(!isOutputCable)
        {
            if (toggledOn && intensityWiggle != 0)
            {
                SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                UpdateColorOriginal(rend.material.color);
            }
            else if (toggledOn && intensityWiggle == 0)
            {
                SetIntensity(1f);
                UpdateColorOriginal(rend.material.color);
            }
            if (currentlyMixed)
            {
                SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                rend.material.SetTextureOffset("_MainTex", new Vector2(0, Time.timeSinceLevelLoad));
            }
            else
            {
                UpdateColorOriginal(originalColor);
            }
        }
        else
        {

        }
       

        
    }

    public bool GetToggled()
    {
        return toggledOn;
    }
    public void Toggle(bool toggleValue)
    {
        toggledOn = toggleValue;
        if (!toggleValue)
        {
            UpdateColorOriginal(Color.clear);
            UpdateColor();
        }
    }
   
    public void SetIntensity(float value)
    {
        intensity = value;
    }

    public void UpdateColorOriginal(Color setColor) //sets normal non-mixed color
    {
        Material mat = rend.material;
        
        if (!currentlyMixed && toggledOn)
        {
            finalColor = setColor * Mathf.LinearToGammaSpace(intensity);
            mat.SetTexture("_EmissionMap", originalTexture);
            mat.SetTexture("_MainTex", originalTexture);
            mat.SetColor("_EmissionColor", finalColor);
        }
        else if (currentlyMixed && toggledOn)
        {
            finalColor = Color.white * Mathf.LinearToGammaSpace(intensity);
            mat.SetTexture("_MainTex", mixedColorTexture);
            mat.SetTexture("_EmissionMap", mixedColorTexture);
            mat.SetColor("_EmissionColor", finalColor);
        }
        else
        {
            UpdateColor();
        }

    }
    
    public void UpdateColor() //reset
    {
        Material mat = rend.material;
        mat.SetTexture("_EmissionMap", null);
        finalColor = Color.black * Mathf.LinearToGammaSpace(0);
        mat.SetColor("_EmissionColor", finalColor);
    }
}
