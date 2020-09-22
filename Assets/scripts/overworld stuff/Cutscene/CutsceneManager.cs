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

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        dialogueRunner.AddCommandHandler("moveEntity", moveEntity);
    }

    //so this means that to load a cutscene, the npc or the trigger
    //needs to store a reference to it 

    public void LoadCutscene(Cutscene cutscene)
    {
        //cutscene is now loaded and we can manipulate it via yarn
        currentCutscene = cutscene;
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







}
