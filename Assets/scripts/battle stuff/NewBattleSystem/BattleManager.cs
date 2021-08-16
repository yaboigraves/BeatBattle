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

    }

    // public void InitQueue()
    // {
    //     //Debug.Log("Initializing the turnQueue");
    //     turnQueue = new List<BattleAction>();

    //     //so rather than building these randomly build them from the players set
    //     for (int b = 0; b < 2; b++)
    //     {
    //         for (int i = 0; i < playerSet.Length; i++)
    //         {
    //             PlayerBattleAction turn = new PlayerBattleAction();
    //             Sample s = Instantiate(playerSet[i]);
    //             turn.minigameSceneName = playerSet[i].sampleName;
    //             turn.playerOrEnemy = true;
    //             turn.sample = s;
    //             turnQueue.Add(turn);

    //             //do an enemy turn for this player turn
    //             EnemyBattleAction enemyTurn = new EnemyBattleAction();
    //             enemyTurn.playerOrEnemy = false;
    //             enemyTurn.dmg = enemies[0].attack;
    //             turnQueue.Add(enemyTurn);

    //         }
    //     }


    //     //store this turn queue as the one we can reset to
    //     savedTurnQueue = new List<BattleAction>();
    //     savedTurnQueue.AddRange(turnQueue);

    //     //Debug.Log(savedTurnQueue.Count);


    //     calculateQueueModifiers();
    //     //MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)turnQueue[0]).sample.miniGameSceneName);
    //     BattleUIManager.current.InitTurnQueue(turnQueue);


    // }

    //so once all the samples get added we then go through and calculate how all the samples will modify one another
    //basically each modifier needs the whole state of the queue, so we modify the state by handing it off to 
    //the sample one at a time right->left

    // public void calculateQueueModifiers()
    // {
    //     //go through each of the samples in the queue, and call their function, store the result, and then do it again for the next modifier
    //     //Debug.Log(turnQueue.Count);

    //     //+= 2 to skip the enemy turns
    //     for (int i = 0; i < turnQueue.Count; i += 2)
    //     {
    //         PlayerBattleAction a = (PlayerBattleAction)turnQueue[i];
    //         if (a.sample.functionName != "")
    //         {
    //             turnQueue = SampleEffects.processSampleEffect(turnQueue, i);
    //         }
    //     }


    // }




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


        //start the countin phase
        battle.currentState = BattleState.Countin;
        //wait 1 bar then go into either player or enemy turn phase
        //this should be handled by a seperate static class that helps dispatch waits based on the audiosettings.dsp time
        WaitCallback methodToCall = ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, 2));

        //play the audio
        BattleAudioManager.current.StartSong();
    }


    //so yea this basically handles damamge and shit
    public void EndTurn()
    {

        if (((PlayerBattleAction)battle.turnQueue[0]).sample.sampleType == SampleType.block)
        {
            //TODO: add block
        }

        //TODO: this happens all after the minigame runs


        //so these should happen at the end of the phase, not the beggining 

        //do the players damage and the enemies damage
        PlayerHealth -= ((EnemyBattleAction)battle.turnQueue[1]).dmg;

        if (((PlayerBattleAction)battle.turnQueue[0]).sample.sampleType == SampleType.damage)
        {
            EnemyHealth -= ((PlayerBattleAction)battle.turnQueue[0]).sample.numericValue;
        }
        else if (((PlayerBattleAction)battle.turnQueue[0]).sample.sampleType == SampleType.heal)
        {
            PlayerHealth += ((PlayerBattleAction)battle.turnQueue[0]).sample.numericValue;
        }


        //BattleUIManager.current.UpdateHealth();

        //update the ui, 
        BattleUIManager.current.UpdateTurnQueue();

        //remove the elements from the queue

        battle.turnQueue.RemoveRange(0, 2);
    }


    //depending on battle phase we start a different persons turn
    public void ChangeTurn()
    {
        //so we need to check if we should refill the queue here



        //Debug.Log("turn changing");
        //turn off the active minigame canvas

        //if there was a turn previously running, end it
        if (battle.currentState != BattleState.Prebattle && battle.currentState != BattleState.Countin)
        {
            EndTurn();
        }


        //so if there's only one bar left of input just add on the whole turnqueue to the end to create a new loop
        if (battle.turnQueue.Count / 2 <= 4)
        {
            Debug.Log("resetting the turnqueue");
            battle.turnQueue.AddRange(battle.savedTurnQueue);

            //reupadte the ui

            //so we only need to recalculate the queue if you do an interlude
            //calculateQueueModifiers();

            BattleUIManager.current.InitTurnQueue(battle.turnQueue);
        }


        if (battle.turnQueue.Count > 0)
        {
            MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)battle.turnQueue[0]).sample.miniGameSceneName);


            //TODO: this organizatio needs to be refactored
            //pull out and load the minigame
            MinigameManager.current.ActivateMinigame(((PlayerBattleAction)battle.turnQueue[0]).sample.miniGameSceneName);

            //load the next minigame in the queue



        }


        switch (battle.currentState)
        {
            case BattleState.Countin:
                battle.currentState = BattleState.PlayerTurn;
                break;
            case BattleState.PlayerTurn:
                battle.currentState = BattleState.EnemyTurn;
                break;
            case BattleState.EnemyTurn:
                //so here is where we check if an interlude is requested, if so we go into an interlude
                battle.currentState = interludeRequested ? BattleState.Interlude : BattleState.PlayerTurn;
                interludeRequested = false;
                break;
            case BattleState.Interlude:
                //if we're in an interlude then we just go back to the players turn
                battle.currentState = BattleState.PlayerTurn;
                break;
        }



        //after the turn is changed, wait however many bars and then do it all again woo
        WaitCallback methodToCall = ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, 2));
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