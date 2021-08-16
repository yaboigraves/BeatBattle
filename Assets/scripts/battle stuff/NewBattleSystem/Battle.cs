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




}
