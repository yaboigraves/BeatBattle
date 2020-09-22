using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//each cutscene needs a array of gameobjects in the cutscene to be manipulated by yarn
//each gameobject needs an array of waypoints 
public class Cutscene : MonoBehaviour
{
    public CutsceneEntity[] entities;


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

    //this function takes in an entity and starts a coroutine that moves it to its waypoint






}
