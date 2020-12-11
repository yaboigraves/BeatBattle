using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using TMPro;

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

    public void Interact()
    {


        //check if we have a cutscene to load 
        if (cutscene != null)
        {
            CutsceneManager.current.LoadCutscene(cutscene);
        }

        print(cameraPositions);

        FindObjectOfType<DialogueRunner>().StartDialogue(talkToNode);
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



        Transform cameraObjs = transform.GetChild(0);
        for (int i = 0; i < cameraObjs.childCount; i++)
        {

            cameraPositions.Add(cameraObjs.GetChild(i).name, cameraObjs.GetChild(i).GetComponent<CinemachineVirtualCamera>());
            //print(cameraObjs.GetChild(i).name);
        }

        DialogCameraController.current.setCameraObjects(cameraPositions);

        npcTextBox.transform.parent.gameObject.SetActive(false);
    }

    //text stuff
    public void NPCGreet()
    {
        StopCoroutine(npcTextTrigger());
        if (greetingText != "")
        {
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
}
