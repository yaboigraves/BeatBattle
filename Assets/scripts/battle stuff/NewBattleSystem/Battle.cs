using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//so the battle object will just be an object held by the battle manager that tracks all the stat info n shit
//can be updated, basically a living state machine which we can move around

[System.Serializable]
public class Battle
{
    //so what info does the battle need to track?
    //the current beat/bar we're on 
    //the current minigame
    //the current track
    //what battle state we're in
    //the player and enemy health
    //probably some other stuff
    //OH definitly the queue of tracks

    //so yeah alot of responsibilities from the battlemanager can get moved into here, mostly for tracking
    //the battle manager can basically just be a storehouse for this object and can still handle running the update
    public BattleState currentState = BattleState.Prebattle;
    public List<BattleAction> turnQueue, savedTurnQueue;
    public NEnemy[] enemies;
    public BattleManager manager;
    //so the enemy turns are going to need to later be pre-generated for now just use the basic one
    bool firstRound = true;

    public Battle()
    {
        //should probably look into the enemies for what kind of abilities they make
        turnQueue = new List<BattleAction>();
        InitEnemyTurns();
    }

    public void InitTurnQueue(Sample[] playerSamples)
    {
        firstRound = true;

        for (int i = 0; i < playerSamples.Length; i += 1)
        {
            PlayerBattleAction turn = new PlayerBattleAction();
            Sample s = (Sample)ScriptableObject.CreateInstance("Sample");

            s = playerSamples[i].DeepCopy(s);

            turn.minigameSceneName = playerSamples[i].sampleName;
            turn.playerOrEnemy = true;
            turn.sample = s;
            turnQueue.Add(turn);
            Debug.Log("added player turn");


            //TODO: ok gotta rewrite how enemy turns work a little bit, turns out these will need to actually be initialized BEFORE the player

            // //do an enemy turn for this player turn
            // EnemyBattleAction enemyTurn = new EnemyBattleAction();
            // enemyTurn.playerOrEnemy = false;
            // enemyTurn.dmg = enemies[0].attack;

            turnQueue.Add(enemyBattleActions[i]);


        }

        savedTurnQueue = new List<BattleAction>();
        savedTurnQueue.AddRange(turnQueue);

        calculateQueueModifiers();
        BattleUIManager.current.InitTurnQueue(turnQueue);
    }

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

    //ok so if this is to be based off of the beat timeline then we can just assign it to the callbacks every n number of beats
    public void ChangeTurn()
    {
        // Debug.Log("changing turn!");
        // Debug.Break();

        //mark in the time manager that a minigame started


        if (turnQueue.Count > 0)
        {
            if (!firstRound)
            {
                TimeManager.MarkMinigameStartIndex(((PlayerBattleAction)turnQueue[0]).sample);
                // Debug.Log(turnQueue.Count);
                EndTurn();
                MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)turnQueue[0]).sample);
                //MinigameManager.current.ActivateMinigameScene(((PlayerBattleAction)turnQueue[0]).sample.miniGameSceneName);

            }
            else
            {
                firstRound = false;
                MinigameManager.current.PreloadMiniGame(((PlayerBattleAction)turnQueue[0]).sample);
            }
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
                currentState = manager.interludeRequested ? BattleState.Interlude : BattleState.PlayerTurn;
                manager.interludeRequested = false;
                break;
            case BattleState.Interlude:
                //if we're in an interlude then we just go back to the players turn
                currentState = BattleState.PlayerTurn;
                break;
        }

        //so the audio should probably switch here?

        //TODO: so this may end up fucking stuff up because of bpm switches now, need to re-look
        //manager.RefreshTurn();
    }

    //so yea this basically handles damamge and shit
    public void EndTurn()
    {
        // if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.block)
        // {
        //     //TODO: add block
        // }

        //TODO: this happens all after the minigame runs

        //so these should happen at the end of the phase, not the beggining 

        //do the players damage and the enemies damage
        manager.PlayerHealth -= ((EnemyBattleAction)turnQueue[1]).dmg;

        if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.damage)
        {
            manager.EnemyHealth -= ((PlayerBattleAction)turnQueue[0]).sample.numericValue;
        }
        else if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.heal)
        {
            manager.PlayerHealth += ((PlayerBattleAction)turnQueue[0]).sample.numericValue;


        }

        turnQueue.RemoveRange(0, 2);
        BattleUIManager.current.UpdateTurnQueue();

        //turn off the dmg report text and reset it to 0
        BattleUIManager.current.ToggleReportText(false);
    }


    public Track getCurrentTrack()
    {
        if (turnQueue.Count <= 0)
        {
            Debug.LogWarning("O FUCK NO TRACKS LOL");
            return null;
        }

        return ((PlayerBattleAction)turnQueue[0]).sample.sampleTrack;
    }

    public Track getNextTrack()
    {
        if (turnQueue.Count <= 2)
        {
            Debug.LogWarning("O FUCK NO TRACKS LOL");
            return null;
        }

        return ((PlayerBattleAction)turnQueue[2]).sample.sampleTrack;
    }


    //so this gets read by the ui manager when displaying the turns to the user
    public List<EnemyBattleAction> enemyBattleActions;
    public void InitEnemyTurns()
    {
        // //do an enemy turn for this player turn
        // EnemyBattleAction enemyTurn = new EnemyBattleAction();
        // enemyTurn.playerOrEnemy = false;
        // enemyTurn.dmg = enemies[0].attack;
        // turnQueue.Add(enemyTurn);
        enemyBattleActions = new List<EnemyBattleAction>();

        for (int i = 0; i < 4; i++)
        {
            EnemyBattleAction enemyTurn = new EnemyBattleAction();
            enemyTurn.playerOrEnemy = false;
            enemyTurn.dmg = 1;
            enemyBattleActions.Add(enemyTurn);
        }


    }
}
