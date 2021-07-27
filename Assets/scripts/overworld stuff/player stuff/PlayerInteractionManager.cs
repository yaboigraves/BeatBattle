using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: rename this because this is moreso just going to handle all of the objects in range for the player
public class PlayerInteractionManager : MonoBehaviour
{

    public List<GameObject> objectsInRange;
    PlayerInventory inventory;
    IInteractable currentSelected;


    void Start()
    {
        objectsInRange = new List<GameObject>();
        inventory = transform.parent.GetComponent<PlayerInventory>();
    }

    //so this needs to get called every time we issue a move command if stuff is in range
    //should probably make some kind of observer based on the players movement so stuff can be scripted based off of it 
    //

    private void Update()
    {
        if (objectsInRange.Count > 1)
        {
            RecalculateSelected();
        }
    }

    public void InteractWithSelected()
    {
        currentSelected.Interact();
    }



    void RecalculateSelected()
    {


        if (objectsInRange.Count <= 0)
        {
            //turn off the selection icon
            UIManager.current.selectionIcon.SetActive(false);
            return;
        }


        //so this should be based on distance
        //find the closest dude i guess lol

        UIManager.current.selectionIcon.SetActive(true);

        GameObject closest = objectsInRange[0];

        for (int i = 0; i < objectsInRange.Count; i++)
        {
            if (Vector3.Distance(transform.position, objectsInRange[i].transform.position) < Vector3.Distance(transform.position, closest.transform.position))
            {
                closest = objectsInRange[i];
            }
        }

        currentSelected = closest.GetComponentInChildren<IInteractable>();

        closest.GetComponent<IInteractable>().notify(InteractionEvent.SELECTED);
    }

    public void NotifyInRange(GameObject interactable, bool inRange)
    {
        if (inRange)
        {
            objectsInRange.Add(interactable.gameObject);
            //notify the element that we're in range so we can do whatever we need to do
            interactable.gameObject.GetComponent<IInteractable>().notify(InteractionEvent.IN_RANGE);
            RecalculateSelected();
        }
        else
        {
            objectsInRange.Remove(interactable.gameObject);
            interactable.gameObject.GetComponent<IInteractable>().notify(InteractionEvent.OUT_OF_RANGE);
            RecalculateSelected();
        }

    }
}

