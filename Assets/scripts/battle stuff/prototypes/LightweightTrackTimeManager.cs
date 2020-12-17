using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightweightTrackTimeManager : MonoBehaviour
{
    //smaller version of other track time manager, all this one does is count the time of the current track 

    public static LightweightTrackTimeManager current;
    public bool isCounting;

    public float songPositionInBeats, dspSongTime, songBpm, secPerBeat, songPosition;
    public AudioSource audioSource;


    private void Awake()
    {
        current = this;
    }
    void Start()
    {

    }

    public void SetSongData(Track track)
    {
        audioSource.clip = track.trackClip;
        songBpm = track.bpm;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (isCounting)
        {
            songPosition = (float)(AudioSettings.dspTime - (dspSongTime));
            songPositionInBeats = (songPosition / secPerBeat);
        }
    }
}
