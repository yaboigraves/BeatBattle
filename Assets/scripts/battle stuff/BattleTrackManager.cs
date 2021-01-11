using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleTrackManager : MonoBehaviour
{
    AudioSource audioSource;
    //this guy takes in a track and then manages all the playing of that track IN a battle
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


    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
        AudioClip audioClip;
        if (TrackManager.current == null)
        {
            //initialize the indicator positions 

            //TRACK REWRITE COMMENTOUT
            //testPlayerTracks[0].kickBeats.indicatorPositions = Array.ConvertAll(testPlayerTracks[0].kickBeats.indicatorData.Split(' '), float.Parse);
            //testPlayerTracks[0].snareBeats.indicatorPositions = Array.ConvertAll(testPlayerTracks[0].snareBeats.indicatorData.Split(' '), float.Parse);

            //default the selected track to just the first track
            playerSelectedTrack = testPlayerTracks[0];
            currentTrack = playerSelectedTrack;
            audioClip = testPlayerTracks[0].trackClip;
        }
        else
        {
            playerTracks = GameManager.current.player.GetComponent<PlayerInventory>().battleEquippedTracks;
            currentTrack = playerTracks[0];
            playerSelectedTrack = playerTracks[0];
            audioClip = currentTrack.trackClip;
        }

        //print("audiosource" + audioSource.name);

        currentBpm = currentTrack.bpm;
        audioSource.clip = audioClip;

        //commented out to test timescale shit 
        //beatDeltaTime = (1 / currentBpm) * 60;

        //uniform speed for timescale change
        beatDeltaTime = 1;
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
        TrackTimeManager.current.startTrackTimer();
    }

    public void StopBattle()
    {
        //for now just pauses but should play the win audio
        audioSource.Pause();
    }

    public void StartCountIn()
    {
        TrackTimeManager.current.beatWait(4);
        //need to go to the tracktime manager and have it wait 4 beats, then reset all the timing data and actually start the song
    }


    public int nextTurnStart;
    public void setBattleTrack(Track newTrack, bool doWait)
    {
        //couple things need to happen here
        //1. we turn off the current track audio
        audioSource.Stop();
        //2.we load in the new track as the current track
        currentTrack = newTrack;
        //3.replace the audiosource's clip

        audioSource.clip = currentTrack.trackClip;
        //4.we need to modify the speed of the indicators (they all look at this variable for their speed)
        currentBpm = newTrack.bpm;
        //5.we need to setup the new indicators 
        //BattleManager.current.setupTurnIndicators(newTrack);
        IndicatorManager.current.setupTurnIndicators(newTrack);
        //6.play the new track AFTER 4 BARS OF WAITING if a wait is requested

        nextTurnStart = totalBeats + 5;

        TrackTimeManager.current.SetSongData(currentTrack);
        TrackTimeManager.current.stopTrackTimer();
        TrackTimeManager.current.resetTrackTimer();

        if (!doWait)
        {
            // StartCoroutine(barWait());
            TrackTimeManager.current.beatWait(4);
        }
    }

    //so we need a centralized place for the time passage to be managed from

    public void setPlayerSelectedTrack(Track newTrack)
    {
        playerSelectedTrack = newTrack;
    }
}
