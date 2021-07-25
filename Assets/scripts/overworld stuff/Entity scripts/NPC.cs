using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

public class NPC : Entity, IInteractable
{
    //this npc class is moreso like the talk-to-able npc there need to also be one for npcs with passing dialogue

    //the npc also needs to load all the custom cameras for its dialogue if it has any
    //these are then loaded into a dictionary 
    //this dictionary of camera's is then
    Dictionary<string, CinemachineVirtualCamera> cameraPositions = new Dictionary<string, CinemachineVirtualCamera>();
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    public TextMeshProUGUI npcTextBox;
    [TextArea]
    public string greetingText, goodbyeText;
    //doesnt need to exist, just if entities need to move in this dialogue
    public Cutscene cutscene;

    //if this npc spawns a battle set this here
    public GameObject battleEnemy;

    DialogueRunner dialogueRunner;
    public PlayableDirector cutsceneDirector;

    public bool hasDialogue => scriptToLoad != null && talkToNode != null;

    public Image npcDialogueIcon;

    public void notify(InteractionEvent interactionEvent)
    {
        if (interactionEvent == InteractionEvent.IN_RANGE && hasDialogue)
        {
            //if the npc has dialogue
            //activate the little icon above its head
            npcDialogueIcon.enabled = true;
        }
        else if (interactionEvent == InteractionEvent.OUT_OF_RANGE && hasDialogue)
        {
            npcDialogueIcon.enabled = false;
        }
        else if (interactionEvent == InteractionEvent.SELECTED)
        {
            //so if we got selected as the npc then we need to activate some kind of underline effect on the dialogue icon

            //ask the ui manager for the selected icon to overlay on top of the selection icon

            Debug.Log("asking for selection icon");
            UIManager.current.AskForSelectionIcon(this.npcDialogueIcon);

        }
    }

    public bool Interact()
    {

        if (talkToNode == null || scriptToLoad == null)
        {
            return false;
        }

        //check if we have a cutscene to load 
        //TODO: reiplimnent this
        if (cutsceneDirector != null && cutsceneDirector.playableAsset != null)
        {
            CutsceneManager.current.SetCutscene(cutsceneDirector);
        }

        DialogCameraController.current.EnterDialogue(this);


        FindObjectOfType<DialogueRunner>().StartDialogue(talkToNode);
        //here's where the camera should be notified that it's time to do some dialogue shiet

        return true;
    }


    private void Awake()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
    }
    private void Start()
    {
        if (scriptToLoad != null)
        {
            try
            {
                //dialogueRunner.AddommandHandler("triggerBattle", triggerBattle);
                //bruh why tf this borked
                dialogueRunner.Add(scriptToLoad);
            }
            catch
            {
                // Debug.LogWarning("o shit?");
                //script already loaded
            }
        }



        //TODO: reimpliment later, this only needs to be registered when a player actually talks to the npc
        //DialogCameraController.current.setCameraObjects(cameraPositions);

        npcTextBox.transform.parent.gameObject.SetActive(false);

        cutsceneDirector = GetComponent<PlayableDirector>();
    }

    //text stuff
    public void NPCGreet()
    {
        StopCoroutine(npcTextTrigger());
        if (greetingText != "")
        {
            //Debug.Log("Greeting");
            npcTextBox.text = greetingText;
            npcTextBox.transform.parent.gameObject.SetActive(true);
            StartCoroutine(npcTextTrigger());
        }

    }

    public void NPCGoodbye()
    {
        StopCoroutine(npcTextTrigger());
        if (goodbyeText != "")
        {
            npcTextBox.text = goodbyeText;
            npcTextBox.transform.parent.gameObject.SetActive(true);
            StartCoroutine(npcTextTrigger());
        }
    }

    IEnumerator npcTextTrigger()
    {
        yield return new WaitForSeconds(2);
        npcTextBox.transform.parent.gameObject.SetActive(false);
    }

    [YarnCommand("triggerBattle")]
    public void triggerBattle(string[] parameters)
    {
        //launch a battle with the enemy 
        Enemy enemy = battleEnemy.GetComponent<Enemy>();


        if (battleEnemy != null)
        {
            //lock the players movement

            InputHandler.current.LockPlayerMovement(true);
            // UIManager.current.NPCNextTalk();
            //end dialogue
            SceneManage.current.TransitionToBattle(battleEnemy, enemy.battleTracks[UnityEngine.Random.Range(0, enemy.battleTracks.Length)]);

        }
        else
        {
            Debug.LogError("NO ENEMY IN NPC COMPONENT");
        }
    }

    //called from the signal emitter
    public void PauseCutscene()
    {
        CutsceneManager.current.PauseCutscene();
    }

    public Dictionary<string, CinemachineVirtualCamera> getCameraPositions()
    {
        Dictionary<string, CinemachineVirtualCamera> cameraPositions = new Dictionary<string, CinemachineVirtualCamera>();


        //go through all the objects that are children of the camera parent

        Transform cameraObjs = transform.GetChild(0);
        for (int i = 0; i < cameraObjs.childCount; i++)
        {

            cameraPositions.Add(cameraObjs.GetChild(i).name, cameraObjs.GetChild(i).GetComponent<CinemachineVirtualCamera>());
            //print(cameraObjs.GetChild(i).name);
        }

        if (cameraPositions.Count <= 0)
        {
            Debug.LogError("NO CAMERA POSITIONS FOUND FOR NPC");
            return null;
        }

        return cameraPositions;
    }
}
