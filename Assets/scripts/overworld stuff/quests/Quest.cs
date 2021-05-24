using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Quest
{

    // NOTE: all quest names need to start with $ to play nice with yarns format
    public string questName;
    public int questStatus;
    //quests are tracked as integers
    //0 - not started 
    //1 - started
    //2 - completed


    //for now just one quest objective
    public GameItem questObjective;



}