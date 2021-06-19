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



public class MiniGame : MonoBehaviour
{

    public MiniGameSettings miniGameSettings;
    public Canvas miniGameCanvas;
    public MiniGameState state;

    private void Awake()
    {

    }

    private void Start()
    {
        LoadStuff();
    }

    public void LoadStuff()
    {
        //Debug.Log(miniGameSettings.minigameSample.miniGameSceneName);
        miniGameCanvas.gameObject.SetActive(false);
        state = MiniGameState.Inactive;
        //register this minigame with the minigame manager

        if (MinigameManager.current)
        {
            MinigameManager.current.registerMinigame(this);
        }

    }

    public virtual void StartMiniGame()
    {
        state = MiniGameState.Active;
    }

    public virtual void Preload()
    {

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
    public float bpm;
    public int numBeats;
    public Sample minigameSample;
}