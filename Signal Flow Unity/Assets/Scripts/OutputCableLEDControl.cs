using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCableLEDControl : MonoBehaviour
{
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material channel1OnMaterial;
    [SerializeField] private Material channel2OnMaterial;
    [SerializeField] private Material channel3OnMaterial;
    [SerializeField] private Material channel4OnMaterial;
    [SerializeField] private Material channel5_6OnMaterial;
    [SerializeField] private Material channel7_8OnMaterial;
    [SerializeField] private Material channel9_10OnMaterial;
    [SerializeField] private Material channel11_12OnMaterial;
    [SerializeField] private Material mixedMaterial; //research mixing materials or whether this might be a good use of GPT to avoid repetitive code wasting time.

    [SerializeField] private LedControl[] inputTrainingLED;
    [SerializeField] private LedControl[] inputTrainingLEDPhaseTwo;

    bool correctOutputs;
    public float intensityWiggle = 3, intensityWiggleRange = 4;
    private float intensity;
    private Renderer rend;
    private GameManager gameManager;
    public bool isCorrect;

    private void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Update()
    {
        if(gameManager.correctL && gameManager.correctR)
        {
            correctOutputs = true;
        }
        else
        {
            correctOutputs = false;
        }
        //NEED BASIC isCorrect flag
        if (!gameManager.phaseTwo )
        {
            if (inputTrainingLED[0].GetToggled()) //Channel 1 training LED is on
            {
                if (inputTrainingLED[1].GetToggled() && isCorrect) //BOTH channels are sending, line should be mixed
                {
                    rend.material = mixedMaterial;
                    rend.material.SetTextureOffset("_MainTex", new Vector2(0, Time.timeSinceLevelLoad * -1));
                    SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                    UpdateColor(rend.material.color);
                    
                }
                else if(isCorrect)
                {
                    rend.material = channel1OnMaterial;
                    
                }
            }
            else if (inputTrainingLED[1].GetToggled() && isCorrect) //Channel 2 is on without Channel 1
            {
                rend.material = channel2OnMaterial;

                
            }

            if(!isCorrect) // no channels, should be non-illuminated
            {
                
                rend.material = offMaterial;
            }
        }
        else if (correctOutputs) //phase two
        {
            if(inputTrainingLEDPhaseTwo[0].GetToggled() && inputTrainingLEDPhaseTwo[1].GetToggled())
            {
                if(inputTrainingLEDPhaseTwo[0].currentlyMixed && inputTrainingLEDPhaseTwo[0].currentlyMixed)
                {
                    rend.material = mixedMaterial;
                    rend.material.SetTextureOffset("_MainTex", new Vector2(0, Time.timeSinceLevelLoad * -1));
                    SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                }
                else if (!inputTrainingLEDPhaseTwo[0].currentlyMixed || !inputTrainingLEDPhaseTwo[0].currentlyMixed)
                {
                    rend.material = channel3OnMaterial;
                    rend.material.SetTextureOffset("_MainTex", new Vector2(0, Time.timeSinceLevelLoad * -1));
                    SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                }
            }
            
            else
            {
                rend.material = offMaterial;
            }
        }
        

        
    }

    public void SetIntensity(float value)
    {
        intensity = value;
    }
    public void UpdateColor(Color setColor) //sets normal non-mixed color
    {
        Material mat = rend.material;

   
            var finalColor = mat.color* Mathf.LinearToGammaSpace(intensity);
            mat.SetColor("_EmissionColor", Color.white);
    }

    public void setCorrect(bool correctOrNot)
    {
        isCorrect = correctOrNot;
    }
}

