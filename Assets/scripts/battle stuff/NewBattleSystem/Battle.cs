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

    public void InitTurnQueue(Sample[] playerSamples)
    {
        turnQueue = new List<BattleAction>();

        for (int i = 0; i < playerSamples.Length; i++)
        {
            PlayerBattleAction turn = new PlayerBattleAction();
            Sample s = new Sample();

            s = playerSamples[i].DeepCopy(s);

            turn.minigameSceneName = playerSamples[i].sampleName;
            turn.playerOrEnemy = true;
            turn.sample = s;
            turnQueue.Add(turn);

            //do an enemy turn for this player turn
            EnemyBattleAction enemyTurn = new EnemyBattleAction();
            enemyTurn.playerOrEnemy = false;
            enemyTurn.dmg = enemies[0].attack;
            turnQueue.Add(enemyTurn);
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

    public void ChangeTurn()
    {

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
            BattleUIManager.current.InitTurnQueue(turnQueue);
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
                currentState = manager.interludeRequested ? BattleState.Interlude : BattleState.PlayerTurn;
                manager.interludeRequested = false;
                break;
            case BattleState.Interlude:
                //if we're in an interlude then we just go back to the players turn
                currentState = BattleState.PlayerTurn;
                break;
        }

        //so the audio should probably switch here?


        manager.RefreshTurn();
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
        manager.PlayerHealth -= ((EnemyBattleAction)turnQueue[1]).dmg;

        if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.damage)
        {
            manager.EnemyHealth -= ((PlayerBattleAction)turnQueue[0]).sample.numericValue;
        }
        else if (((PlayerBattleAction)turnQueue[0]).sample.sampleType == SampleType.heal)
        {
            manager.PlayerHealth += ((PlayerBattleAction)turnQueue[0]).sample.numericValue;
        }

        //BattleUIManager.current.UpdateHealth();
        //update the ui, 
        BattleUIManager.current.UpdateTurnQueue();

        //remove the elements from the queue
        turnQueue.RemoveRange(0, 2);

        //so i guess here is where we schedule the next song to play?

        //so we just schedule the next song in the queue to start playing i guess
        //first things first get the minigames just like lasting as long as the track actually specifies

    }

    public Track getNextTrack()
    {
        if (turnQueue.Count <= 0)
        {
            Debug.LogWarning("O FUCK NO TRACKS LOL");
            return null;
        }

        return ((PlayerBattleAction)turnQueue[0]).sample.sampleTrack;
    }


}
