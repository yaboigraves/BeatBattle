using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBattleManager : MonoBehaviour
{

    //so the basic thing this is gonna need is gonna be a track to source the info from
    //a way to drop down all the track info onto the grid
    //a way to move the indicators inwards and then delete once reaching the center
    //a way to make bars that indicate bpm


    public Track testTrack;
    public GameObject indicator, circleBar;

    public static CircularBattleManager current;

    public Transform indicatorContainer;

    public bool battleStarted;

    AudioSource audio;
    private void Awake()
    {
        current = this;
    }

    public void StartBattle()
    {
        if (!battleStarted)
        {
            battleStarted = true;
            LightweightTrackTimeManager.current.isCounting = true;
            //play the song 
            audio.Play();
        }

    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = testTrack.trackClip;

        //set up all the indicators at their various lanes depending on the track info for indicator positions
        LightweightTrackTimeManager.current.SetSongData(testTrack);

        testTrack.kickBeats.initData();
        testTrack.snareBeats.initData();
        //kick is left
        //snare is right lane
        //kick indicators



        for (int i = 0; i < testTrack.kickBeats.indicatorPositions.Length; i++)
        {
            Vector3 kickPos = new Vector3(-testTrack.kickBeats.indicatorPositions[i], 0, 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, indicatorContainer);

            indic.GetComponent<Indicator>().end = new Vector3(-1, 0, 0);
            //check who's turn it is 
        }
        for (int i = 0; i < testTrack.snareBeats.indicatorPositions.Length; i++)
        {
            Vector3 snarePos = new Vector3(testTrack.snareBeats.indicatorPositions[i], 0, 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            GameObject indic = Instantiate(indicator, snarePos, Quaternion.identity, indicatorContainer);
            indic.GetComponent<Indicator>().end = new Vector3(1, 0, 0);
        }

        //bars

        for (int i = 1; i <= 16; i++)
        {
            GameObject b = Instantiate(circleBar, Vector3.zero, Quaternion.identity);
            b.GetComponent<CircularBar>().start = new Vector3(i, i, i);
            b.transform.localScale = new Vector3(i, i, i);
        }
    }
    void SetupIndicators()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle();
        }
    }
}
