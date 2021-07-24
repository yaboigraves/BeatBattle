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
            other.gameObject.GetComponent<IInteractable>().notify(true);
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
                other.gameObject.GetComponent<IInteractable>().notify(false);

                objectsInRange.Remove(other.gameObject);
                //notify the element we're not in range anymore


            }
        }
    }

}
