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
    public string testingJsons;

    public float currentBpm;
    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();


        //TODO: rewrite this to use a scriptable object
        //if theres no track manager
        AudioClip audioClip;
        if (TrackManager.current == null)
        {
            //if theres no manager then we source the track from a set of possible presets 
            //Track testTrack = 
            //currentTrack = testTrack;
            audioClip = (AudioClip)Resources.Load("audio/battleTracks/" + currentTrack.trackName);

        }
        else
        {
            //look and grab the current track
            currentTrack = TrackManager.current.currTrack;
            //load the track
            audioClip = (AudioClip)Resources.Load("audio/battleTracks/" + currentTrack.trackName);

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
