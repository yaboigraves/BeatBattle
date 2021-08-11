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
        Debug.Log(nextBeatTime);

    }


    private void Update()
    {
        if (BattleManager.current.currentState != BattleState.Prebattle)
        {
            CheckForBeat();
        }

    }


    public double lastBeatTime = 0, nextBeatTime = 0;
    //checks to see if we should trigger the beat callback
    void CheckForBeat()
    {
        // Debug.Log("Next Beat Time : " + nextBeatTime);
        // Debug.Log("dsp time" + AudioSettings.dspTime);
        // Debug.Log("");

        if (AudioSettings.dspTime >= nextBeatTime)
        {
            Debug.Log("beat callback");
            TimeManager.BeatCallBack();

            //so remember this is unreliable bud
            //we need to mark the start time and everything is relative to that dont use the current time
            //nextBeatTime = TimeManager.battleStartTime + ((60d / TimeManager.currentSongBpm) * (float)TimeManager.currentBeat);

            //BUG REPORT TODO HIGH PRIO: TODO: TODO: TODO: SO FOR SOME REASON THIS BREAKS ON A RECOMPILE, MAKE SURE THAT CURRENTBEATDSPTIME IS WORKING PROPERLY
            nextBeatTime = TimeManager.currentBeatDSPTime;

            // Debug.Log(nextBeatTime);
            // Debug.Break();

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