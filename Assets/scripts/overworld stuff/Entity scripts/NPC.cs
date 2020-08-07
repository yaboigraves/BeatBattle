using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public void Interact()
    {
        //TODO: reimplement with setence object array
        UIManager.current.NPCStartTalk(dialogue.sentences, dialogue);
    }

    private void Start()
    {
        //lol
        dialogue.initDialogue();
    }
}
