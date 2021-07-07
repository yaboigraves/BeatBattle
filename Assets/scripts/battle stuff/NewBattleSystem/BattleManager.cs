using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sketches for this : 
//phasing is a big deal, so we need to keep track of the state of the battle

//STATES

//-init
//-1 bar of basically just get ready, and you can do any last minute swaps of your order of party members if you want


//-your turn/enemies turn
//-minigame comes out
//-do the minigame
//-the last beat of the minigame is empty no input there, so we can get ready for the next minigame\
//if you don't request a interlude after the player/enemy turn phase it just loops back around


//prototype : just have a simple player minigame and a simple enemy minigame and alternate between
//beat should change for each minigame, play about 20 seconds each 
//one cycle should probably be about 40 seconds



//so we need some basic sample abilities 
//first setup an infrastucture like the gear pipeline where we can just make sample scriptable objects
//these scriptable objects need the basic info for the sample, as well as some way to find its function that it applies to the quue
//the queue needs to be expanded so that we can iterate through it and calculate the effects of all the modifiers in the queue
//basic UI for demonstrating this
//finally, create a basic menu for organizing a set based on a premade set of samples



//6/14 notes
/*
ok so the basic thing is laid out, theres no audio however
so lets build that in, we can start by using a standard loop that plays 
idea should be that we just for now run through the set you setup on bpm with whatever audio is loaded
minigames duration and speed is dictated by this so the audio and bpm shit needs to be coded in before doing any actual minigame programming
so step 1 is to get the turn changes setup to transition based on the audio thats playing, time manager can handle this
*need to refactor alot of this later so be ready to refine this system and lean more on the timemanager in the future


so for now.... going to hard boil in audio settings but later we gotta figure out how its dynamic
*/

public class BattleManager : MonoBehaviour
{
    public static BattleManager current;
    public delegate void WaitCallback();

    public BattleState currentState = BattleState.Prebattle;

    public bool interludeRequested = false;

    public List<BattleAction> turnQueue;

    public Sample[] playerSet, playerSamples;

    public NEnemy[] enemies;

    public int playerHealth, enemyHealth;

    List<BattleAction> savedTurnQueue;



    private void Awake()
    {

        current = this;
        //once all the minigames scenes are loaded, we can construct the queue of turns based on the players sets
        //for now we'll just construct a simple back and forth for 2 iterations
    }

    void Start()
    {
        //so first things first, we have to set the state to pre-battle
        StartCoroutine(MinigameManager.current.LoadMinigames());

        enemyHealth = enemies[0].hp;
        playerHealth = 10;

        //update health text
        NBattleUIManager.current.UpdateHealth();
    }

    public void InitQueue(Sample[] samples)
    {
        //        Debug.Log("Initializing the from the ui");
        playerSet = samples;
        InitQueue();

        //after the queue has been initialized, we can load the minigames requested
        //send this off to the minigame manager to handle, probably want a coroutine so we cant start the battle till these load
        // StartCoroutine(MinigameManager.current.LoadMinigames(turnQueue));

    }

    public void InitQueue()
    {
        //Debug.Log("Initializing the turnQueue");
        turnQueue = new List<BattleAction>();

        //so rather than building these randomly build them from the players set
        for (int b = 0; b < 2; b++)
        {
            for (int i = 0; i < playerSet.Length; i++)
            {
                PlayerBattleAction turn = new PlayerBattleAction();
                Sample s = Instantiate(playerSet[i]);
                turn.minigameSceneName = playerSet[i].sampleName;
                turn.playerOrEnemy = true;
                turn.sample = s;
                turnQueue.Add(turn);

                //do an enemy turn for this player turn
                EnemyBattleAction enemyTurn = new EnemyBattleAction();
                enemyTurn.playerOrEnemy = false;
                enemyTurn.dmg = enemies[0].attack;
                turnQueue.Add(enemyTurn);

            }
        }


        //store this turn queue as the one we can reset to
        savedTurnQueue = new List<BattleAction>();
        savedTurnQueue.AddRange(turnQueue);

        //Debug.Log(savedTurnQueue.Count);


        calculateQueueModifiers();
        //MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)turnQueue[0]).sample.miniGameSceneName);
        NBattleUIManager.current.InitTurnQueue(turnQueue);


    }

    //so once all the samples get added we then go through and calculate how all the samples will modify one another
    //basically each modifier needs the whole state of the queue, so we modify the state by handing it off to 
    //the sample one at a time right->left

    public void calculateQueueModifiers()
    {
        //go through each of the samples in the queue, and call their function, store the result, and then do it again for the next modifier
        //Debug.Log(turnQueue.Count);

        //+= 2 to skip the enemy turns
        for (int i = 0; i < turnQueue.Count; i += 2)
        {
            PlayerBattleAction a = (PlayerBattleAction)turnQueue[i];
            if (a.sample.functionName != "")
            {
                turnQueue = SampleEffects.processSampleEffect(turnQueue, i);
            }
        }


    }




    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (currentState == BattleState.Prebattle && Input.GetKeyDown(KeyCode.Space) && MinigameManager.current.minigamesLoaded)
        {
            StartBattle();
        }

        if ((currentState == BattleState.PlayerTurn || currentState == BattleState.EnemyTurn) && Input.GetKeyDown(KeyCode.Escape))
        {
            interludeRequested = true;
        }
    }

    void StartBattle()
    {


        //start the countin phase
        currentState = BattleState.Countin;
        //wait 1 bar then go into either player or enemy turn phase
        //this should be handled by a seperate static class that helps dispatch waits based on the audiosettings.dsp time
        WaitCallback methodToCall = ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, 2));

        //play the audio
        NBattleAudioManager.current.StartSong();
    }


    //so yea this basically handles damamge and shit
    public void EndTurn()
    {

        if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.block)
        {
            //TODO: add block
        }

        //TODO: this happens all after the minigame runs


        //so these should happen at the end of the phase, not the beggining 

        //do the players damage and the enemies damage
        playerHealth -= ((EnemyBattleAction)turnQueue[1]).dmg;

        if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.damage)
        {
            enemyHealth -= ((PlayerBattleAction)turnQueue[0]).sample.numericValue;
        }
        else if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.heal)
        {
            playerHealth += ((PlayerBattleAction)turnQueue[0]).sample.numericValue;
        }


        NBattleUIManager.current.UpdateHealth();

        //update the ui, 
        NBattleUIManager.current.UpdateTurnQueue();

        //remove the elements from the queue

        turnQueue.RemoveRange(0, 2);
    }


    //depending on battle phase we start a different persons turn
    public void ChangeTurn()
    {
        //so we need to check if we should refill the queue here



        //Debug.Log("turn changing");
        //turn off the active minigame canvas

        //if there was a turn previously running, end it
        if (currentState != BattleState.Prebattle && currentState != BattleState.Countin)
        {
            EndTurn();
        }


        //so if there's only one bar left of input just add on the whole turnqueue to the end to create a new loop
        if (turnQueue.Count / 2 <= 4)
        {
            Debug.Log("resetting the turnqueue");
            turnQueue.AddRange(savedTurnQueue);

            //reupadte the ui

            //so we only need to recalculate the queue if you do an interlude
            //calculateQueueModifiers();

            NBattleUIManager.current.InitTurnQueue(turnQueue);
        }


        if (turnQueue.Count > 0)
        {
            MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)turnQueue[0]).sample.miniGameSceneName);


            //TODO: this organizatio needs to be refactored
            //pull out and load the minigame
            MinigameManager.current.ActivateMinigame(((PlayerBattleAction)turnQueue[0]).sample.miniGameSceneName);

            //load the next minigame in the queue



        }


        switch (currentState)
        {
            case BattleState.Countin:
                currentState = BattleState.PlayerTurn;
                break;
            case BattleState.PlayerTurn:
                currentState = BattleState.EnemyTurn;
                break;
            case BattleState.EnemyTurn:
                //so here is where we check if an interlude is requested, if so we go into an interlude
                currentState = interludeRequested ? BattleState.Interlude : BattleState.PlayerTurn;
                interludeRequested = false;
                break;
            case BattleState.Interlude:
                //if we're in an interlude then we just go back to the players turn
                currentState = BattleState.PlayerTurn;
                break;
        }



        //after the turn is changed, wait however many bars and then do it all again woo
        WaitCallback methodToCall = ChangeTurn;
        StartCoroutine(TimeManager.barWait(methodToCall, 2));
    }

    //also works for healing
    public void DmgPlayer(int amount)
    {
        playerHealth -= amount;
        NBattleUIManager.current.UpdateHealth();
    }

    public void HealPlayer(int amount)
    {
        playerHealth += amount;
        NBattleUIManager.current.UpdateHealth();
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