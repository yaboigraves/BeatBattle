using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//so this things job is to use both the player prefs, and game state objects that are serialized to load and save game info


public static class SaveManager
{

    /*  SETTINGS NAMES
        -musicVolume

    */


    //public static StoryData storyData;

    //big globally accessible access to the currently loaded gamestate 
    //this can be used to trigger alot of stuff and/or autosave stuff without needing to use yarn
    public static GameStateData gameStateData;


    public static void saveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/testSave.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //Load the players data into a playerdata object, send this object to the player,
        //player will then fill it up and then ship it back here to be saved

        PlayerData data = GameManager.current.player.savePlayerData();

        //load all the story data
        StoryData storyData = new StoryData();

        //so when we save the game we need to go through all of our story variables, and see if yarn's 
        //variable storage has any differences
        //load the values from yarn into the storage then we're good to go



        foreach (QuestData q in storyData.questList.quests)
        {
            Yarn.Value questValue = UIManager.current.dialogueRunner.variableStorage.GetValue(q.questName);

            Debug.Log("saving story data for quest" + q.questName + " as value " + questValue);
            if (questValue != null)
            {
                q.questStatus = (int)questValue.AsNumber;
            }
        }

        //load any progress data currently stored in the yarn variable storage
        //alot of progress data is going to be set via scripts so this is fine

        ProgressData progressData;
        if (gameStateData != null)
        {

            progressData = gameStateData.progressData;
        }
        else
        {
            Debug.Log("creating new progress data");
            progressData = new ProgressData();
        }


        foreach (ProgressPoint p in progressData.progress)
        {

            Yarn.Value progressValue = UIManager.current.dialogueRunner.variableStorage.GetValue(p.pointName);

            //Yarn.Values are always not null check the asstring variable
            //what in the fuck is this, so yea its not null its a string containing null
            if (progressValue.AsString != "null")
            {
                Debug.Log("saved progress value " + progressValue + " from yarn");
                p.pointValue = progressValue.AsBool;
            }
        }





        //load cutscene progress 

        //so if there is no gamestate currently loaded, then we create a new empty list of integers 
        //TODO: remove this as gamestate is only loaded one time then assumed to be set good
        List<int> cutsceneData;

        if (gameStateData.cutScenesRun == null)
        {
            cutsceneData = new List<int>();
        }
        //otherwise we just load whatever cutscene data is stored in the save currently (this is changed during runtme) and save that
        else
        {
            cutsceneData = gameStateData.cutScenesRun;
        }


        //eee

        //TODO: right now the gamestatedata is only used to hold a live version of the progress data
        //this is because the story data is mostly handled by yarn and the player data is loaded on save
        //later probably load the story data the same way
        GameStateData gameData = new GameStateData(data, storyData, progressData, cutsceneData);

        formatter.Serialize(stream, gameData);
        stream.Close();

    }




    public static GameStateData loadGame()
    {
        string path = Application.persistentDataPath + "/testSave.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameStateData data = null;

            if (stream.Length > 0)
            {
                data = formatter.Deserialize(stream) as GameStateData;
            }

            stream.Close();

            gameStateData = data;
            return data;
        }
        else
        {
            //create a default save
            Debug.LogError("o shit no save");
            return null;
        }


        //test stuff for loading story variables into the yarn variables
    }

    public static void LoadDefaultSettings()
    {
        //go through some of the settings and if they're 0 or null set them to defaults
    }

    public static void LoadSettings()
    {
        //set the track volume equal to whatever is loaded in memory 
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        //TrackManager.current.UpdateTrackVolume(musicVolume);
        //tell the ui to reflect this change
        UIManager.current.volumeSlider.value = musicVolume;
    }

    public static void UpdateVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("musicVolume", newVolume);
    }


    //tells the currentsave that we've seen a custcene so dont run it again
    public static void UpdateCutsceneData(int cutSceneID)
    {
        gameStateData.cutScenesRun.Add(cutSceneID);
    }

    public static bool checkIfCutsceneRan(int cutSceneID)
    {

        Debug.Log(gameStateData.cutScenesRun);

        if (gameStateData.cutScenesRun.Contains(cutSceneID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

