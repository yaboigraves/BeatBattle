using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO:
/*
   holy fuck this needs to get refactored lol

   alot of cobweb code thats all its pretty functional could just be waaaaay cleaner
*/



public static class TrackTimeManager
{

    // public static TrackTimeManager current;
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public static float songBpm;

    //The number of seconds for each song beat
    public static float secPerBeat;

    //Current song position, in seconds
    public static float songPosition;

    //Current song position, in beats
    public static float songPositionInBeats;

    //How many seconds have passed since the song started
    public static float dspSongTime;

    // //an AudioSource attached to this GameObject that will play the music.
    // public AudioSource audioSource;

    public static float debugDSPTIME;

    public static float currentPlayerBars;

    public static bool trackStarted = false;

    public static bool countingIn = false;

    public static float turnBeatCounter;

    public static int beatsBeforeNextPhase;

    public static void SetTrackData(TrackData track)
    {
        //audioSource.clip = track.trackClip;
        songBpm = track.bpm;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
    }

    public static void setBeatsBeforeNextPhase(int beats)
    {
        beatsBeforeNextPhase = beats;
    }



    public static float startUpTime;

    public static float currentTurnStartBeat = 0;



    public static void ManualUpdate()
    {

        beatTick();
        debugDSPTIME = (float)AudioSettings.dspTime;

        //update ui with data 

        // if (songPositionInBeats >= 16)
        // {
        //     //turn change 
        //     // currentTurnStartBeat = songPositionInBeats;
        //     //BattleManager.current.changeTurn();
        //     BattleCameraController.current.CameraSwitchup();
        // }

        if (trackStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - startUpTime - (dspSongTime));
            songPositionInBeats = (songPosition / secPerBeat);
            BattleUIManager.current.UpdateMetronome(((Mathf.FloorToInt(songPositionInBeats)) % 4), false);
        }

        //this i think is deprecated, can probably get removed
        if (doingWait)
        {

            MoveIndicatorContainerForWait();
        }



        if (countingIn)
        {

            //time to end the count
            if ((float)AudioSettings.dspTime > waitTimeOver)
            {
                startUpTime = (float)AudioSettings.dspTime - dspSongTime;
                songPositionInBeats = 0;
                // audioSource.Play();

                //BattleTrackManager.current.playCurrentTrack();

                //tell the track manager to play the current mix
                trackStarted = true;
                countingIn = false;
                Debug.Log("starting wait is over");
                BattleTrackManager.current.NextBattlePhase();
            }
        }
    }


    static float lastTick = 0;
    public static void beatTick()
    {
        if (lastTick + 1 < songPositionInBeats)
        {
            //call all the stuff we need to call for a beattick 
            BattleManager.current.VibeUpdate();
            BattleManager.current.UpdateGearPipeline();

            if (BattleManager.current.battleType == BattleManager.BattleType.quickMix)
            {
                beatsBeforeNextPhase--;
                if (beatsBeforeNextPhase <= 0)
                {
                    //we're moving to a new phase so we have to switch audioclips and possibly bpms
                    BattleTrackManager.current.NextBattlePhase();
                }
            }
            else if (BattleManager.current.battleType == BattleManager.BattleType.longMix)
            {

            }

            lastTick = songPositionInBeats;



            //spawn a bar
            IndicatorManager.current.spawnBar(songPositionInBeats + IndicatorManager.current.barSpawnPosition);

            BattleTrackManager.current.checkForTransition();



        }
    }


    public static float dspTimeDifferenceFromStart;
    public static void startTrackTimer()
    {
        trackStarted = true;

        //TODO: so also need to take note of the current difference in dsp time
    }

    public static void stopTrackTimer()
    {
        trackStarted = false;
    }

    public static void resetTrackTimer()
    {
        songPosition = 0;
        songPositionInBeats = 0;
    }


    //TODO: reimpliment the beat wait using updates from audiosettings.dsp time rather than a coroutine
    public static void beatWait(int numBeats)
    {
        //dspTimeDifferenceFromStart = (float)AudioSettings.dspTime - dspSongTime;

        //so we want to have 4 beats of time progress
        //as this time progresses need to lerp the indicator container down 4 units 
        //so lerp(start,start -4, currentBeat/4)
        Debug.Log("wait is starting");
        waitTimeOver = (float)AudioSettings.dspTime + 4 * secPerBeat;

        // BattleTrackManager.current.mix1AudioSource.PlayScheduled(waitTimeOver);

        //so now this is going to need to essentialy trigger a boolean that is just checked in the manual update

        countingIn = true;
    }



    //starts shit but waits 4 beats before resetting all the data back to 0

    static float waitTimeOver;
    static bool doingWait = false;
    static float waitTimeStart;
    public static IEnumerator beatWaitRoutine(int numBeats)
    {

        //so this needs to figure out basically jsut how much time needs to pass from now till 4 beats from now 
        //first lets figure out when NOW IS 

        float songCurrentBeatPosition = songPositionInBeats;
        doingWait = true;


        yield return new WaitUntil(() => AudioSettings.dspTime > waitTimeOver);

        doingWait = false;
        //yield return null;

        //startup time is the difference in dsptime of the song + the amount of time it currently is 
        startUpTime = (float)AudioSettings.dspTime - dspSongTime;
        songPositionInBeats = 0;
        // audioSource.Play();

        BattleTrackManager.current.playCurrentTrack();

        //tell the track manager to play the current mix
        trackStarted = true;
    }

    public static GameObject currIndicatorContainer;
    static Vector3 indicatorStartPos;
    public static void setCurrIndicatorContainer(GameObject indiContainer)
    {
        currIndicatorContainer = indiContainer;
    }

    public static void MoveIndicatorContainerForWait()
    {
        float movePercent = (float)((AudioSettings.dspTime - waitTimeStart) / (waitTimeOver - waitTimeStart));
        //lerp the indicator container between its spawn position and 0 based on where audio time is between the waittimeover variable
        //currIndicatorContainer.transform.position = Vector3.Lerp(new Vector3(0, 4, 0), new Vector3(0, 0, 0), movePercent);
    }

    public static float GetDSPTimeForNextPlay(int length)
    {
        //so lets see if this works
        float nexttime = (float)AudioSettings.dspTime + ((length) * secPerBeat);
        Debug.Log("scheduling track to play at " + nexttime.ToString());
        return nexttime;
    }


    //notes 
    //we can now just use the songPositionInBeats variable to set the position of indicatoes = to that rather than translating them 
    //this will yield perfect syncing of the track time and the indicators
}
