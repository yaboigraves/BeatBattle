using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

//so this managers job is to handle anytime a cutscene needs to be triggered in the overworld
//this is anytime the player is forced into dialogue, ie not entering it via interacting with an npc 

//CUTSCENES
/*
couple of options for these
things that happen in cutscenes 
-camera changes position (yarn can do that already)
-entities need to be able to move <- this is gonna be handled by this script and yarn can trigger it 

*/

//so this managers job is 2 fold
//1. - triggering cutscenes (basically just a yarn script passed in from a trigger object)
//2. - managing the movement of entities during a cutscene 

//each cutscene is a scriptable object(?) that contains a list of cutscene entities that can be referenced via yarn 
//              v position to move to
//ex : entity1 nextPosition 0.1f 
//    ^ name of entity      ^ time to get there? or maybe time to wait

//cutscene entities 
//these are basically just gameobjects and a list of waypoints said gameobject can move between 

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager current;
    public DialogueRunner dialogueRunner;

    public Cutscene currentCutscene;

    public int numEvents;

    public bool inCutscene;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        dialogueRunner.AddCommandHandler("moveEntity", moveEntity);

        if (cutsceneYarnProgram != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(cutsceneYarnProgram);
        }
    }

    //so this means that to load a cutscene, the npc or the trigger
    //needs to store a reference to it 

    public void LoadCutscene(Cutscene cutscene)
    {
        //cutscene is now loaded and we can manipulate it via yarn
        currentCutscene = cutscene;
    }


    public void StartCutscene()
    {
        numEvents = 0;
        inCutscene = true;
        //lock player movement
        InputHandler.current.LockPlayerMovement(true);

        //runs all the events, once they're all done they subtract from the number of events and when the number of events hits 0 we end the cutscene
        currentCutscene.cutsceneActions.Invoke();

        //tell the savemanager we've run this cutscene
        SaveManager.UpdateCutsceneData(currentCutscene.cutsceneID);


    }


    //functions needed for cutscenes

    //move entity to its next waypoint
    public void moveEntity(string[] parameters)
    {
        print("moving entity");
        //look into the current cutscene and find an entity with the given name
        CutsceneEntity entity = currentCutscene.GetEntityByName(parameters[0]);
        //tell the entity to move to its next position
        entity.moveToWayPoint();
    }

    //so unity events can only take strings (annoying) so the arguments to these 2 functions need to be parsed between the comma 
    //the format is argument,time to wait before we actually trigger the event
    public void moveEntity(string entityName)
    {
        KeyValuePair<string, float> argTime = parseCutsceneEvent(entityName);
        //pass this to a coroutine to wait  the alloted amount of time
        StartCoroutine(moveEntityRoutine(argTime.Key, argTime.Value));

    }

    IEnumerator moveEntityRoutine(string entityName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CutsceneEntity entity = currentCutscene.GetEntityByName(entityName);
        //tell the entity to move to its next position
        entity.moveToWayPoint();

        //after the routine is done subtract 1 from the number of cutscenes, if we're out of events then end the cutscene 
        numEvents--;

        CheckEndOfCutscene();
    }



    //this is just going to look at the camera in that position in the cameras array
    public void moveCamera(string positionNum)
    {
        KeyValuePair<string, float> argTime = parseCutsceneEvent(positionNum);
        //switch the camera priority to the current camera after the wait
        int cameraIndex = int.Parse(argTime.Key);
        float waitTime = argTime.Value;

        StartCoroutine(moveCameraRoutine(cameraIndex, waitTime));



    }

    IEnumerator moveCameraRoutine(int cameraIndex, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        numEvents--;
        currentCutscene.cameras[cameraIndex].Priority = 420; //haha weed number
        CheckEndOfCutscene();
    }


    private KeyValuePair<string, float> parseCutsceneEvent(string eventArg)
    {
        //we know that if we're parsing an event then that cutscene is running so add 1 to the numScenes 

        numEvents++;

        //find the comma then seperate the string there
        string arg = eventArg.Substring(0, eventArg.IndexOf(','));

        float time = float.Parse(eventArg.Substring(eventArg.IndexOf(',') + 1));

        return new KeyValuePair<string, float>(arg, time);
    }

    private void CheckEndOfCutscene()
    {
        if (numEvents <= 0)
        {
            inCutscene = false;
            //reset the camera priority back to player camera
            //can just looop through all the cutscene cameras and set prio to -420 

            currentCutscene.DisableCameras();
            //give the player movement back
            InputHandler.current.LockPlayerMovement(false);
        }
    }




    //random utility custscene stuff 

    public YarnProgram cutsceneYarnProgram;
    public string cutsceneYarnNode = "";
    public void PickupUniqueItemCutscene()
    {
        //lock the player movement


        InputHandler.current.TogglePickupCutscene(true);

        //enable the ui text for picking up a unique item (do this with yarn)
        FindObjectOfType<DialogueRunner>().StartDialogue(cutsceneYarnNode);

        //switch to the pickup camera through the camera manager
        CameraManager.current.enablePickupItemCamera(true);
    }

    public void EndPickupItemCutscene()
    {
        FindObjectOfType<DialogueUI>().MarkLineComplete();
        InputHandler.current.TogglePickupCutscene(false);
        CameraManager.current.enablePickupItemCamera(false);
        GameManager.current.player.inventory.TogglePickupItemSprite(false);
    }





}
