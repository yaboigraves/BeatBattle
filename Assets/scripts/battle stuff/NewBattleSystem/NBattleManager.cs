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

public class NBattleManager : MonoBehaviour
{

    public static NBattleManager current;
    public delegate void WaitCallback();

    public BattleState currentState = BattleState.Prebattle;

    public bool interludeRequested = false;

    public List<BattleTurn> turnQueue;

    private void Awake()
    {

        current = this;
        //once all the minigames scenes are loaded, we can construct the queue of turns based on the players sets
        //for now we'll just construct a simple back and forth for 2 iterations

    }

    public void InitQueue()
    {
        Debug.Log("Initializing the turnQueue");
        turnQueue = new List<BattleTurn>();

        for (int i = 1; i <= 4; i++)
        {
            BattleTurn turn = new BattleTurn();
            turn.minigameSceneName = "ass";
            turn.damage = Random.Range(1, 4);
            if (i % 2 == 0)
            {
                turn.playerOrEnemy = false;
            }
            else
            {
                turn.playerOrEnemy = true;
            }
            turnQueue.Add(turn);
        }
    }


    void Start()
    {
        //so first things first, we have to set the state to pre-battle
        StartCoroutine(MinigameManager.current.LoadMinigames());
    }

    // Update is called once per frame
    void Update()
    {

        UpdateState();
    }

    void UpdateState()
    {
        if (currentState == BattleState.Prebattle && Input.GetKeyDown(KeyCode.Space))
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
        StartCoroutine(TimeManager.barWait(methodToCall));
    }


    //depending on battle phase we start a different persons turn
    public void ChangeTurn()
    {

        if (turnQueue.Count > 0)
        {
            //debug log the new turn
            BattleTurn currentTurn = turnQueue[0];

            Debug.Log("currentTurn damg : " + currentTurn.damage);
            Debug.Log("currentTurn name : " + currentTurn.minigameSceneName);
            Debug.Log("player turn? : " + currentTurn.playerOrEnemy);
            turnQueue.RemoveAt(0);
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
        StartCoroutine(TimeManager.barWait(methodToCall));
    }
}

public enum BattleState
{
    Prebattle,
    Countin,
    PlayerTurn,
    EnemyTurn,
    Interlude
}





//TODO: probably move this to another file but yea
//so battleturns represent each object in the queue of minigames that you build
//you can move stuff around the queue at runtime in interlude phases
//we start you with a default queue of turns but you can customize these as much as you want into "sets" 


//these fields will get fleshed out later but yea basically this should be a modular component that is effected
//by its context within the queue 
public class BattleTurn
{
    public string minigameSceneName;
    public bool playerOrEnemy;
    public int damage;
}