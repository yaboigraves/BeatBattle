using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class CircularBattleManager : MonoBehaviour
{
    //so the basic thing this is gonna need is gonna be a track to source the info from
    //a way to drop down all the track info onto the grid
    //a way to move the indicators inwards and then delete once reaching the center
    //a way to make bars that indicate bpm
    public Track testTrack;
    public GameObject indicator, circleBar;
    public static CircularBattleManager current;
    public Transform rightIndicatorLane, leftIndicatorLane,barContainer;
    public bool battleStarted;
    public TextMeshProUGUI bpmText;
    AudioSource audio;

    public Track[] testTracks;
    private void Awake()
    {
        if(current != null){
            Destroy(this.gameObject);
        }
        
        current = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartBattle()
    {
        if (!battleStarted)
        {
            battleStarted = true;

            //play the song 
            audio.Play();

            print("starting counter?");
            

            LightweightTrackTimeManager.current.StartCount();
            //set the timing to actually start now
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

        //creates the bars

        for (int i = 1; i <= 64; i++)
        {
            GameObject b = Instantiate(circleBar, Vector3.zero, Quaternion.identity,barContainer);
            b.GetComponent<CircularBar>().start = new Vector3(i, i, i);
            b.transform.localScale = new Vector3(i, i, i);
        }
        SetupIndicators();
    }

    void SetupIndicators()
    {
        //do this multiple times
        for(int j = 0;j <= 2; j++){
            for (int i = 0; i < testTrack.kickBeats.indicatorPositions.Length; i++)
            {
                Vector3 kickPos = new Vector3((-testTrack.kickBeats.indicatorPositions[i] - (j * 15)), 0, 0);

                //each unit is 1 bar 
                //therefore we need to start the next batck of indicators at wherever the loop ends
                //probablyh easiest for now just to bake the length of the loop into the track object 
                GameObject indic = Instantiate(indicator, kickPos + leftIndicatorLane.transform.position, Quaternion.identity, leftIndicatorLane);

                indic.GetComponent<Indicator>().SetIndicInfo(new Vector3(-1, 0, 0), testTrack.kickBeats.indicatorPositions[i] + (16 * j));
                indic.GetComponent<Indicator>().SetIndicatorType(true, "Heady");
                //check who's turn it is 
            }
            for (int i = 0; i < testTrack.snareBeats.indicatorPositions.Length; i++)
            {
                Vector3 snarePos = new Vector3(testTrack.snareBeats.indicatorPositions[i] + (j * 15), 0, 0);
                //each unit is 1 bar 
                //therefore we need to start the next batck of indicators at wherever the loop ends
                //probablyh easiest for now just to bake the length of the loop into the track object 
                GameObject indic = Instantiate(indicator, snarePos + rightIndicatorLane.transform.position, Quaternion.identity, rightIndicatorLane);
                indic.GetComponent<Indicator>().SetIndicInfo(new Vector3(1, 0, 0), testTrack.snareBeats.indicatorPositions[i] +(16 * j) );
            }
        }
        

        //bars
        // for (int i = 1; i <= 64; i++)
        // {
        //     GameObject b = Instantiate(circleBar, Vector3.zero, Quaternion.identity,barContainer);
        //     b.GetComponent<CircularBar>().start = new Vector3(i, i, i);
        //     b.transform.localScale = new Vector3(i, i, i);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle();
        }
    }

    public void updateBPMTime(int beat)
    {
        bpmText.text = beat.ToString();
    }

    public void ClearIndicators(){
        for(int i = 0; i < leftIndicatorLane.transform.childCount; i++){
            Destroy(leftIndicatorLane.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < rightIndicatorLane.transform.childCount; i++){
            Destroy(rightIndicatorLane.transform.GetChild(i).gameObject);
        }
    }

    public void SetNewTrack(Track newTrack){

        LightweightTrackTimeManager.current.StopCount();
        battleStarted = false;
        //stop the current audio 
        audio.Stop();

        //clear the old indicators
        ClearIndicators();


        testTrack = newTrack;
        
        audio.clip = testTrack.trackClip;

        //set up all the indicators at their various lanes depending on the track info for indicator positions
        LightweightTrackTimeManager.current.SetSongData(testTrack);

        testTrack.kickBeats.initData();
        testTrack.snareBeats.initData();
        //kick is left
        //snare is right lane
        //kick indicators
        SetupIndicators();
    }

    public void UpdateTrackTest(int val){
        //reload the scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
        SetNewTrack(testTracks[val]);
        
    }

    public void ResetBattle(){
        //stop the audio 
        
        SetNewTrack(testTrack);
    }
}
