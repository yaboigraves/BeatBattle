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



    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();


        //TODO: rewrite this to use a scriptable object
        //if theres no track manager
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
        audioSource.Play();
    }

    private void Update()
    {
        if (!paused)
        {
            bpmTimer += Time.deltaTime;
            if (bpmTimer > beatDeltaTime)
            {
                bpmTimer = 0;
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
            }
        }
    }
}
