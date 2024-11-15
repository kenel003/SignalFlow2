using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericControl : MonoBehaviour
{
    
    private MouseSelectionController mouseSelection;
    [SerializeField] private string controlName;
    [SerializeField] private float dialMin = 0f;
    [SerializeField] private float dialMax = 359f;
    private float dialSpeed = 90f;
    [SerializeField] private float toggleUpHeight = 0.1f;
    [SerializeField] private float toggleDownHeight = 0.015f;
    [SerializeField] private float sliderMin = 0f;
    [SerializeField] private float sliderMax = 1.25f;
    [SerializeField] private float sliderSpeed = .15f;
    private float dialRot = 0;
    public float sliderValue = 0f, sliderPercent = 0f, dialPercent = 0f;
    public bool toggled = false, inputCooldown = false;
    public float controlValue = 0;
    public bool useToggleLED = false, ToggleLEDOnAtStart= false;
    public ToggledLED connectedToggleLED;
    public GameObject pluggedInto;

    void Start()
    {
        mouseSelection = GameObject.Find("Main Camera").GetComponent<MouseSelectionController>();
        sliderValue = sliderMin;
        if (useToggleLED && ToggleLEDOnAtStart)
        {
            connectedToggleLED.Toggled(true);
        }
    }

    IEnumerator InputCooldown()
    {
        yield return new WaitForSeconds(1);
        inputCooldown = false;
    }
    // Update is called once per frame
    void Update()
    {
        //If this control is connected to a toggled LED light, turn it on if the control is at something other than zero.
        if(controlValue > 0 && useToggleLED)
        {
            connectedToggleLED.Toggled(true);
        }
        else if (useToggleLED)
        {
            connectedToggleLED.Toggled(false);
        }

        if (mouseSelection.clickedObject == gameObject )
        {
            if (mouseSelection.clickedObject.CompareTag("Dial"))
            {
                
                if (dialRot < dialMin)
                {
                    dialRot = dialMin;
                }
                if (dialRot > dialMax)
                {
                    dialRot = dialMax;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    inputCooldown = true;
                    StartCoroutine(InputCooldown());
                    dialRot = 0;
                    transform.localEulerAngles = new Vector3(mouseSelection.clickedObject.transform.localEulerAngles.x, dialRot, mouseSelection.clickedObject.transform.localEulerAngles.z);
                    controlValue = 0;
                }
                if (Input.GetAxis("Horizontal") != 0 &&(dialRot >= dialMin && dialRot <= dialMax) && !inputCooldown)
                {
                    
                    controlValue = Input.GetAxis("Horizontal");
                    if (controlValue < 0)
                    {
                        dialRot -= dialSpeed * Time.deltaTime;
                    }
                    if (controlValue > 0)
                    {
                        dialRot += dialSpeed * Time.deltaTime;
                    }
                    //Sets dial rotation based on dialRot
                    transform.localEulerAngles = new Vector3(mouseSelection.clickedObject.transform.localEulerAngles.x, dialRot, mouseSelection.clickedObject.transform.localEulerAngles.z);
                    controlValue = dialRot / (dialMax - dialMin);
                }/*If the dialRot is inside the bounds, the user can change it with the arrow keys
                  * The dialRot is then used to set a new Y axis EulerAngle*/

                
            }//If the game object is a dial, then it can move.

            else if (mouseSelection.clickedObject.CompareTag("Slider"))
            {
                if (sliderValue < sliderMin)
                {
                    sliderValue = sliderMin;
                }
                if (sliderValue > sliderMax)
                {
                    sliderValue = sliderMax;
                }
                if (sliderValue >= sliderMin && sliderValue <= sliderMax)
                {
                    controlValue = Input.GetAxis("Vertical");
                    if ( controlValue > 0)
                    {
                        if (sliderMax - sliderValue >= sliderSpeed * controlValue * Time.deltaTime) sliderValue += sliderSpeed * controlValue * Time.deltaTime;
                        else sliderValue = sliderMax; //gets rid of the 'hop' when maxed out
                    }

                    if (controlValue < 0)
                    {
                        if (sliderValue - sliderMin >= sliderSpeed * controlValue * Time.deltaTime) sliderValue -= -sliderSpeed * controlValue * Time.deltaTime;
                        else sliderValue = sliderMin;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, sliderValue);
                    controlValue = (sliderValue - sliderMin) / (sliderMax - sliderMin);
                }
            }

            else if (mouseSelection.clickedObject.CompareTag("Toggle") && mouseSelection.newClickForToggle) //needed newClick to make sure toggles stay
            {
                //play click sound
                mouseSelection.newClickForToggle = false; //keep toggle from being toggled until the next new click of the mouse
                if (transform.localPosition.y >= toggleUpHeight) //toggle currently off
                {
                    toggled = true; //turns toggle on
                    controlValue = 1;
                    
                    transform.localPosition = new Vector3(transform.localPosition.x, toggleDownHeight, transform.localPosition.z);
                }
                else if (transform.localPosition.y <= toggleDownHeight) //toggle is currently on
                {
                    
                    toggled = false; //turns toggle off
                    controlValue = 0;
                    transform.localPosition = new Vector3(transform.localPosition.x, toggleUpHeight, transform.localPosition.z);
                }
            }
            

        }
    }
}
