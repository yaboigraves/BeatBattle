using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightweightTrackTimeManager : MonoBehaviour
{
    //smaller version of other track time manager, all this one does is count the time of the current track 

    public static LightweightTrackTimeManager current;
    public bool isCounting;

    public float songPositionInBeats, dspSongTime, songBpm, secPerBeat, songPosition, songStartTime;
    public AudioSource audioSource;

    [Header("DEBUG DSP TIME")]
    public float debugDspSongTime;

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
        songBpm = track.oldBPM;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
    }

    public void StartCount()
    {
        songStartTime = (float)AudioSettings.dspTime;
        isCounting = true;
    }

    public void StopCount()
    {
        isCounting = false;
    }


    // Update is called once per frame
    void Update()
    {
        debugDspSongTime = (float)AudioSettings.dspTime;

        if (isCounting)
        {
            songPosition = (float)(AudioSettings.dspTime - (songStartTime));
            songPositionInBeats = (songPosition / secPerBeat);

            CircularBattleManager.current.updateBPMTime((Mathf.FloorToInt(songPositionInBeats) % 4) + 1);
        }
    }
}
