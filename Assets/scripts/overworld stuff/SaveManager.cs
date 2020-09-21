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



    public static void saveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/testSave.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //Load the players data into a playerdata object, send this object to the player,
        //player will then fill it up and then ship it back here to be saved

        PlayerData data = GameManager.current.player.savePlayerData();
        GameStateData gameData = new GameStateData(data);

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
            return data;
        }
        else
        {
            //create a default save
            Debug.LogError("o shit no save");
            return null;
        }
    }

    public static void LoadDefaultSettings()
    {
        //go through some of the settings and if they're 0 or null set them to defaults

    }

    public static void LoadSettings()
    {

        //set the track volume equal to whatever is loaded in memory 
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        TrackManager.current.UpdateTrackVolume(musicVolume);
        //tell the ui to reflect this change
        UIManager.current.volumeSlider.value = musicVolume;


    }

    public static void UpdateVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("musicVolume", newVolume);
    }

    //so to save the gamestate data going to need some kind of object that tracks a shit ton of booleans
    //this can be an object in the game manager that gets 







}
