using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBattleAudioManager : MonoBehaviour
{
    public static NBattleAudioManager current;

    AudioSource musicAudioSource;

    //so the battle audio manager is going to every beat fire a callback to the time manager to handle on beat stuff





    public SongInfo songInfo;

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

        musicAudioSource.clip = songInfo.clip;
        TimeManager.SetCurrentSongInfo(songInfo.bpm);
    }



    public void StartSong()
    {
        //this may need to be halted until the audio source can reliably be known to be playing

        double battleStartTime = AudioSettings.dspTime + 0.5f;
        TimeManager.SetBattleStart(battleStartTime);
        musicAudioSource.PlayScheduled(battleStartTime);
    }


    private void Update()
    {
        if (NBattleManager.current.currentState != BattleState.Prebattle)
        {
            CheckForBeat();
        }

    }


    public double lastBeatTime = 0, nextBeatTime = 0;
    //checks to see if we should trigger the beat callback
    void CheckForBeat()
    {
        if (AudioSettings.dspTime > nextBeatTime)
        {
            TimeManager.BeatCallBack();

            //so remember this is unreliable bud
            //we need to mark the start time and everything is relative to that dont use the current time
            nextBeatTime = TimeManager.battleStartTime + ((60d / TimeManager.currentSongBpm) * (float)TimeManager.currentBeat);
        }
    }



}


[System.Serializable]
public class SongInfo
{
    public AudioClip clip;
    public float bpm;
    public int lengthInBeats;
}