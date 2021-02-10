using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO:
/*
    so a better way to manage beat ticks would be to preplan out a list of times when the beat switches
    essentially mark each point that a new beat comes 
    during each phase we can track the beat by measuring the last positions time difference to the next marked position

    -this array would need to be constructed when the track queue is constructed
    -essentially as long as we know the bpm info we know exactly when stuff should be triggered
    this will make bpm measurements easier too, and bpm switchups should work fine?
    we'll see...

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
    public static double songPosition;

    //Current song position, in beats
    public static double songPositionInBeats;

    //How many seconds have passed since the song started
    public static double dspSongTime;

    // //an AudioSource attached to this GameObject that will play the music.
    // public AudioSource audioSource;

    public static float debugDSPTIME;

    public static float currentPlayerBars;

    public static bool trackStarted = false;

    public static bool countingIn = false;

    public static float turnBeatCounter;

    public static int beatsBeforeNextPhase;

    public static double battleDSPStartTime;

    public static double deltaDSPTime, lastDSPTime;

    public static double[] beatTimeLine;

    public static int nextBeatIndex = 1;

    public static void SetTrackData(TrackData track)
    {
        //audioSource.clip = track.trackClip;
        songBpm = track.bpm;
        secPerBeat = 60f / songBpm;

        //check this out it may be a cause of alot of fuckups
        // dspSongTime = AudioSettings.dspTime;
    }

    public static void setBeatsBeforeNextPhase(int beats)
    {
        beatsBeforeNextPhase = beats;
    }

    public static double[] CalculateTrackBeatTimeLine(Queue<Track> trackQueue)
    {
        List<double> beatTimeLine = new List<double>();

        int numBeats = 0;

        Track[] trackArray = trackQueue.ToArray();


        double beatTime = 0;
        beatTimeLine.Add(beatTime);

        //instead of using i as a marker for where the beat should be use time 
        foreach (Track t in trackArray)
        {
            for (int i = 0; i < t.randomTrackData.numBeats; i++)
            {

                // double offsetTime = numBeats * (60 / t.randomTrackData.bpm);
                //so the time of each beat is i * the seconds per beat
                // beatTimeLine.Add(offsetTime + ( * (60 / t.randomTrackData.bpm)));

                double timePerBeat = 60 / t.randomTrackData.bpm;
                beatTimeLine.Add(beatTime + timePerBeat);

                beatTime += timePerBeat;

            }


            //loop through the transition
            for (int i = 0; i < t.randomTransitionData.numBeats; i++)
            {
                // double offsetTime = numBeats * (60 / t.randomTransitionData.bpm);

                // beatTimeLine.Add(offsetTime + (i * (60 / t.randomTransitionData.bpm)));

                double timePerBeat = 60 / t.randomTransitionData.bpm;
                beatTimeLine.Add(beatTime + timePerBeat);
                beatTime += timePerBeat;
            }
            // numBeats += t.randomTransitionData.numBeats;

        }



        TrackTimeManager.beatTimeLine = beatTimeLine.ToArray();

        return beatTimeLine.ToArray();
    }



    public static double startUpTime;

    public static float currentTurnStartBeat = 0;



    public static void ManualUpdate()
    {
        deltaDSPTime = AudioSettings.dspTime - lastDSPTime;
        //look at the event queue and see if anything in there needs to be processed
        CheckQueue();

        debugDSPTIME = (float)AudioSettings.dspTime;


        if (trackStarted)
        {
            beatTick();
            songPosition = (AudioSettings.dspTime - startUpTime - (dspSongTime));




            //so this is going to need to be calculated a little more dynamically now
            //we're going to need to fill the bucket with delta times essentially, assuming rounding error isnt too bad

            //if this can be accurate constantly we're good

            //TODO: recalcualte this to be dynamic depending on bpm

            //we can use the beatTimeLine for this 

            //last whole beat + percentage we are through the current beat
            //convert the seconds from the last wholebeat into beats

            int lastWholebeat = nextBeatIndex - 1;

            double lastBeatTime = AudioSettings.dspTime - beatTimeLine[lastWholebeat];
            double nextBeatTime = lastBeatTime + beatTimeLine[nextBeatIndex];

            double beatPercentage = (AudioSettings.dspTime - lastBeatTime) / nextBeatTime;

            songPositionInBeats = lastWholebeat + beatPercentage;
            //Debug.Log(songPositionInBeats);

        }

        //this i think is deprecated, can probably get removed
        if (doingWait)
        {
            MoveIndicatorContainerForWait();
        }


        if (countingIn)
        {

            //time to end the count
            if (AudioSettings.dspTime > waitTimeOver)
            {
                startUpTime = AudioSettings.dspTime - dspSongTime;
                songPositionInBeats = 0;
                // audioSource.Play();

                //BattleTrackManager.current.playCurrentTrack();

                //tell the track manager to play the current mix
                trackStarted = true;
                countingIn = false;
                Debug.Log("starting wait is over");
                //BattleTrackManager.current.NextBattlePhase();
            }
        }

        lastDSPTime = AudioSettings.dspTime;
    }





    //TODO: rewrite this to work more closely with audioSettings dsp time
    public static void beatTick()
    {
        if (nextBeatIndex >= beatTimeLine.Length)
        {
            return;
        }



        if (AudioSettings.dspTime > startUpTime + beatTimeLine[nextBeatIndex])
        {


            //call all the stuff we need to call for a beattick 
            BattleManager.current.VibeUpdate();
            BattleManager.current.UpdateGearPipeline();

            BattleUIManager.current.UpdateMetronome(((Mathf.FloorToInt((float)songPositionInBeats))), false);


            if (BattleManager.current.battleType == BattleManager.BattleType.quickMix)
            {
                beatsBeforeNextPhase--;
                if (beatsBeforeNextPhase <= 0)
                {
                    //we're moving to a new phase so we have to switch audioclips and possibly bpms
                    //BattleTrackManager.current.NextBattlePhase();
                }
            }
            else if (BattleManager.current.battleType == BattleManager.BattleType.longMix)
            {

            }

            nextBeatIndex++;


            Debug.Log("beat tick, next beat shoould be " + (startUpTime + beatTimeLine[nextBeatIndex]));


            //spawn a bar
            //IndicatorManager.current.spawnBar((float)songPositionInBeats + IndicatorManager.current.barSpawnPosition);
        }
    }


    public static float dspTimeDifferenceFromStart;
    public static void startTrackTimer()
    {
        trackStarted = true;
        battleDSPStartTime = AudioSettings.dspTime;

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
        waitTimeOver = AudioSettings.dspTime + 4 * secPerBeat;

        // BattleTrackManager.current.mix1AudioSource.PlayScheduled(waitTimeOver);

        //so now this is going to need to essentialy trigger a boolean that is just checked in the manual update

        countingIn = true;
    }



    //starts shit but waits 4 beats before resetting all the data back to 0

    static double waitTimeOver;
    static bool doingWait = false;
    static float waitTimeStart;
    public static IEnumerator beatWaitRoutine(int numBeats)
    {

        //so this needs to figure out basically jsut how much time needs to pass from now till 4 beats from now 
        //first lets figure out when NOW IS 

        double songCurrentBeatPosition = songPositionInBeats;
        doingWait = true;


        yield return new WaitUntil(() => AudioSettings.dspTime > waitTimeOver);
        BattleManager.current.SetBattleStarted(true);
        doingWait = false;
        //yield return null;

        //startup time is the difference in dsptime of the song + the amount of time it currently is 
        startUpTime = AudioSettings.dspTime - dspSongTime;
        songPositionInBeats = 0;
        // audioSource.Play();

        //BattleTrackManager.current.playCurrentTrack();

        //tell the track manager to play the current mix
        trackStarted = true;
        dspSongTime = AudioSettings.dspTime; dspSongTime = AudioSettings.dspTime;

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


    public static Queue<TrackEvent> eventQueue = new Queue<TrackEvent>();

    public static void AddEvent(string eventName, double eventTime)
    {
        Debug.Log(eventName + " queued for " + (AudioSettings.dspTime + eventTime));
        eventQueue.Enqueue(new TrackEvent(eventName, AudioSettings.dspTime + eventTime));
    }

    //this will get called in the manual update as often as possible
    public static void CheckQueue()
    {
        if (eventQueue.Count <= 0) { return; }

        TrackEvent nextEvent = eventQueue.Peek();

        if (nextEvent.time <= AudioSettings.dspTime)
        {
            Debug.Log("EVENT TRIGGERED at - " + nextEvent.time + " DSP TIME IS - " + AudioSettings.dspTime);


            //dequeu the event 
            nextEvent = eventQueue.Dequeue();

            //process the event
            if (nextEvent.eventName == "nextPhase")
            {
                //call the next phase code to run
                BattleTrackManager.current.NextBattlePhase();
            }

            if (nextEvent.eventName == "bpmSwitch")
            {
                Debug.Log("SWAPPING BPM");
                // Debug.Break();
                SetTrackData(BattleTrackManager.current.nextTrack.randomTrackData);

            }
        }
    }





}

public class TrackEvent
{
    public string eventName;
    public double time;

    public TrackEvent(string eventName, double time)
    {
        this.eventName = eventName;
        this.time = time;
    }
}