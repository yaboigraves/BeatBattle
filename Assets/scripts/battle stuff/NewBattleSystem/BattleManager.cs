using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//8-21 leaving off note:
/*
so yeah got alot done, just gotta make it so the queue can actually empty and move through tracks/audio dynamically
transitions need to be added but theyre less important then getting midi, so once everything works in an infinite loop we're good
assume a min song length possibly?

so yeah next session just try and get the queue able to completly run through and then just pause at the end when theirs no more tracks
//should probably just loop again for now, probably need to make songs longer than 4 bars, try some demos that run it with 8 bars


*/

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
        //add a metronome tick to all of them
        BattleUIManager.current.EnableMetronomeTicks();

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

        battle.currentState = BattleState.PlayerTurn;

        battle.ChangeTurn();
    }


    public void RefreshTurn()
    {

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