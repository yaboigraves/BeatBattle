using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SampleEffects
{
    public delegate List<BattleTurn> SampleEffect(List<BattleTurn> turnQueue);
    public static Dictionary<string, SampleEffect> sampleEffects = new Dictionary<string, SampleEffect>
    {
        {"dmgBuff",dmgBuff},
        {"heal",heal}
    };


    public static List<BattleTurn> processSampleEffect(Sample s, List<BattleTurn> turnQueue)
    {
        //look at the effect of the sample and call that function on the queue
        //repeat etc

        return sampleEffects[s.functionName](turnQueue);


    }



    public static List<BattleTurn> dmgBuff(List<BattleTurn> turnQueue)
    {
        Debug.Log("dmgBuff Effect applying");
        return turnQueue;
    }

    public static List<BattleTurn> heal(List<BattleTurn> turnQueue)
    {
        Debug.Log("heal effect applying");
        return turnQueue;
    }

}
