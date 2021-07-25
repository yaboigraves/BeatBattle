using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string goesToScene;

    public bool Interact()
    {
        GameManager.current.player.interactRange.objectsInRange.Clear();
        SceneManage.current.loadLevel(goesToScene, GameManager.current.player.transform.position);
        return true;
    }

    public void notify(InteractionEvent interactionEvent)
    {

    }
}
