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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
