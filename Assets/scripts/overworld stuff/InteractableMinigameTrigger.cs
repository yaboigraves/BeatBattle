using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMinigameTrigger : MonoBehaviour, IInteractable
{

    public string miniGameSceneName;

    public bool Interact()
    {
        Debug.Log("launch the minigame");

        SceneManage.current.LoadOverworldMinigame();

        return true;
    }


    public void notify(InteractionEvent interactionEvent)
    {
        //TODO: debug this it seems to be triggering alot....
        //Debug.Log("detected");
    }
}
