
using System.Collections.Generic;
using UnityEngine;


public class BattleTrackManager : MonoBehaviour
{




    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

    public AudioSource mix1AudioSource, mix2AudioSource, transitionAudioSource;
    public Track currentTrack, nextTrack;
    public static BattleTrackManager current;

    public bool paused;
    public float currentBpm;
    public Track[] playerTracks;
    public Track[] testPlayerTracks;
    public Track[] testEnemyTracks;
    //if this is enabled play a sound when the metronome ticks
    public bool metronomeAudio;
    public int totalBeats;
    public Track playerSelectedTrack;
    //used by the track time manager to hopefully setup bar waits between tracks coming out
    public float countInBeats = 4;


    //=====================================

    public Queue<Track> trackQueue = new Queue<Track>();



    private void Awake()
    {
        current = this;
        AudioClip audioClip;
        if (TrackManager.current == null)
        {

            // playerSelectedTrack = testPlayerTracks[0];
            // currentTrack = playerSelectedTrack;

            // audioClip = testPlayerTracks[0].tracks[0].trackClip;
        }
        else
        {
            // playerTracks = GameManager.current.player.GetComponent<PlayerInventory>().battleEquippedTracks;
            // currentTrack = playerTracks[0];
            // playerSelectedTrack = playerTracks[0];
            // audioClip = currentTrack.trackClip;
        }

        //print("audiosource" + audioSource.name);

        // currentBpm = currentTrack.bpm;


        // mix1AudioSource.clip = audioClip;


        //commented out to test timescale shit 
        //beatDeltaTime = (1 / currentBpm) * 60;

        //uniform speed for timescale change
        // beatDeltaTime = 1;
    }


    private void Start()
    {

        paused = true;
    }

    public void StartBattle()
    {
        paused = false;

        //audioSource.Play();
        BattleManager.current.battleStarted = true;

        //so from right here, going to start weaving in the track time manager
        TrackTimeManager.startTrackTimer();
    }

    public void StopBattle()
    {
        //for now just pauses but should play the win audio
        mix1AudioSource.Pause();
    }

    public void StartCountIn()
    {
        TrackTimeManager.beatWait(4);
    }

    public void StartQuickMixBattle()
    {
        //set the current track by dequeing from the queue
        currentTrack = trackQueue.Peek();
        //set the current bpm
        currentBpm = currentTrack.randomTrackData.bpm;
        TrackTimeManager.SetTrackData(currentTrack.randomTrackData);

        //set the audiosclip for mix1 
        mix1AudioSource.clip = currentTrack.randomTrackData.trackClip;

        //set the audioclip for the transition 
        transitionAudioSource.clip = currentTrack.randomTrackData.trackClip;

        //queue up the first track and then the transition audio
        double startTime = AudioSettings.dspTime + (4 * (60d / currentTrack.randomTrackData.bpm));
        mix1AudioSource.PlayScheduled(startTime);
        mix1AudioSource.SetScheduledEndTime(startTime + mix1AudioSource.clip.length);

        //transitionAudioSource.PlayScheduled(AudioSettings.dspTime + (32 * (60d / currentTrack.randomTrackData.bpm)));
        //mix2AudioSource.PlayScheduled(AudioSettings.dspTime + (36 * (60d / currentTrack.randomTrackData.bpm)));


        //mix2AudioSource.PlayScheduled((float)AudioSettings.dspTime + 10 + currentTrack.randomTransitionData.numBeats * (60f / currentTrack.randomTransitionData.bpm));

        //TODO: double check this is working bc this might need a 4+ in front if 
        // TrackTimeManager.setBeatsBeforeNextPhase(4);
        //wait time 


        TrackTimeManager.beatWait(4);
        TrackTimeManager.AddEvent("nextPhase", 4 * (60 / currentTrack.randomTrackData.bpm));

    }

    //so this function is going to need to be called at the end of any phase to queue up the next track and set the phase data

    public void NextBattlePhase()
    {
        if (trackQueue.Count <= 0)
        {
            Debug.Log("END OF TRACK QUEUE");
            Debug.Break();
        }

        Debug.Log("NEXT PHASE");



        //so this needs to also setup dynamic audio file playing on transitions, as well as 
        //managing the audio sources

        string currentPhase = BattleManager.current.battlePhase;

        double nextPhaseTime;

        //TODO: left off here need to reapproach this to fix bugs
        switch (currentPhase)
        {


            //MIX1 MOVING INTO TRANSITION
            case "mix1":
                Debug.Log("NEXT PHASE : MIX1 -> TRANSITION");
                //Debug.Break();

                //transition started, going into mix2 next
                BattleManager.current.SetBattlePhase("transition");

                //so we're in the transition now, need to queue up mix2's audio to play
                nextPhaseTime = currentTrack.randomTransitionData.numBeats * (60 / currentTrack.randomTransitionData.bpm);
                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);

                mix2AudioSource.PlayScheduled((AudioSettings.dspTime) + nextPhaseTime);

                break;

            //MIX2 MOVING INTO TRANSITION 
            case "mix2":
                Debug.Log("NEXT PHASE : MIX1 -> TRANSITION");

                BattleManager.current.SetBattlePhase("transition");

                nextPhaseTime = currentTrack.randomTransitionData.numBeats * (60 / currentTrack.randomTransitionData.bpm);
                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);

                mix1AudioSource.PlayScheduled((AudioSettings.dspTime) + nextPhaseTime);


                break;


            //TRANSITION MOVING INTO MIX1 OR MIX2
            case "transition":



                currentTrack = trackQueue.Dequeue();
                Debug.Log("Current Track Dequeued As : " + currentTrack.name);

                if (trackQueue.Count > 0)
                {
                    nextTrack = trackQueue.Peek();
                }



                //we're assuming at this point that the audio source is stopped
                if (BattleManager.current.lastMix == "mix1")
                {
                    Debug.Log("NEXT PHASE : TRANSITION -> MIX2");
                    //Debug.Break();

                    BattleManager.current.SetBattlePhase("mix2");
                    mix1AudioSource.clip = nextTrack.randomTrackData.trackClip;
                }
                else if (BattleManager.current.lastMix == "mix2")
                {

                    Debug.Log("NEXT PHASE : TRANSITION -> MIX1");
                    //Debug.Break();

                    BattleManager.current.SetBattlePhase("mix1");
                    mix2AudioSource.clip = nextTrack.randomTrackData.trackClip;
                }

                nextPhaseTime = currentTrack.randomTrackData.numBeats * (60 / currentTrack.randomTrackData.bpm);
                double transitionEndTime = nextPhaseTime + (currentTrack.randomTransitionData.numBeats * (60 / currentTrack.randomTransitionData.bpm));
                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);

                //load the audio source for the transition
                transitionAudioSource.clip = currentTrack.randomTransitionData.trackClip;

                //schedule said audio source to play 

                Debug.Log("SCHEDULING TRANSITION AUDIO TO PLAY AT " + nextPhaseTime);
                transitionAudioSource.PlayScheduled(AudioSettings.dspTime + nextPhaseTime);
                //schedule the audio source to stop
                //transitionAudioSource.SetScheduledEndTime(transitionEndTime);
                break;
        }
    }



    public int nextTurnStart;

    //this is for setting the battle track for the longmix mode
    public void setBattleTrack(Track newTrack, bool doWait)
    {
        //couple things need to happen here
        //1. we turn off the current track audio
        mix1AudioSource.Stop();
        //2.we load in the new track as the current track
        currentTrack = newTrack;
        //3.replace the audiosource's clip

        mix1AudioSource.clip = currentTrack.tracks[0].trackClip;
        //4.we need to modify the speed of the indicators (they all look at this variable for their speed)
        currentBpm = newTrack.oldBPM;
        //5.we need to setup the new indicators 
        //BattleManager.current.setupTurnIndicators(newTrack);
        IndicatorManager.current.setupTurnIndicators(newTrack);
        //6.play the new track AFTER 4 BARS OF WAITING if a wait is requested

        nextTurnStart = totalBeats + 5;

        //TrackTimeManager.SetTrackData(currentTrack);
        TrackTimeManager.stopTrackTimer();
        TrackTimeManager.resetTrackTimer();

        // if (!doWait)
        // {
        //     // StartCoroutine(barWait());
        //     TrackTimeManager.current.beatWait(4);
        // }
    }


    public void setupQuickMix()
    {
        //ok so we need some way of indicating in each track which transition and which track are going to be played
        //therefor the tracks are going to need two integer indexes for each which are randomly set here

        for (int i = 0; i < BattleManager.current.numQuickMixTracks; i++)
        {
            Track t = testPlayerTracks[Random.Range(0, testPlayerTracks.Length)];

            t.randomTrackData = t.tracks[Random.Range(0, t.tracks.Length)];
            t.randomTransitionData = t.trackTransitions[Random.Range(0, t.trackTransitions.Length)];
            trackQueue.Enqueue(t);
        }

        IndicatorManager.current.setupQuickMixIndicators(trackQueue);
    }



    //so we need a centralized place for the time passage to be managed from

    public void setPlayerSelectedTrack(Track newTrack)
    {
        playerSelectedTrack = newTrack;
    }

    //this is essentially a start function
    public void playCurrentTrack()
    {
        mix1AudioSource.Play();
        //here we're going to queue up the transition s o theres no possibly delay
    }

    public void ScheduleNextTrack()
    {

    }



    public float beatTransitionCounter = 0;
    public void checkForTransition()
    {
        //TODO: this function needs to check if its time to trigger a transition or trigger another mix 
        //after a transition

        beatTransitionCounter++;

        //check the battles phase 

        if (BattleManager.current.battlePhase == "mix1" || BattleManager.current.battlePhase == "mix2")
        {
            //so if we're in mix1 or mix2 and 4 * the bars per mixphase is less than or equal to the counter we're gonna go to a transition'
            if (beatTransitionCounter >= BattleManager.current.barsPerTurn * 4)
            {
                //reset the transition counter 

                beatTransitionCounter = 0;
                //trigger a transition in the battle manager

            }
        }
        else
        {
            //we're in a transition
            //so do the same check and then look at what the last battle phase is to see if we need to do a transition


        }
    }

    private void Update()
    {
        mix1AudioSource.volume = volume;
        mix2AudioSource.volume = volume;
        transitionAudioSource.volume = volume;
    }
}
