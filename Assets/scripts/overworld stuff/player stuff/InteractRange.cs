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
                objectsInRange.Remove(other.gameObject);
            }
        }
    }

}
