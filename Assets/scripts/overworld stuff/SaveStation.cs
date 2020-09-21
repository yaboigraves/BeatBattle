using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        print("Game Saved!");
        SaveManager.saveGame();

    }

}
