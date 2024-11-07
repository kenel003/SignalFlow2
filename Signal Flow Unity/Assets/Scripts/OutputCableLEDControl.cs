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

        if (!gameManager.phaseTwo && correctOutputs)
        {
            if (inputTrainingLED[0].GetToggled()) //Channel 1 training LED is on
            {
                if (inputTrainingLED[1].GetToggled()) //BOTH channels are sending, line should be mixed
                {
                    rend.material = mixedMaterial;
                    rend.material.SetTextureOffset("_MainTex", new Vector2(0, Time.timeSinceLevelLoad * -1));
                    SetIntensity(Mathf.Abs(Mathf.Sin(Mathf.Sin(intensityWiggle * Time.time)) * intensityWiggleRange) + 1);
                    UpdateColor(rend.material.color);
                    Debug.Log("MIXED");
                }
                else
                {
                    rend.material = channel1OnMaterial;
                    Debug.Log("CHANNEL 1");
                }
            }
            else if (inputTrainingLED[1].GetToggled()) //Channel 2 is on without Channel 1
            {
                rend.material = channel2OnMaterial;

                Debug.Log("CHANNEL 2");
            }
            else // no channels, should be non-illuminated
            {
                Debug.Log("NOTHING");
                rend.material = offMaterial;
            }
        }
        else if (correctOutputs) //phase two
        {
            if(inputTrainingLEDPhaseTwo[0].GetToggled() && inputTrainingLEDPhaseTwo[1].GetToggled())
            {
                rend.material = mixedMaterial;
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

}

