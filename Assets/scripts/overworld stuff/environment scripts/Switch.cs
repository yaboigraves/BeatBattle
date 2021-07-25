using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public GameObject thingToActivate;
    IActivateable activateable;

    private void Start()
    {
        activateable = thingToActivate.GetComponent<IActivateable>();
    }



    public bool Interact()
    {
        activateable.Activate();
        return true;
    }

    public void notify(InteractionEvent interactionEvent)
    {

    }
}
