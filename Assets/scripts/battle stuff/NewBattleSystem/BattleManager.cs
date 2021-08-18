using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//8-14 rewrite
//okie dokie so this is gonna be a slightly big redo
//we are going to need a general input manager for this, the minigame scenes that are loaded are really just visualizers


public class BattleManager : MonoBehaviour
{
    public static BattleManager current;
    public delegate void WaitCallback();


    public bool interludeRequested = false;

    public Sample[] playerSet, playerSamples;

    int playerHealth, enemyHealth;


    public int PlayerHealth
    {
        get => playerHealth;

        set
        {
            BattleUIManager.current.UpdateHealth();
            playerHealth = value;


        }
    }

    public int EnemyHealth
    {
        get => enemyHealth;

        set
        {
            BattleUIManager.current.UpdateHealth();
            enemyHealth = value;


        }
    }

    List<BattleAction> savedTurnQueue;


    public Battle battle;

    private void Awake()
    {

        current = this;
        //once all the minigames scenes are loaded, we can construct the queue of turns based on the players sets
        //for now we'll just construct a simple back and forth for 2 iterations

        //init the battle
        InitBattle();

    }

    void Start()
    {
        //so first things first, we have to set the state to pre-battle
        StartCoroutine(MinigameManager.current.LoadMinigames());

        //EnemyHealth = battle.enemies[0].hp;
        EnemyHealth = 5;
        PlayerHealth = 10;

        //update health text
        // BattleUIManager.current.UpdateHealth();
    }

    void InitBattle()
    {
        battle = new Battle();
        battle.manager = this;
        battle.enemies = new NEnemy[1];

        //testing code for now
        battle.enemies[0] = GameObject.FindObjectOfType<NEnemy>();
    }

    public void InitQueue(Sample[] samples)
    {
        //        Debug.Log("Initializing the from the ui");
        playerSet = samples;
        //InitQueue();

        battle.InitTurnQueue(samples);

        //after the queue has been initialized, we can load the minigames requested
        //send this off to the minigame manager to handle, probably want a coroutine so we cant start the battle till these load
        // StartCoroutine(MinigameManager.current.LoadMinigames(turnQueue));
        TimeManager.beatTimeline.InitializeTimeline(samples);

        BattleAudioManager.current.InitializeBattleAudio();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (battle.currentState == BattleState.Prebattle && Input.GetKeyDown(KeyCode.Space) && MinigameManager.current.minigamesLoaded)
        {
            StartBattle();
        }

        if ((battle.currentState == BattleState.PlayerTurn || battle.currentState == BattleState.EnemyTurn) && Input.GetKeyDown(KeyCode.Escape))
        {
            interludeRequested = true;
        }
    }

    void StartBattle()
    {
        //play the audio
        BattleAudioManager.current.StartSong();

        //start the countin phase
        battle.currentState = BattleState.Countin;
        //wait 1 bar then go into either player or enemy turn phase
        //this should be handled by a seperate static class that helps dispatch waits based on the audiosettings.dsp time
        WaitCallback methodToCall = battle.ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, battle.getCurrentTrack().numBars));
    }


    public void RefreshTurn()
    {
        Debug.Log("starting the wait for the next turn");
        Debug.Break();
        WaitCallback methodToCall = battle.ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, battle.getCurrentTrack().numBars));
    }

    //also works for healing
    public void DmgPlayer(int amount)
    {
        PlayerHealth -= amount;
        //BattleUIManager.current.UpdateHealth();
    }

    public void HealPlayer(int amount)
    {
        PlayerHealth += amount;
        //BattleUIManager.current.UpdateHealth();
    }

}
//test

public enum BattleState
{
    Prebattle,
    Countin,
    PlayerTurn,
    EnemyTurn,
    Interlude
}




//so these are going to need to have a scriptable object loaded with them
//load these around the samples put in the array


//TODO: Refactor to be base battleturn, stuff with both enemy and player, 
public class BattleAction
{
    public bool playerOrEnemy;

}

public class PlayerBattleAction : BattleAction
{
    public string minigameSceneName;
    public Sample sample;
}

public class EnemyBattleAction : BattleAction
{
    //dmg range?

    //tODO: make it so the enemy actually effects this

    public int dmg = 1;
}