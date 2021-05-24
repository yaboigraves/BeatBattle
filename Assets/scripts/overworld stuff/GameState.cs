using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateData
{
    public PlayerData playerData;
    public StoryData storyData;
    public ProgressData progressData;
    public List<int> cutScenesRun;
    public GameStateData(PlayerData playerData, StoryData storyData, ProgressData progressData, List<int> cutsceneData)
    {
        this.playerData = playerData;
        this.storyData = storyData;
        this.progressData = progressData;
        this.cutScenesRun = cutsceneData;
        //this is a test comment
    }
}


//player info 
[System.Serializable]
public class PlayerData
{
    public string playerName = "Yancey";
    public int skrillaCount, playerHealth, playerMaxHealth;
    //array of all the equipped tracks
    public string[] tracks = new string[0];
    public string[] gear = new string[0];

}




//story stuff 

//TODO: probably going to need to store this in terms of levels rather than all variables in one object
//that way we can just load the variables relevant to this level/area and not all the variables for the whole damn game
//for now just testing though, so going to keep it simple then iterate later

//story progress info
[System.Serializable]

public class StoryData
{
    public QuestList questList;

    public StoryData()
    {
        questList = new QuestList();
    }



}


//basic container object that just holds info on if a quest has been started or ended
[System.Serializable]
public class QuestData
{

    // NOTE: all quest names need to start with $ to play nice with yarns format
    public string questName;
    public int questStatus;
    //quests are tracked as integers
    //0 - not started 
    //1 - started
    //2 - completed

}


//big list of default values and names for quests
//TODO: move this to json file


[System.Serializable]
public class QuestList
{

    public QuestData[] quests;
    public QuestList()
    {
        QuestData testQuest = new QuestData();
        testQuest.questName = "$testQuest";
        testQuest.questStatus = 0;
        quests = new QuestData[] { testQuest };
    }

}




//this is just a fat list of booleans that tracks various milestones in the game

[System.Serializable]
public class ProgressData
{
    //this right here is the big fat list of all the booleans in the game, this may need to be jsonified 
    public ProgressPoint[] progress = new ProgressPoint[] {

        new ProgressPoint("$enteredShop", false)

    };

    public ProgressPoint GetProgressPoint(string pointName)
    {
        for (int i = 0; i < progress.Length; i++)
        {
            if (progress[i].pointName == pointName)
            {
                return progress[i];
            }
        }
        //progress point not found
        return null;
    }


    //this returns true or false depending on whether the event has already been triggered
    public bool SetProgressPoint(string pointName, bool value)
    {
        for (int i = 0; i < progress.Length; i++)
        {
            if (progress[i].pointName == pointName)
            {
                Debug.Log("setting progress for " + pointName + " to " + value);

                //check if the value is already what it needs to be 
                if (progress[i].pointValue == value)
                {
                    //no event or anything needs to be triggered 
                    Debug.Log("value is equal to current value, no change required event already triggered");
                    return false;
                }
                else
                {
                    //change the value and trigger whatever stuff the trigger does
                    Debug.Log("difference in values, event triggered for first time");

                    progress[i].pointValue = value;
                    return true;
                }



            }
        }

        return false;
        //Debug.LogError("progress point not found : " + pointName);
    }

}


//basic hacked dictionary that can actually be serialized 
[System.Serializable]
public class ProgressPoint
{
    public string pointName;
    public bool pointValue;

    public ProgressPoint(string name, bool value)
    {
        this.pointName = name;
        this.pointValue = value;
    }
}
