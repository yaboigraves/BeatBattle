using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTimeManager : MonoBehaviour
{

    public static TrackTimeManager current;
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource audioSource;

    public float debugDSPTIME;


    public bool trackStarted = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // secPerBeat = 60f / songBpm;
    }

    public void SetSongData(Track track)
    {
        audioSource.clip = track.trackClip;
        songBpm = track.bpm;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
    }

    // Update is called once per frame

    public float startUpTime;

    //TODO: implement a way to modify the starting audio time (because its starting time since the )
    void Update()
    {

        debugDSPTIME = (float)AudioSettings.dspTime;

        songPosition = (float)(AudioSettings.dspTime - startUpTime - (dspSongTime));
        songPositionInBeats = (songPosition / secPerBeat);

        //update ui with data 


        if (trackStarted)
        {
            BattleUIManager.current.UpdateMetronome(((Mathf.FloorToInt(songPositionInBeats)) % 4), false);
        }
    }


    public float dspTimeDifferenceFromStart;
    public void startTrackTimer()
    {
        trackStarted = true;

        //TODO: so also need to take note of the current difference in dsp time
    }

    public void stopTrackTimer()
    {
        trackStarted = false;
    }

    public void resetTrackTimer()
    {
        songPosition = 0;
        songPositionInBeats = 0;
    }


    public void beatWait(int numBeats)
    {
        //dspTimeDifferenceFromStart = (float)AudioSettings.dspTime - dspSongTime;


        StartCoroutine(beatWaitRoutine(numBeats));
        // audioSource.Play();
        // trackStarted = true;
    }



    //starts shit but waits 4 beats before resetting all the data back to 0
    public IEnumerator beatWaitRoutine(int numBeats)
    {
        float songCurrentBeatPosition = songPositionInBeats;
        //yield return new WaitUntil(() => songPositionInBeats > songCurrentBeatPosition + numBeats);
        yield return null;

        //startup time is the difference in dsptime of the song + the amount of time it currently is 
        startUpTime = (float)AudioSettings.dspTime - dspSongTime;
        //songPositionInBeats = 0;
        audioSource.Play();
        trackStarted = true;
    }


    //notes 
    //we can now just use the songPositionInBeats variable to set the position of indicatoes = to that rather than translating them 
    //this will yield perfect syncing of the track time and the indicators
}
