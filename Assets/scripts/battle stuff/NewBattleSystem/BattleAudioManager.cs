using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    public static BattleAudioManager current;

    //ok, so this def needs to be made into some kind of 'track' object that contains bpm info and the audio clip


    AudioSource musicAudioSource;

    public Track audioTrack;

    //so the battle audio manager is going to every beat fire a callback to the time manager to handle on beat stuff

    // public SongInfo songInfo;

    private void Awake()
    {
        current = this;
        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitializeSongInfo();
    }

    public void InitializeSongInfo()
    {
        //set the bpm
        //set the audio source


        musicAudioSource.clip = audioTrack.trackClip;
        TimeManager.SetCurrentSongInfo(audioTrack.oldBPM);
    }



    public void StartSong()
    {
        //this may need to be halted until the audio source can reliably be known to be playing
        //print("test");
        double battleStartTime = AudioSettings.dspTime + 0.5f;
        TimeManager.SetBattleStart(battleStartTime);
        musicAudioSource.PlayScheduled(battleStartTime);
        nextBeatTime = TimeManager.battleStartTime + TimeManager.timePerBeat;
        //Debug.Log(nextBeatTime);



    }


    private void Update()
    {
        if (BattleManager.current.battle.currentState != BattleState.Prebattle)
        {
            CheckForBeat();
        }

    }


    public double lastBeatTime = 0, nextBeatTime = 0;
    //checks to see if we should trigger the beat callback

    //maybe move this somewhere else, probably time manager really doesnt need to be in here
    void CheckForBeat()
    {

        if (AudioSettings.dspTime >= nextBeatTime)
        {
            //print("nce");
            TimeManager.BeatCallBack();
            nextBeatTime = TimeManager.currentBeatDSPTime;

            //if we're at the end of a song, queue up the next one
            //going to need to track the length of the current track



        }
    }



}


// [System.Serializable]
// public class SongInfo
// {
//     public AudioClip clip;
//     public float bpm;
//     public int lengthInBeats;
// }