using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRange : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractionManager i = FoundPlayer(other);
        if (i)
        {
            i.NotifyInRange(transform.parent.gameObject, true);
            //Debug.Log("found a thing");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractionManager i = FoundPlayer(other);
        if (i)
        {
            i.NotifyInRange(transform.parent.gameObject, false);
            //Debug.Log("leaving a thing");
        }
    }



    PlayerInteractionManager FoundPlayer(Collider other)
    {
        PlayerInteractionManager playerInteractRange = other.gameObject.GetComponent<PlayerInteractionManager>();

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
