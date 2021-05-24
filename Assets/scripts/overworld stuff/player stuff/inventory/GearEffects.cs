using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//so all of these will be called in the order they're organized in the inventory during battle
//every battleupdate they need to grab relevant info (probably gonna make an object that we can pass)
//that just tells the effects stats about the battle (time,vibe,streaks) etc and then applies effects to the battle


public static class GearEffects
{

    public delegate void GearEffect(BattleManager.BattleState state);

    public static Dictionary<string, GearEffect> gearEffectDictionary = new Dictionary<string, GearEffect>()
    {
        {"sp404",sp404}
    };




    public static void sp404(BattleManager.BattleState state)
    {
        //so this will make it so that every 4th beat you get in a row does 4x damage
        //Debug.Log("sp404 effect");
        if (state.beatStreak != 0 && state.beatStreak % 4 == 0)
        {

            //apply a buff of 4x damage for one hit to the player
            BattleManager.current.sp404Buff = true;

            //we also need to tell the ui that the effect is active
            //will need a dictionary/container to hold the gear icons
            BattleUIManager.current.ToggleUiIconBorder("sp404", true);
        }
    }







}