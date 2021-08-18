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
        //Debug.Log("NCE");
        //BattleUIManager.current.UpdateMetronome();

        //check for any shit that needs to happen on like the 1
        // Debug.Log(currentBeat);
        if ((currentBeat) % 4 == 1)
        {
            // //Debug.Log(currentBeat);
            // Debug.Break();

            //so this is on the 1

            if (bpmSwitchRequested)
            {
                SetCurrentSongInfo(newBPM);
            }
        }


        foreach (BeatCallBackFunction b in beatCallbacks)
        {
            //Debug.Log(b);
            b();
        }
    }


    public static IEnumerator barWait(BattleManager.WaitCallback methodToCall, int numBars = 1)
    {
        //start the wait based on the current dspTime 
        double waitStart = AudioSettings.dspTime;


        //for now we're just hard coding for 4 seconds
        //TODO: make this dynamic based on bpm

        float barLength = (60 / currentSongBpm) * 4 * numBars;

        //so the bar length is the beat length * 4
        //the beat length is the bpm/60

        double waitEnd = AudioSettings.dspTime + barLength;

        Debug.Log("doing bar wait for " + numBars.ToString());

        Debug.Log(waitEnd);

        yield return new WaitUntil(() => AudioSettings.dspTime >= waitEnd);
        Debug.Log("bar wait over");

        methodToCall();
    }


    //TODO: make this happen on the 1, not the 4
    public static void SetCurrentSongInfo(float bpm)
    {
        Debug.Log("setting bpm to " + bpm);
        //Debug.Break();
        currentSongBpm = bpm;
        timePerBeat = 60f / bpm;
    }

    public static void SetBattleStart(double start)
    {
        currentBeat = 0;
        battleStartTime = start;
        // Debug.Log(currentBeat);
        // Debug.Log(timePerBeat);
        currentBeatDSPTime = battleStartTime + (currentBeat * timePerBeat);
        //Debug.Log(currentBeatDSPTime);
    }

    public static float GetTimePerBeat()
    {
        return 60.0f / currentSongBpm;
    }

    public static double GetNextBeatDSPTime()
    {
        return currentBeatDSPTime + timePerBeat;
    }

    static bool bpmSwitchRequested;
    static float newBPM;
    public static void BPMSwitch(float bpm)
    {
        //so this should either start a coroutine or have the beatupdate do something

        //lets try doing it with the beat update

        //so a bpm switch is requested
        newBPM = bpm;
        bpmSwitchRequested = true;

    }

}
