using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHack : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    GameObject currentlySelectedRadioObj;
    Radio currentlySelectedRadio;



    private void Start()
    {
        enabled = false;
    }

    public void ToggleHack(bool toggle)
    {
        this.enabled = toggle;

        if (!toggle)
        {
            Deselect();
        }
    }


    private void Update()
    {
        //so if we're hacking basically shoot a ray out of the player's camera
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, 20f, mask))
        {
            //so if we hit a radio we should tell it to raise its notification icon so you can see it's selected

            //then you can select it and a menu will open up showing a list of songs and their vibes
            //for now approximate this with 4 preset options

            //depending on which button you press or select it will apply an effect to all things within the radius of the radio

            //this effect is then processed by the events hit by it
            if (hit.collider.gameObject != currentlySelectedRadioObj)
            {
                if (currentlySelectedRadioObj != null)
                {
                    currentlySelectedRadio.Select(false);
                }

                currentlySelectedRadioObj = hit.collider.gameObject;
                currentlySelectedRadio = currentlySelectedRadioObj.GetComponentInParent<Radio>();
                currentlySelectedRadio.Select(true);
            }


            if (Input.GetMouseButtonDown(0))
            {
                //so now we need to open a ui menu, so we ask the ui manager to open a menu and set it to spawn near the mouse 
                UIManager.current.SpawnHackingMenu();
            }

            return;
        }

        //if we get here and there's something selected unselect it

        if (currentlySelectedRadio != null)
        {
            currentlySelectedRadio.Select(false);
            currentlySelectedRadio = null;
            currentlySelectedRadioObj = null;
        }


    }

    void Deselect()
    {
        if (currentlySelectedRadio != null)
        {
            currentlySelectedRadio.Select(false);
            currentlySelectedRadio = null;
            currentlySelectedRadioObj = null;
        }
    }

}
