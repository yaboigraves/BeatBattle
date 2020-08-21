using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;

public class NPC : MonoBehaviour, IInteractable
{
    //this npc class is moreso like the talk-to-able npc there need to also be one for npcs with passing dialogue

    //the npc also needs to load all the custom cameras for its dialogue if it has any
    //these are then loaded into a dictionary 
    //this dictionary of camera's is then

    Dictionary<string, CinemachineVirtualCamera> cameraPositions;
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    public void Interact()
    {
        DialogCameraController.current.setCameraObjects(cameraPositions);
        FindObjectOfType<DialogueRunner>().StartDialogue(talkToNode);
    }

    private void Start()
    {
        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);
        }

        cameraPositions = new Dictionary<string, CinemachineVirtualCamera>();

        Transform cameraObjs = transform.GetChild(0);
        for (int i = 0; i < cameraObjs.childCount; i++)
        {
            cameraPositions.Add(cameraObjs.GetChild(i).name, cameraObjs.GetChild(i).GetComponent<CinemachineVirtualCamera>());
        }
    }
}
