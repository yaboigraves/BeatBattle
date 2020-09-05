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


    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
        AudioClip audioClip;
        if (TrackManager.current == null)
        {
            //initialize the indicator positions 

            testPlayerTracks[0].kickBeats.indicatorPositions = Array.ConvertAll(testPlayerTracks[0].kickBeats.indicatorData.Split(' '), float.Parse);
            testPlayerTracks[0].snareBeats.indicatorPositions = Array.ConvertAll(testPlayerTracks[0].snareBeats.indicatorData.Split(' '), float.Parse);

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

            //TODO: depending on if the player or the enemy goes first we grab different tracks
            //
        }

        print("audiosource" + audioSource.name);

        currentBpm = currentTrack.bpm;
        audioSource.clip = audioClip;
        beatDeltaTime = (1 / currentBpm) * 60;
    }


    private void Start()
    {
        paused = true;
    }

    public void StartBattle()
    {
        paused = false;

        audioSource.Play();
        BattleManager.current.battleStarted = true;
        StartCoroutine(beatTick());
    }

    public void StopBattle()
    {
        //for now just pauses but should play the win audio
        audioSource.Pause();
    }

    public void StartCountIn()
    {
        StartCoroutine(battleCountIn());
    }

    IEnumerator battleCountIn()
    {
        // shitty 1 2 3 4
        BattleUIManager.current.UpdateMetronome(beat, true);
        yield return new WaitForSeconds(beatDeltaTime);
        beat++;
        BattleUIManager.current.UpdateMetronome(beat, true);
        yield return new WaitForSeconds(beatDeltaTime);
        beat++;
        BattleUIManager.current.UpdateMetronome(beat, true);
        yield return new WaitForSeconds(beatDeltaTime);
        beat++;
        BattleUIManager.current.UpdateMetronome(beat, true);
        yield return new WaitForSeconds(beatDeltaTime);
        StartBattle();
    }

    public int nextTurnStart;
    public void switchBattleTrack(Track newTrack, bool doWait)
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
        BattleManager.current.setupTurnIndicators(newTrack);
        //6.play the new track AFTER 4 BARS OF WAITING if a wait is requested

        nextTurnStart = totalBeats + 5;

        if (!doWait)
        {
            StartCoroutine(barWait());
        }
    }

    IEnumerator barWait()
    {
        yield return new WaitUntil(() => totalBeats == nextTurnStart);
        print("bar waiting playing");
        audioSource.Play();
    }


    //so we need a centralized place for the time passage to be managed from
    IEnumerator beatTick()
    {
        //if we're paused dont do any waiting
        while (paused)
        {
            yield return null;
        }

        //update the beat
        beat++;

        //tell the battle manager to update the beat info (for now it updates every beat)
        BattleManager.current.UpdateGearPipeline();

        if (beat > 3)
        {
            beat = 0;
            battleTurn++;

            if (battleTurn >= battleBarsPerTurn)
            {
                BattleManager.current.changeTurn();
                battleTurn = 0;


            }
        }

        BattleUIManager.current.UpdateMetronome(beat, false);

        totalBeats++;
        currentBar = (int)Mathf.Floor(totalBeats / 4.0f);

        yield return new WaitForSeconds(beatDeltaTime);
        StartCoroutine(beatTick());
    }

    public void setPlayerSelectedTrack(Track newTrack)
    {
        playerSelectedTrack = newTrack;
    }
}
