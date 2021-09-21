using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//so minigames def need a reference to their canvas for sure

//what stuff does literally every minigame need

//-canvas

//-whate state its in
//-staging (its coming up next so we should show this to the player somehow)
//-active (minigame actively being run)
//-inactive

//later*
//what modular effectors are currently active on it



//so the basic flow for this is
/*
-minigame loads
-should start with an inactive canvas
-minigame chills until it gets staged when we then activate and initialize all its stuff
-minigame gets run
-after the minigame gets run we set it back inactive and reset all of its shit
*/

//NOTE: these need to be modular minigames IE there are effectors that can modify how the minigame plays
//basic idea will be program in a bunch of small little permutations that are only activated when fighting a certain type of enemy


[System.Serializable]
public class MiniGame : MonoBehaviour
{

    //so this could maybe become a scriptable object actually
    //so actually there will be a minigame scriptable object that contains like the scene to load etc
    //this can be inside of a sample, so samples directly have a way to know what minigame theyre loading and the scene


    public MiniGameSettings miniGameSettings;
    public Canvas miniGameCanvas;
    public MiniGameState state;

    //pretty much all minigames are going to have indicators now, so lets make it so we can plug in a channel of floats for these
    //TODO: extend this to work with multiple channels
    //TODO: we probably want these to be able to have there samples assigned as well

    public List<float> beatTimes;

    public MinigameReport report;

    public Scene minigameScene;

    private void Awake()
    {

    }

    private void Start()
    {
        //so we should onloy actually load stuff once the minigame has been loadeed and its sample has been set
        LoadStuff();
    }

    public void LoadStuff()
    {
        //Debug.Log(gameObject.scene.GetHashCode());
        // Debug.Log(miniGameSettings.minigameSample.miniGameSceneName);
        miniGameCanvas.gameObject.SetActive(false);
        state = MiniGameState.Inactive;
        //register this minigame with the minigame manager

        if (MinigameManager.current)
        {
            //so when we register the minigame, register it with the sample that the minigame uses

            MinigameManager.current.registerMiniGame(this);
        }
        minigameScene = gameObject.scene;
        //later this will be based on more channels
        //report = new MinigameReport(miniGameSettings.minigameSample.sampleTrack.randomTrackData.kickBeats.Count);
    }

    public virtual void StartMiniGame()
    {
        state = MiniGameState.Active;
    }

    public virtual void Preload(Sample sample) { }

    public void SetBeatTimes(List<float> times)
    {

        beatTimes = times;
    }

}

public enum MiniGameState
{
    Inactive,
    Staged,
    Active
}

//so this can just contain some base settings about like what the bpm is how many beats the minigame is going to be out for, etc

[System.Serializable]
public class MiniGameSettings
{
    public int numBeats;
    public Sample minigameSample;
}