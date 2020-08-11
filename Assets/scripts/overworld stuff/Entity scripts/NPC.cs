using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    public void Interact()
    {
        //TODO: reimplement with setence object array
        //UIManager.current.NPCStartTalk(dialogue.sentences, dialogue);

        FindObjectOfType<DialogueRunner>().StartDialogue(talkToNode);
    }

    private void Start()
    {
        //lol
        dialogue.initDialogue();

        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);
        }
    }
}
