using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    //so here we're going to need to get some bpm info sorted before we do anything

    public static float currentSongBpm;

    public static double battleStartTime;

    public static IEnumerator barWait(NBattleManager.WaitCallback methodToCall)
    {
        //start the wait based on the current dspTime 
        double waitStart = AudioSettings.dspTime;

        //for now we're just hard coding for 4 seconds
        //TODO: make this dynamic based on bpm

        float barLength = (60 / currentSongBpm) * 4;

        //so the bar length is the beat length * 4
        //the beat length is the bpm/60

        double waitEnd = AudioSettings.dspTime + barLength;


        yield return new WaitUntil(() => AudioSettings.dspTime >= waitEnd);

        methodToCall();
    }

    public static void SetCurrentSongInfo(float bpm)
    {
        currentSongBpm = bpm;
    }

    public static void SetBattleStart()
    {
        battleStartTime = AudioSettings.dspTime;
    }

}
