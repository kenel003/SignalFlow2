using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseSelectionController : MonoBehaviour
{
    public GameObject clickedObject;
    public bool newClick = false, newClickForToggle = false;
    [SerializeField] private GameManager gameManager;

   

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0) && !gameManager.guiUp)
       {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if(raycastHit.transform != null && !raycastHit.transform.CompareTag("Untagged"))
                {
                    clickedObject = raycastHit.transform.gameObject;
                    newClick = true;
                    newClickForToggle = true;
                    Debug.Log("Click happened once");
                }
            }
       } 
    }
}
