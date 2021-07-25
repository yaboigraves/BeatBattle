using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRange : MonoBehaviour
{

    public List<GameObject> objectsInRange;
    PlayerInventory inventory;


    void Start()
    {
        objectsInRange = new List<GameObject>();
        inventory = transform.parent.GetComponent<PlayerInventory>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>() != null)
        {
            objectsInRange.Add(other.gameObject);

            //notify the element that we're in range so we can do whatever we need to do
            other.gameObject.GetComponent<IInteractable>().notify(InteractionEvent.IN_RANGE);

            RecalculateSelected();
        }

        if (other.gameObject.GetComponent<IPickupable>() != null)
        {
            other.gameObject.GetComponent<IPickupable>().Pickup(inventory);
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>() != null)
        {
            if (objectsInRange.Contains(other.gameObject))
            {
                other.gameObject.GetComponent<IInteractable>().notify(InteractionEvent.OUT_OF_RANGE);

                objectsInRange.Remove(other.gameObject);
                //notify the element we're not in range anymore

                RecalculateSelected();


            }
        }
    }

    void RecalculateSelected()
    {
        //mark the 0th position in objects in range as the selected object
        //for now we can just activate the underline graphic and mark it to follow the image?
        //probably easier than having every icon also contain the selected icon

        if (objectsInRange.Count > 0)
        {
            UIManager.current.selectionIcon.SetActive(true);

            objectsInRange[0].GetComponent<IInteractable>().notify(InteractionEvent.SELECTED);
        }
        else
        {
            //turn off the selection icon
            UIManager.current.selectionIcon.SetActive(false);
        }
    }
}
