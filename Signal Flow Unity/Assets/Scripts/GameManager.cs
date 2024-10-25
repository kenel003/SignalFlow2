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
    public GenericControl[] channel1, channel2, channel3, channel4, channel5_6, channel7_8, channel9_10, channel11_12, channelFX, channelSub1_2, channelMain, incorrectControls;
    // Variables for Text
    public GameObject selected, titleScreen, instructionScreen, finishText;
    public TextMeshProUGUI selectText, titleText;
    public Button start1Button, restartButton, start2Button;
    public TextMeshProUGUI descriptionText, gameDoneText, uIDText;
    private bool followSlider = false, gameOver = false;
    public bool isQuizMode = false;
    public TextMeshProUGUI incorrectItemsText;
    public bool guiUp = true;
    private GameObject selectedCable;
    [SerializeField] private GenericControl outputCableL;
    [SerializeField] private GenericControl outputCableR;
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

        if (channel1[2].controlValue > 0f )//GAIN IS ABOVE 0, else turns off all
        {
            ledNodes[0].Toggle(true); //Turns on GAIN and EQ light
            
            if (!channel1[10].toggled)//MUTE is off, turns on light next to fader
            {
                ledNodes[1].Toggle(true); //Mute light on

                if (channel1[14].controlValue > 0f) //FADER IS ABOVE 0
                            {
                                ledNodes[2].Toggle(true); //Fader light on

                                if(channel1[12].toggled) //LR Bus enabled
                                {
                                    ledNodes[3].Toggle(true); //LR Bus light on

                                    if(channelMain[3].controlValue > 0f && !channelMain[2].toggled) //MAIN fader is above 0 and not muted
                                    {
                        
                                        ledNodes[4].Toggle(true);
                                        ledNodes[5].Toggle(true);
                                        ledNodes[6].Toggle(true);
                                        gameOver = true;
                                        StartCoroutine(GameOver());
                                    }
                                    else //main slider is 0 or muted
                                    {
                        
                                        ledNodes[4].Toggle(false);
                                        ledNodes[5].Toggle(false);
                                        ledNodes[6].Toggle(false);
                                    }

                                } //LR Bus enabled
                                else //LR Bus disabled, turns off all lights after
                                {
                                    ledNodes[3].Toggle(false);
                                    ledNodes[4].Toggle(false);
                                    ledNodes[5].Toggle(false);
                                    ledNodes[6].Toggle(false);
                                }//LR Bus disabled, turns off all lights after
                            }
                else //FADER is 0
                {
                    ledNodes[2].Toggle(false);//fader light and all after is off
                    ledNodes[3].Toggle(false);
                    ledNodes[4].Toggle(false);
                    ledNodes[5].Toggle(false);
                    ledNodes[6].Toggle(false);
                } 
            }//MUTE is off, turns on light next to fader
            
            
            else//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
            {
                ledNodes[1].Toggle(false);
                ledNodes[2].Toggle(false);
                ledNodes[3].Toggle(false);
                ledNodes[4].Toggle(false);
                ledNodes[5].Toggle(false);
                ledNodes[6].Toggle(false);
            }//MUTE is ON, turn off all lights except GAIN and EQ light (index 0)
        }
        else //turns off all lights when GAIN is not above 0
        {
            ledNodes[0].Toggle(false);
            ledNodes[1].Toggle(false);
            ledNodes[2].Toggle(false);
            ledNodes[3].Toggle(false);
            ledNodes[4].Toggle(false);
            ledNodes[5].Toggle(false);
            ledNodes[6].Toggle(false);
        }

    }
    public void SelectCable(GameObject cable)
    {
        selectedCable = cable;
    }

    public void MoveCable(Vector3 interfaceLocation, GameObject interfacePlug)
    {
        selectionParticle.SetActive(false);
        selectedCable.transform.position = new Vector3(interfaceLocation.x, selectedCable.transform.position.y, interfaceLocation.z);
        selectionParticle.transform.localPosition = new Vector3(mouseSelection.clickedObject.transform.position.x, 0.4f, mouseSelection.clickedObject.transform.position.z);
        selectionParticle.SetActive(true);
        selectedCable.GetComponent<GenericControl>().pluggedInto = interfacePlug;
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
        incorrectItemsText.text = " You used " + wrongControlsUsed + " incorrect controls.";
        yield return new WaitForSeconds(2);
        guiUp = true;
        finishText.gameObject.SetActive(true);
        selected.gameObject.SetActive(false);

    }//End of game text when game ends. 

    public void Start1()
    {
        guiUp = true;
        titleScreen.gameObject.SetActive(false);
        instructionScreen.gameObject.SetActive(true);
    }//Goes through starts screen and instruction screen. On Button press.

    public void Start2()
    {
        instructionScreen.gameObject.SetActive(false);
        guiUp = false;
        selected.gameObject.SetActive(true);
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
