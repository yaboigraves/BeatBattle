using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this singletons job is to keep playing music in the overworld and then transition control to the battle audio controller when a fight happens
//its the big database of songs to pick from for battles, and for overworld background stuff
//experimenting with having this also manage battle audio, probably smart

public class TrackManager : MonoBehaviour
{

    public static TrackManager current;
    public string[] backgroundAudioTrackJsons;
    public Track[] backgroundAudioTracks;
    public Track[] battleAudioTracks;
    public AudioClip[] backgroundAudioClips;

    //TODO: maybe try a dictionary of Track objects to AudioClips for better organization 

    Dictionary<Track, AudioClip> trackClipDictionary;

    public AudioSource currAudio;
    public Track currTrack;
    public float currentBpm;
    public int currentTrack = 0;
    public bool paused;
    public float bpmTimer, beatDeltaTime;
    public int beat;
    public bool inBattle;
    Coroutine songRoutine;

    void Awake()
    {
        if (current == null)
        {
            current = this;
            currAudio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitTracks();

        Track testTrack = TrackJsonParser.parseJSON(backgroundAudioTrackJsons[0]);
        currTrack = testTrack;

        UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
    }


    void InitTracks()
    {
        backgroundAudioTracks = new Track[backgroundAudioTrackJsons.Length];
        backgroundAudioClips = new AudioClip[backgroundAudioTracks.Length];

        trackClipDictionary = new Dictionary<Track, AudioClip>();


        //reads all the track jsons for the current level (does every single one right now)

        for (int i = 0; i < backgroundAudioTrackJsons.Length; i++)
        {
            backgroundAudioTracks[i] = TrackJsonParser.parseJSON(backgroundAudioTrackJsons[i]);
        }

        //loads all the background audio clips (the actual audio) into an array from the resources folder
        for (int i = 0; i < backgroundAudioClips.Length; i++)
        {
            backgroundAudioClips[i] = (AudioClip)Resources.Load("audio/backgroundTracks/" + backgroundAudioTracks[i].trackName);
            //add to the dictionary 
            trackClipDictionary.Add(backgroundAudioTracks[i], backgroundAudioClips[i]);

        }
    }


    public IEnumerator playSong()
    {
        currAudio.Play();

        //print("PLAYING SONG AND WAITING FOR " + currAudio.clip.length);


        yield return new WaitForSeconds(currAudio.clip.length);

        //for now it just picks a random track afterwords
        playRandomBackgroundTrack();
    }

    public void UpdateCurrentTrack(Track newTrack)
    {

        if (newTrack.isBattleTrack)
        {
            AudioClip audioClip = (AudioClip)Resources.Load("audio/battleTracks/" + newTrack.trackName);
            currAudio.clip = audioClip;

        }
        else
        {
            currAudio.clip = trackClipDictionary[newTrack];
            songRoutine = StartCoroutine(playSong());
        }

        currTrack = newTrack;

        currentBpm = newTrack.bpm;

        UIManager.current.updateCurrentTrack(newTrack);

        beatDeltaTime = (1 / currentBpm) * 60;
        bpmTimer = 0;
        beat = 0;
    }

    //so the problem is there's literally nothing in the background audio tracks
    //going to need to setup this array based on all the shit in the jsons array
    public void playRandomBackgroundTrack()
    {

        currentTrack = Random.Range(0, backgroundAudioTracks.Length);
        UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
    }

    public void PauseTrack()
    {
        currAudio.Pause();
        paused = true;
    }

    public void UnPauseTrack()
    {
        currAudio.UnPause();
        paused = false;
    }

    public void StopCurrentTrack()
    {
        PauseTrack();
        StopCoroutine(songRoutine);
    }
}



