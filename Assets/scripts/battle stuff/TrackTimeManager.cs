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

    public float currentPlayerBars;

    public bool trackStarted = false;

    public bool countingIn = false;

    public float turnBeatCounter;


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

    public float currentTurnStartBeat = 0;

    //TODO: implement a way to modify the starting audio time (because its starting time since the )
    void Update()
    {



        debugDSPTIME = (float)AudioSettings.dspTime;

        if (trackStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - startUpTime - (dspSongTime));
            songPositionInBeats = (songPosition / secPerBeat);
        }


        //update ui with data 

        if (songPositionInBeats >= 16)
        {
            //turn change 
            currentTurnStartBeat = songPositionInBeats;
            BattleManager.current.changeTurn();
        }





        if (trackStarted)
        {
            BattleUIManager.current.UpdateMetronome(((Mathf.FloorToInt(songPositionInBeats)) % 4), false);
        }

        if (doingWait)
        {

            MoveIndicatorContainerForWait();
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

        //so we want to have 4 beats of time progress
        //as this time progresses need to lerp the indicator container down 4 units 
        //so lerp(start,start -4, currentBeat/4)

        waitTimeStart = (float)AudioSettings.dspTime;
        waitTimeOver = (float)AudioSettings.dspTime + 4 * secPerBeat;


        StartCoroutine(beatWaitRoutine(numBeats));
        // audioSource.Play();
        // trackStarted = true;
    }



    //starts shit but waits 4 beats before resetting all the data back to 0

    float waitTimeOver;
    bool doingWait = false;
    float waitTimeStart;
    public IEnumerator beatWaitRoutine(int numBeats)
    {

        //so this needs to figure out basically jsut how much time needs to pass from now till 4 beats from now 
        //first lets figure out when NOW IS 

        float songCurrentBeatPosition = songPositionInBeats;
        doingWait = true;


        yield return new WaitUntil(() => AudioSettings.dspTime > waitTimeOver);

        doingWait = false;
        //yield return null;

        //startup time is the difference in dsptime of the song + the amount of time it currently is 
        startUpTime = (float)AudioSettings.dspTime - dspSongTime;
        songPositionInBeats = 0;
        audioSource.Play();
        trackStarted = true;
    }

    public GameObject currIndicatorContainer;
    Vector3 indicatorStartPos;
    public void setCurrIndicatorContainer(GameObject indiContainer)
    {
        currIndicatorContainer = indiContainer;
    }

    public void MoveIndicatorContainerForWait()
    {
        float movePercent = (float)((AudioSettings.dspTime - waitTimeStart) / (waitTimeOver - waitTimeStart));
        //lerp the indicator container between its spawn position and 0 based on where audio time is between the waittimeover variable
        //currIndicatorContainer.transform.position = Vector3.Lerp(new Vector3(0, 4, 0), new Vector3(0, 0, 0), movePercent);
    }


    //notes 
    //we can now just use the songPositionInBeats variable to set the position of indicatoes = to that rather than translating them 
    //this will yield perfect syncing of the track time and the indicators
}
