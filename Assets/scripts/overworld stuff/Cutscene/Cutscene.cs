using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.Playables;

//each cutscene needs a array of gameobjects in the cutscene to be manipulated by yarn
//each gameobject needs an array of waypoints 

//CUTSCENES CAN ALSO be used in dialogue to move entities around but are not required. 

//so without dialogue a cutscene is triggered 
//after it is triggered we then need to know the order in which actions occur (entities moving/camera positions)
//ideally this should be able to be done via editor 

//first prototype : open a door and show the camera there, then show the player moving towards the door
//so the first thing that happens here is a camera movement and then a wait 
//then the player moves then a wait 

public class Cutscene : MonoBehaviour
{

    public UnityEvent cutsceneActions;

    public CutsceneEntity[] entities;
    public CinemachineVirtualCamera[] cameras;

    public int cutsceneID;

    public PlayableDirector director;

    //also going to maybe need camera positions for some cutscenes
    //need a function that goes through entities list and finds an entity by name

    public CutsceneEntity GetEntityByName(string entityName)
    {
        foreach (CutsceneEntity entity in entities)
        {
            if (entity.entityName == entityName)
            {
                return entity;
            }
        }

        Debug.LogError("entity not found in cutscene " + entityName);
        return null;
    }

    public void DisableCameras()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            camera.Priority = -420;
        }
    }

    //this function takes in an entity and starts a coroutine that moves it to its waypoint







}
