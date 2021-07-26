using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRange : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        InteractRange i = FoundPlayer(other);
        if (i)
        {
            i.NotifyInRange(transform.parent.gameObject, true);
            //Debug.Log("found a thing");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractRange i = FoundPlayer(other);
        if (i)
        {
            i.NotifyInRange(transform.parent.gameObject, false);
            //Debug.Log("leaving a thing");
        }
    }



    InteractRange FoundPlayer(Collider other)
    {
        InteractRange playerInteractRange = other.gameObject.GetComponent<InteractRange>();

        if (playerInteractRange != null)
        {
            return playerInteractRange;
        }
        else
        {
            return null;
        }
    }
}
