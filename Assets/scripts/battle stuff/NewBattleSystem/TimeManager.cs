using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    //so here we're going to need to get some bpm info sorted before we do anything

    public static float currentSongBpm;

    public static double battleStartTime;

    public static float timePerBeat;


    public static int currentBeat = 0;
    public static double currentBeatDSPTime = 0;

    //TODO: 6/17 create a list of delegate functions that need to get called every beat that we can assign stuff too


    public delegate void BeatCallBackFunction();

    public static List<BeatCallBackFunction> beatCallbacks = new List<BeatCallBackFunction>();

    public static void BeatCallBack()
    {
        currentBeat++;
        currentBeatDSPTime = battleStartTime + (currentBeat * timePerBeat);
        Debug.Log("NCE");
        //NBattleUIManager.current.UpdateMetronome();

        foreach (BeatCallBackFunction b in beatCallbacks)
        {
            b();
        }
    }


    public static IEnumerator barWait(NBattleManager.WaitCallback methodToCall, int numBars = 1)
    {
        //start the wait based on the current dspTime 
        double waitStart = AudioSettings.dspTime;

        //for now we're just hard coding for 4 seconds
        //TODO: make this dynamic based on bpm

        float barLength = (60 / currentSongBpm) * 4 * numBars;

        //so the bar length is the beat length * 4
        //the beat length is the bpm/60

        double waitEnd = AudioSettings.dspTime + barLength;


        yield return new WaitUntil(() => AudioSettings.dspTime >= waitEnd);

        methodToCall();
    }

    public static void SetCurrentSongInfo(float bpm)
    {
        currentSongBpm = bpm;
        timePerBeat = 60f / bpm;
    }

    public static void SetBattleStart(double start)
    {
        battleStartTime = start;
    }

    public static float GetTimePerBeat()
    {
        return 60.0f / currentSongBpm;
    }

}
