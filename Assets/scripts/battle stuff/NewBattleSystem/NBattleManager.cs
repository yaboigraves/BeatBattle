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

    public delegate void WaitCallback();

    public BattleState currentState = BattleState.Prebattle;

    public bool interludeRequested = false;
    void Start()
    {
        //so first things first, we have to set the state to pre-battle
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