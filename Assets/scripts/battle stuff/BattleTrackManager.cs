using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        current = this;
        //look and grab the current track
        currentTrack = TrackManager.current.currTrack;
        //load the track
        audioSource = GetComponent<AudioSource>();
        AudioClip audioClip = (AudioClip)Resources.Load("audio/battleTracks/" + currentTrack.trackName);
        audioSource.clip = audioClip;

        //loadup all the bpm variables n shit
        beatDeltaTime = (1 / currentTrack.bpm) * 60;

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
