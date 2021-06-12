using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SampleEffects
{

    //  LEFT -> RIGHT ALWAYS ME BOI


    public delegate List<BattleAction> SampleEffect(List<BattleAction> turnQueue, int index);
    public static Dictionary<string, SampleEffect> sampleEffects = new Dictionary<string, SampleEffect>
    {
        {"dmgBuff",dmgBuff}
    };

    public static List<BattleAction> processSampleEffect(List<BattleAction> turnQueue, int index)
    {
        //look at the effect of the sample and call that function on the queue
        //repeat etc

        return sampleEffects[((PlayerBattleAction)turnQueue[index]).sample.functionName](turnQueue, index);
    }

    //TODO: reimpliment this
    public static List<BattleAction> dmgBuff(List<BattleAction> turnQueue, int index)
    {
        Debug.Log("dmgBuff Effect applying");

        //apply lEFT TO RIGHT

        //so left and right elements are a spaced out by 2 now

        //check for a left element
        if (index >= 2)
        {
            ((PlayerBattleAction)turnQueue[index - 2]).sample.numericValue += 3;

        }

        //check for a right element
        if (index <= turnQueue.Count - 3)
        {
            ((PlayerBattleAction)turnQueue[index + 2]).sample.numericValue += 3;
        }

        return turnQueue;
    }



}
