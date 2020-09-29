using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTimeManager : MonoBehaviour
{
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


    public bool trackStarted;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (trackStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            songPositionInBeats = songPosition / secPerBeat;
        }
    }


    //notes 
    //we can now just use the songPositionInBeats variable to set the position of indicatoes = to that rather than translating them 
    //this will yield perfect syncing of the track time and the indicators
}
