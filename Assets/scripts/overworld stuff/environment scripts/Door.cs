using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string goesToScene;

    public void Interact()
    {
        SceneManage.current.loadInterior(goesToScene);

    }
}
