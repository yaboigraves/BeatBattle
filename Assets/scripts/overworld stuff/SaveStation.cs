using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour, IInteractable
{
    public bool Interact()
    {
        print("Game Saved!");
        SaveManager.saveGame();
        return true;

    }

    public void notify(InteractionEvent interactionEvent)
    {

    }

}
