using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this singletons job is to keep playing music in the overworld and then transition control to the battle audio controller when a fight happens
//its the big database of songs to pick from for battles, and for overworld background stuff
//experimenting with having this also manage battle audio, probably smart

public class TrackManager : MonoBehaviour
{
    public static TrackManager current;
    public Track[] backgroundAudioTracks;

    //TODO: implement this as maybe part of the player inventory
    public Track[] battleAudioTracks;

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

        //UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
        playRandomBackgroundTrack();
    }

    public IEnumerator playSong()
    {
        currAudio.Play();

        yield return new WaitForSeconds(currAudio.clip.length);

        //for now it just picks a random track afterwords
        playRandomBackgroundTrack();
    }

    public void UpdateCurrentTrack(Track newTrack)
    {
        if (currAudio.isPlaying)
        {
            StopCurrentTrack();
        }

        currAudio.clip = newTrack.trackClip;

        currTrack = newTrack;

        currentBpm = newTrack.bpm;

        UIManager.current.updateCurrentTrack(newTrack);

        beatDeltaTime = (1 / currentBpm) * 60;
        bpmTimer = 0;
        beat = 0;

        songRoutine = StartCoroutine(playSong());
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

    public void UpdateTrackVolume(float newVolume)
    {
        currAudio.volume = newVolume;

        SaveManager.UpdateVolume(newVolume);
    }
}



