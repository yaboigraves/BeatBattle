using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleTrackManager : MonoBehaviour
{

    //gonna do some rewriting here 
    /*
        so we're going to need to track phases now, 
        -mix1
        -mix2
        -transitions


    */



    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

    public AudioSource mix1AudioSource, mix2AudioSource, transitionAudioSource;

    public Track currentTrack;
    public static BattleTrackManager current;
    public float bpmTimer, beatDeltaTime;
    public int beat;
    public bool paused;
    public int battleBarsPerTurn = 4;
    public int battleTurn;
    public float currentBpm;
    public Track[] playerTracks;
    public Track[] testPlayerTracks;
    public Track[] testEnemyTracks;
    //if this is enabled play a sound when the metronome ticks
    public bool metronomeAudio;
    public int totalBeats;
    public int currentBar;
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
        currentTrack = trackQueue.Dequeue();
        //set the current bpm
        currentBpm = currentTrack.tracks[0].bpm;

        TrackTimeManager.SetSongData(currentTrack);

        //set the audiosclip for mix1 
        mix1AudioSource.clip = currentTrack.tracks[0].trackClip;

        //set the audioclip for the transition 
        transitionAudioSource.clip = currentTrack.trackTransitions[0].trackClip;

        //set the audioclip for the next song by peeking the queue 
        mix2AudioSource.clip = trackQueue.Peek().tracks[0].trackClip;

        TrackTimeManager.setBeatsBeforeNextPhase(currentTrack.tracks[0].numBeats);

        //wait time 
        TrackTimeManager.beatWait(4);

    }

    public void NextBattlePhase()
    {
        Debug.Log("NEXT PHASE");

        //note : so instead of actually playing the audio source this assumes that the audio source for the next phase 
        //has alrerady been queued up


        //check what the current phase is 

        string currentPhase = BattleManager.current.battlePhase;

        switch (currentPhase)
        {
            case "mix1":
                //turn off the current audiop
                mix1AudioSource.volume = 0.1f;

                BattleManager.current.SetBattlePhase("transition");
                TrackTimeManager.setBeatsBeforeNextPhase(4);

                //queue up the mix2 track
                //needs to calculate the dsp time of the next play
                //time + beats * beats/sec * 1000
                mix2AudioSource.PlayScheduled(TrackTimeManager.GetDSPTimeForNextPlay(4));

                break;
            case "mix2":
                //turn on the transition audiosource turn off our audio source
                //transitionAudioSource.Play();
                mix2AudioSource.Stop();
                BattleManager.current.SetBattlePhase("transition");
                TrackTimeManager.setBeatsBeforeNextPhase(4);
                break;
            case "transition":
                //so this is where we need to have known the last active mix
                transitionAudioSource.Stop();
                if (BattleManager.current.lastMix == "mix1")
                {
                    BattleManager.current.SetBattlePhase("mix2");
                }
                else if (BattleManager.current.lastMix == "mix2")
                {
                    BattleManager.current.SetBattlePhase("mix1");
                }

                //queue up the next transition
                transitionAudioSource.PlayScheduled(TrackTimeManager.GetDSPTimeForNextPlay(32));

                TrackTimeManager.setBeatsBeforeNextPhase(28);
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

        TrackTimeManager.SetSongData(currentTrack);
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
        //TODO: create 2 queues of quickmix tracks
        //TODO: init all the indicators for quickmix tracks

        //for now just gonna setup up everything manually not necessarily dynamic at all
        trackQueue.Enqueue(testPlayerTracks[0]);
        trackQueue.Enqueue(testPlayerTracks[0]);
        IndicatorManager.current.setupQuickMixIndicators(trackQueue);

        //pass the queue to the indicator manager so it can setup the indicators
        //for now we're assuming we're always going to play the first transitions
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
