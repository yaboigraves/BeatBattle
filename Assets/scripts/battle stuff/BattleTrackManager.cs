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
    public int battleBarsPerTurn = 2;
    int battleTurn;
    public float currentBpm;
    public Track testTrack;

    //if this is enabled play a sound when the metronome ticks
    public bool metronomeAudio;

    int totalBeats;


    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
        AudioClip audioClip;
        if (TrackManager.current == null)
        {
            //initialize the indicator positions 

            testTrack.kickBeats.indicatorPositions = Array.ConvertAll(testTrack.kickBeats.indicatorData.Split(' '), float.Parse);
            testTrack.snareBeats.indicatorPositions = Array.ConvertAll(testTrack.snareBeats.indicatorData.Split(' '), float.Parse);

            currentTrack = testTrack;
            audioClip = testTrack.trackClip;

        }
        else
        {
            //look and grab the current track
            currentTrack = TrackManager.current.currTrack;
            //load the track
            audioClip = currentTrack.trackClip;
        }

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
        //start counting down then do everything
        //StartCoroutine(battleCountIn());

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


    IEnumerator beatTick()
    {

        //if we're paused dont do any waiting
        while (paused)
        {
            yield return null;
        }

        //if we're not paused wait for 1 beat
        //we can calculate this using the bpm and just dividing it by 60



        //update the beat
        beat++;

        if (beat > 3)
        {
            beat = 0;
            battleTurn++;
            if (battleTurn >= battleBarsPerTurn)
            {
                // print("changiung turn");
                BattleManager.current.changeTurn();
                battleTurn = 0;
            }
        }

        BattleUIManager.current.UpdateMetronome(beat, false);

        //call ourself again

        totalBeats++;

        yield return new WaitForSeconds(beatDeltaTime);
        StartCoroutine(beatTick());
    }


}
