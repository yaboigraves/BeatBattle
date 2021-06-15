using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBattleAudioManager : MonoBehaviour
{
    public static NBattleAudioManager current;

    AudioSource musicAudioSource;

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
        TimeManager.SetBattleStart();
        musicAudioSource.Play();
    }



}


[System.Serializable]
public class SongInfo
{
    public AudioClip clip;
    public float bpm;
    public int lengthInBeats;
}