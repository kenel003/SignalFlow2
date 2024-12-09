using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Game Manager provides text for start, end, and restart of game.
    //Game Manager also determines the end of the game.

    //Selector script
    private MouseSelectionController mouseSelection;
    public GameObject selectionParticle;
    //Dial Scripts
    public LedControl[] ledNodes;
    public GenericControl[] channel1, channel2, channel3, channel4, channel5_6, channel7_8, channel9_10, channel11_12, channelFX, channelSub1_2, channelMain, incorrectControls, phaseOneIncorrectControls;
    // Variables for Text
    public GameObject selected, titleScreen, instructionScreen, finishText, part2Instructions;
    public TextMeshProUGUI selectText, titleText;
    public Button start1Button, restartButton, start2Button;
    public TextMeshProUGUI descriptionText, gameDoneText, uIDText;
    private bool followSlider = false, gameOver = false;
    public bool phaseTwo;
    public bool isQuizMode = false;
    public TextMeshProUGUI incorrectItemsText, phaseOneIncorrectItemsText;
    public bool guiUp = true, channel1InputComplete, channel2InputComplete, channel1Sending, channel2Sending;
    public bool correctL, correctR;
    private GameObject selectedCable;
    [SerializeField] private LedControl[] mixedChannelLEDs;
    [SerializeField] private LedControl[] mixedChannelLEDsPhase2;
    [SerializeField] private LedControl[] mixedOutputCableLEDs;
    [SerializeField] private GenericControl outputCableL;
    [SerializeField] private GenericControl outputCableR;
    [SerializeField] private GameObject[] correctOutputs;
    public Texture mixedColorTexture;
    public Toggle iHaveRead1, iHaveRead2;
    void Start()
    {
        
        mouseSelection = GameObject.Find("Main Camera").GetComponent<MouseSelectionController>();
        uIDText.text = "UID: " + Random.Range(0, 9999999999999999999);
        if (SceneManager.GetActiveScene().name == "SignalFlowLevel2Quiz")
        {
            isQuizMode = true;
            guiUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {


        //Tells User what Game Object is selected.
        if ((mouseSelection.clickedObject.CompareTag("Dial") || mouseSelection.clickedObject.CompareTag("Slider") || mouseSelection.clickedObject.CompareTag("Toggle") || mouseSelection.clickedObject.CompareTag("StereoCable")) && mouseSelection.newClick&& !gameOver)
        {
            mouseSelection.newClick = false;
            selectText.text = "Currently Selected: " + mouseSelection.clickedObject.name;
            selectionParticle.SetActive(false);
            selectionParticle.transform.localPosition = new Vector3(mouseSelection.clickedObject.transform.position.x, 0.4f , mouseSelection.clickedObject.transform.position.z);
            selectionParticle.SetActive(true);
            if (mouseSelection.clickedObject.CompareTag("Slider"))
            {
                followSlider = true;
            }
            else
            {
                followSlider = false;
            }
            
        }

        if ( followSlider)
        {
            selectionParticle.transform.localPosition = new Vector3(mouseSelection.clickedObject.transform.position.x, 0.4f, mouseSelection.clickedObject.transform.position.z);
        }

        //CHANNEL 1 INPUT SEQUENCE
        if (channel1[2].controlValue > 0f )//GAIN IS ABOVE 0, else turns off all
        {
            ledNodes[0].Toggle(true); //Turns on GAIN and EQ light
            
            if (!channel1[10].toggled)//MUTE is off, turns on light next to fader
            {
                ledNodes[1].Toggle(true); //Mute light on

                if (channel1[14].controlValue > 0f) //FADER IS ABOVE 0
                            {
                                ledNodes[2].Toggle(true); //Fader light on

                                if(channel1[11].toggled) //SUB 1/2 Bus enabled
                                {
                                    ledNodes[3].Toggle(true); //SUB 1/2 Bus LED enabled
                                    channel1Sending = true;
                                    if(channelSub1_2[3].controlValue > 0f) //SUB fader is above 0 
                                    {
                        
                                        ledNodes[4].Toggle(true);

                                        if (!phaseTwo) //if still phase one, light up the output cables if correct 
                                        {
                                            if (outputCableL.pluggedInto == correctOutputs[0] || outputCableL.pluggedInto == correctOutputs[1])
                                            {
                                                //ledNodes[5].Toggle(true);
                                                correctL = true;
                                            }
                                            if ((outputCableR.pluggedInto == correctOutputs[0] || outputCableR.pluggedInto == correctOutputs[1]))
                                            {
                                                //ledNodes[6].Toggle(true);
                                                correctR = true;
                                            }
                                            else
                                            {
                                                //ledNodes[6].Toggle(false);
                                                correctR = false;
                                            }
                                            if (correctL && correctR)
                                            {
                                                channel1InputComplete = true;
                                                //StartCoroutine(StartPartTwo()); //this needs 
                                            }
                                            else
                                            {
                                                channel1InputComplete = false;
                                            }
                                        }
                                        else if (channelSub1_2[2].toggled)// send is toggled
                                        {
                                            Debug.Log("SUB TO MAIN TOGGLED");
                                            if (channel1Sending && channel2Sending)
                                            {
                                                Debug.Log("Main bottom on, mixed");
                                                ledNodes[7].currentlyMixed = true;
                                                ledNodes[7].Toggle(true);
                                            }
                                            else if (!channel1Sending && channel2Sending)
                                            {
                                                Debug.Log("Main bottom on, single");
                                                ledNodes[7].currentlyMixed = false;
                                                ledNodes[7].Toggle(true);
                                            }
                                            else if (channel1Sending && !channel2Sending)
                                            {
                                                Debug.Log("Main bottom on, single");
                                                ledNodes[7].currentlyMixed = false;
                                                ledNodes[7].Toggle(true);
                                            }
                                            else
                                            {
                                                Debug.Log("Main bottom OFF");
                                                ledNodes[7].Toggle(false);
                                            }
                                            if (channelMain[3].controlValue > 0)
                                            {
                                                if (channel1Sending && channel2Sending)
                                                {
                                                    Debug.Log("Main TOP on, mixed");
                                                    ledNodes[8].currentlyMixed = true;
                                                    ledNodes[8].Toggle(true);
                                                }
                                                else if (!channel1Sending && channel2Sending)
                                                {
                                                    Debug.Log("Main TOP on, single");
                                                    ledNodes[8].currentlyMixed = false;
                                                    
                                                    ledNodes[8].Toggle(true);
                                                }
                                                else if (channel1Sending && !channel2Sending)
                                                {
                                                    Debug.Log("Main TOP on, single");
                                                    ledNodes[8].currentlyMixed = false;
                                                    ledNodes[8].Toggle(true);
                                                }
                                                else
                                                {
                                                    Debug.Log("Main TOP OFF");
                                                    ledNodes[8].Toggle(false);
                                                }
                                            }
                                            else ledNodes[8].Toggle(false);

                                            if (phaseTwo && channelMain[3].controlValue > 0 && !channelMain[2].toggled && correctR && correctL && channel1Sending && channel2Sending)
                                            {

                                                gameOver = true;
                                                StartCoroutine(GameOver());
                                            }
                                        }
                                        else 
                                        {
                                            ledNodes[7].Toggle(false);
                                            ledNodes[8].Toggle(false);
                                        }

                                        
                                    }


                                    else //SUB slider is 0 or muted
                                    {
                                        //channel1Sending = false;
                                        ledNodes[4].Toggle(false);
                                        //ledNodes[7].Toggle(false);
                                        //ledNodes[8].Toggle(false);
                                        channel1InputComplete = false;
                                        // ledNodes[5].Toggle(false);
                                        // ledNodes[6].Toggle(false);
                        }

                                } //LR Bus enabled
                                else //LR Bus disabled, turns off all lights after
                                {
                                    channel1Sending = false;
                                    //ledNodes[7].Toggle(false);
                                    //ledNodes[8].Toggle(false);
                                    ledNodes[3].Toggle(false);
                                    ledNodes[4].Toggle(false);
                                    channel1InputComplete = false;
                        // ledNodes[5].Toggle(false);
                        //ledNodes[6].Toggle(false);
                    }//LR Bus disabled, turns off all lights after
                            }
                else //FADER is 0
                {
                    channel1Sending = false;
                    //ledNodes[7].Toggle(false);
                    //ledNodes[8].Toggle(false);
                    ledNodes[2].Toggle(false);//fader light and all after is off
                    ledNodes[3].Toggle(false);
                    ledNodes[4].Toggle(false);
                    channel1InputComplete = false;
                    //ledNodes[5].Toggle(false);
                    //ledNodes[6].Toggle(false);
                } 
            }//MUTE is off, turns on light next to fader
            
            
            else//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
            {
                channel1Sending = false;
                ledNodes[1].Toggle(false);
                ledNodes[2].Toggle(false);
                ledNodes[3].Toggle(false);
                ledNodes[4].Toggle(false);
                //ledNodes[7].Toggle(false);
                //ledNodes[8].Toggle(false);
                channel1InputComplete = false;
                //ledNodes[5].Toggle(false);
                //ledNodes[6].Toggle(false);
            }//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
        }
        else //turns off all lights when GAIN is not above 0
        {
            channel1Sending = false;
            ledNodes[0].Toggle(false);
            ledNodes[1].Toggle(false);
            ledNodes[2].Toggle(false);
            ledNodes[3].Toggle(false);
            ledNodes[4].Toggle(false);
            //ledNodes[7].Toggle(false);
            //ledNodes[8].Toggle(false);
            channel1InputComplete = false;
            //ledNodes[5].Toggle(false);
            //ledNodes[6].Toggle(false);
        }

        //CHANNEL 2 INPUT SEQUENCE
        if (channel2[2].controlValue > 0f)//GAIN IS ABOVE 0, else turns off all
        {
            ledNodes[9].Toggle(true); //Turns on GAIN and EQ light

            if (!channel2[10].toggled)//MUTE is off, turns on light next to fader
            {
                ledNodes[10].Toggle(true); //Mute LED section on

                if (channel2[14].controlValue > 0f) //FADER IS ABOVE 0
                {
                    ledNodes[11].Toggle(true); //Fader light on

                    if (channel2[11].toggled) //SUB 1/2 Bus enabled
                    {
                        ledNodes[12].Toggle(true); //SUB 1/2 Bus LED enabled
                        channel2Sending = true;

                        if (channelSub1_2[3].controlValue > 0f) //SUB fader is above 0 and neither toggle for LR or phones is toggled
                        {

                            ledNodes[13].Toggle(true);

                            if (!phaseTwo) //if still phase one, light up the output cables if correct 
                            {
                                if (correctL)
                                {
                                    outputCableL.toggled = true;
                                }
                                else
                                {
                                    outputCableL.toggled = false;
                                    channel2InputComplete = false;
                                }

                                if (correctR)
                                {
                                    outputCableR.toggled = true;
                                }
                                else
                                {
                                    outputCableR.toggled = false;
                                    channel2InputComplete = false;
                                }
                                //Check if both are done and win condition for part one is done
                                if (correctL && correctR)
                                {
                                    channel2InputComplete = true;
                                }
                                else
                                {
                                    channel2InputComplete = false;
                                }
                            }

                            //////
                            else if (channelSub1_2[2].toggled)// send is toggled
                            {
                                Debug.Log("SUB TO MAIN TOGGLED");
                                if (channel1Sending && channel2Sending)
                                {
                                    Debug.Log("Main bottom on, mixed");
                                    ledNodes[7].currentlyMixed = true;
                                    ledNodes[7].Toggle(true);
                                }
                                else if (!channel1Sending && channel2Sending)
                                {
                                    Debug.Log("Main bottom on, single");
                                    ledNodes[7].currentlyMixed = false;
                                    ledNodes[7].Toggle(true);
                                }
                                else if (channel1Sending && !channel2Sending)
                                {
                                    Debug.Log("Main bottom on, single");
                                    ledNodes[7].currentlyMixed = false;
                                    ledNodes[7].Toggle(true);
                                }
                                else
                                {
                                    Debug.Log("Main bottom OFF");
                                    ledNodes[7].Toggle(false);
                                }
                                if (channelMain[3].controlValue > 0)
                                {
                                    if (channel1Sending && channel2Sending)
                                    {
                                        Debug.Log("Main TOP on, mixed");
                                        ledNodes[8].currentlyMixed = true;
                                        ledNodes[8].Toggle(true);
                                    }
                                    else if (!channel1Sending && channel2Sending)
                                    {
                                        Debug.Log("Main TOP on, single");
                                        ledNodes[8].currentlyMixed = false;

                                        ledNodes[8].Toggle(true);
                                    }
                                    else if (channel1Sending && !channel2Sending)
                                    {
                                        Debug.Log("Main TOP on, single");
                                        ledNodes[8].currentlyMixed = false;
                                        ledNodes[8].Toggle(true);
                                    }
                                    else
                                    {
                                        Debug.Log("Main TOP OFF");
                                        ledNodes[8].Toggle(false);
                                    }
                                }
                                else ledNodes[8].Toggle(false);
                                /////
                            }
                        }


                        else //main slider is 0 or muted
                        {
                            //channel2Sending = false;
                            channel2InputComplete = false;
                            ledNodes[13].Toggle(false);
                            ledNodes[7].Toggle(false);
                            ledNodes[8].Toggle(false);
                        }

                    } //SUB1_2 Bus enabled
                    else //SUB1_2 Bus disabled, turns off all lights after
                    {
                        channel2Sending = false;
                        MixChannelsToggle(false);
                        channel2InputComplete = false;
                        ledNodes[13].Toggle(false);
                        ledNodes[12].Toggle(false);
                        ledNodes[7].Toggle(false);
                        ledNodes[8].Toggle(false);
                    }//LR Bus disabled, turns off all lights after
                }
                else //FADER is 0
                {
                    channel2Sending = false;
                    MixChannelsToggle(false);
                    channel2InputComplete = false;
                    ledNodes[11].Toggle(false);//fader light and all after is off
                    ledNodes[13].Toggle(false);
                    ledNodes[12].Toggle(false);
                    //ledNodes[7].Toggle(false);
                    //ledNodes[8].Toggle(false);
                }
            }//MUTE is off, turns on light next to fader


            else//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
            {
                channel2Sending = false;
                MixChannelsToggle(false);
                ledNodes[10].Toggle(false);
                ledNodes[11].Toggle(false);
                channel2InputComplete = false;
                ledNodes[13].Toggle(false);
                //ledNodes[7].Toggle(false);
                //ledNodes[8].Toggle(false);
            }//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
        }
        else //turns off all lights when GAIN is not above 0
        {
            channel2Sending = false;
            channel2InputComplete = false;
            MixChannelsToggle(false);
            ledNodes[9].Toggle(false);
            ledNodes[10].Toggle(false);
            ledNodes[11].Toggle(false);
            ledNodes[12].Toggle(false);
            //ledNodes[7].Toggle(false);
            //ledNodes[8].Toggle(false);
        }

        //OUTPUT CABLE CORRECT OR NOT
        if (!phaseTwo) //if still phase one, light up the output cables if correct 
        {
            if (outputCableL.pluggedInto == correctOutputs[0] || outputCableL.pluggedInto == correctOutputs[1])
            {
                outputCableL.GetComponentInChildren<OutputCableLEDControl>().setCorrect(true);
            }
            else
            {
                outputCableL.GetComponentInChildren<OutputCableLEDControl>().setCorrect(false);
            }
            if ((outputCableR.pluggedInto == correctOutputs[0] || outputCableR.pluggedInto == correctOutputs[1]))
            {
                outputCableR.GetComponentInChildren<OutputCableLEDControl>().setCorrect(true);
            }
            else
            {
                outputCableR.GetComponentInChildren<OutputCableLEDControl>().setCorrect(false);
            }


            
        }
        //MIXED SIGNAL SENDING
        if (channel1Sending && channel2Sending)
        {
            MixChannelsToggle(true);
        }
        else
        {
            MixChannelsToggle(false);
        }

        //CHECK FOR PART 1 WIN CONDITION
        if(channel1InputComplete && channel2InputComplete)
        {
            channel1InputComplete = false;
            channel2InputComplete = false;
            StartCoroutine(StartPartTwo());
        }
    }
    public void SelectCable(GameObject cable)
    {
        selectedCable = cable;
    }

    public void MoveCable(Vector3 interfaceLocation, GameObject interfacePlug)
    {
        if (!phaseTwo)
        {
            selectionParticle.SetActive(false);
            selectedCable.transform.position = new Vector3(interfaceLocation.x, selectedCable.transform.position.y, interfaceLocation.z);
            selectionParticle.transform.localPosition = new Vector3(mouseSelection.clickedObject.transform.position.x, 0.4f, mouseSelection.clickedObject.transform.position.z);
            selectionParticle.SetActive(true);
            selectedCable.GetComponent<GenericControl>().pluggedInto = interfacePlug;
        }
        
    }

    void MixChannelsToggle(bool mix)
    {
        if (mix)
        {
            foreach (LedControl led in mixedChannelLEDs)
            {
                if (led.GetToggled()) 
                {
                    led.currentlyMixed = true; 
                }

            }
        }
        else
        {
            foreach (LedControl led in mixedChannelLEDs)
            {
                led.currentlyMixed = false;
            }
        }
    }
    
    IEnumerator StartPartTwo()
    {
        yield return new WaitForSeconds(1);
        guiUp = true;
        phaseTwo = true;
        part2Instructions.SetActive(true);
        int wrongControlsUsed = 0;
        foreach (GenericControl control in phaseOneIncorrectControls)
        {
            if (control.controlValue != 0 && !control.ToggleLEDOnAtStart)
            {
                wrongControlsUsed++;
            }
            else if (control.ToggleLEDOnAtStart && control.controlValue == 0)
            {
                wrongControlsUsed++;
            }
        }
        phaseOneIncorrectItemsText.text = "" + wrongControlsUsed;
        outputCableL.transform.localPosition = new Vector3(correctOutputs[2].transform.position.x, 0.4f, correctOutputs[2].transform.position.z);
        outputCableR.transform.localPosition = new Vector3(correctOutputs[3].transform.position.x, 0.4f, correctOutputs[3].transform.position.z);
    }

    public void ClosePart2Instructions()
    {
        
            part2Instructions.SetActive(false);
        if (isQuizMode)
        {
            descriptionText.text = "Route your Submix to the L-R Main Mix.";
        }
        else
        {
            descriptionText.text = "Level 2b: Assign Your Sub Mix to the Main Mix \r\n \r\n Now that you have learned how to assign signal to the Sub 1 & 2 outputs, let’s use a “Sub mix” for one of it’s most common applications: creating a sub mix that fits within our main mix. \r\n\r\n This is often used when making a mix of a group of instruments, like a drum set.You can mix the drum set down to just one fader(Main SUB 1-2 fader) and feed that one fader to the Main Mix.You can then use the one sub mix fader to control the volume of all the assigned channels rather than turn up and down all the faders from those channels. \r\n\r\n Notice the Main SUB 1-2 Fader has a bus assign that feeds the L-R Mix.When using a sub mix in your main mix. Make sure the channels you want in your sub mix are ONLY feeding the Main SUB 1-2 fader and not the L-R Bus.";
        }
            guiUp = false;
            selectText.text = "Currently Selected: NONE";
            selectionParticle.transform.position = new Vector3(1000, selectionParticle.transform.position.y, selectionParticle.transform.position.z);
        
        
    }
    IEnumerator GameOver()
    {
        guiUp = true;
        int wrongControlsUsed = 0;
        foreach (GenericControl control in incorrectControls)
        {
            if (control.controlValue != 0 && !control.ToggleLEDOnAtStart)
            {
                wrongControlsUsed++;
            }
            else if (control.ToggleLEDOnAtStart && control.controlValue == 0)
            {
                wrongControlsUsed++;
            }
        }
        incorrectItemsText.text = "" + wrongControlsUsed ;
        yield return new WaitForSeconds(2);
        guiUp = true;
        finishText.gameObject.SetActive(true);
        selected.gameObject.SetActive(false);

    }//End of game text when game ends. 

    public void Start1()
    {
        guiUp = false;
        titleScreen.gameObject.SetActive(false);
      
    }//Goes through starts screen and instruction screen. On Button press.

    public void Start2()
    {
        
            guiUp = false;
           // selected.gameObject.SetActive(true);
       

    }//Ends instruction screen. Starts selected object text. On Button press.

    public void QuizModeLevel()
    {
        SceneManager.LoadScene("SignalFlowLevel2Quiz");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("SignalFlowLevel2Practice");
    }//If the restart button is pressed, restart the game.
}
