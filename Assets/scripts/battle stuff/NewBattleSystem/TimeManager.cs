using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{

    public static IEnumerator barWait(NBattleManager.WaitCallback methodToCall)
    {
        //start the wait based on the current dspTime 
        double waitStart = AudioSettings.dspTime;

        //for now we're just hard coding for 4 seconds
        //TODO: make this dynamic based on bpm
        double waitEnd = AudioSettings.dspTime + 4;


        yield return new WaitUntil(() => AudioSettings.dspTime >= waitEnd);

        methodToCall();
    }

}
