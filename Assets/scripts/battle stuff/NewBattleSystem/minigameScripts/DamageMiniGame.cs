using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
9/10 minigame notes
-so we probably want a root little object that chills in the scene and just handles like ui stuff
-all settings info like the beats and bpm should be passed in via a scriptable object


*/

public class DamageMiniGame : MiniGame
{
    public GameObject indicator;
    public Transform indicatorContainer;
    float beatOffset;
    RectTransform rectTransform;

    public List<NIndicator> indicators;

    public float hitToleranceTime = 0.2f;
    float bpm;
    public BeatPulse pulsePad;
    private void Start()
    {
        base.LoadStuff();
        //load out some indicators
        //the distance between them can be statically set for now
        //actually we should have a ui panel for it that we use the size of it and divide it by like 8 to get that much space

        //figure out what our settings are, i guess we can just take default ones
        rectTransform = indicatorContainer.GetComponent<RectTransform>();

        beatOffset = rectTransform.rect.height / this.miniGameSettings.numBeats;
        // SpawnIndicators();
    }


    void SpawnIndicators(float bpm)
    {

        this.bpm = bpm;
        //just spawn one every 2 beats for now

        // Debug.Log("spawning indicators");
        // Debug.Log(beatTimes.Count);


        for (int i = 0; i < beatTimes.Count; i++)
        {

            //instantiate an indicator for each position
            GameObject ind = Instantiate(indicator, indicatorContainer.transform.position - (Vector3.up * (rectTransform.rect.height / 2)), Quaternion.identity, indicatorContainer);
            //move the indicator up 
            ind.transform.Translate(Vector3.up * (beatOffset * (float)beatTimes[i]));


            NIndicator indie = ind.GetComponent<NIndicator>();
            //Debug.Log(Vector3.up * (beatOffset * (float)beatTimes[i]));

            // Debug.Log(beatTimes[i]);
            // Debug.Log(beatOffset);

            //TODO: copy this code over to the healing minigame
            indie.SetIndicatorInfo(Vector3.up * (beatOffset * (float)beatTimes[i]), indicatorContainer.transform.position - (Vector3.up * (rectTransform.rect.height / 2)), (int)beatTimes[i]);
            indicators.Add(indie);

            //Debug.Log(indicators.Count);
        }

        //Debug.Break();
    }

    public override void Preload(Sample sample)
    {
        base.Preload(sample);
        SpawnIndicators(sample.sampleTrack.oldBPM);

        //we also plug in any other info like the bpm and the actual settings

        miniGameSettings.minigameSample = sample;
        miniGameSettings.numBeats = sample.sampleTrack.randomTrackData.kickBeats.Count;
        //so we also generate the report here
        report = new MinigameReport(sample.sampleTrack.randomTrackData.kickBeats.Count);
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        //so we need some way of loading the minigame the turn before they happen
        SetIndicatorTimes();
    }

    //so this probably needs to be made more accurate
    void SetIndicatorTimes()
    {

        //so the start time needs to be the battle start time + the first beat of the new minigames time

        //Debug.Log(TimeManager.currentMinigameBeatStart);
        // Debug.Log(TimeManager.beatTimeline.timeline[TimeManager.currentMinigameBeatStart].time);
        //Debug.Break();



        foreach (NIndicator n in indicators)
        {
            //Debug.Log("yeet");
            n.SetStartTime(TimeManager.battleStartTime + TimeManager.beatTimeline.timeline[TimeManager.currentMinigameBeatStart].time);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && state == MiniGameState.Inactive)
        {

            state = MiniGameState.Active;
            //tell all the indicators that now is the time to start
            SetIndicatorTimes();
        }

        if (state == MiniGameState.Active && indicators.Count > 0)
        {
            //update all the indicators
            foreach (NIndicator ind in indicators)
            {
                ind.UpdateIndicator();
            }


            //check if the next indicators time has expired
            // if (Mathf.Abs((float)(AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime))) > hitToleranceTime)
            if ((indicators[0].startTime + indicators[0].endTime + hitToleranceTime) < AudioSettings.dspTime)
            {
                Debug.Log("bad");
                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }

            //so look at the bottom indicators end time, if the difference between that and now is bigger than the tolerance delete it


        }

        if (Input.GetKeyDown(KeyCode.Space) && indicators.Count > 0)
        {
            //check and see if this is a good input
            //Debug.Log(Mathf.Abs((float)(AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime))));

            if (Mathf.Abs((float)(AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime))) < hitToleranceTime)
            {
                //goood hit
                //delete the indicator and its gameobject
                //Debug.Log("good hit");

                pulsePad.Pulse();
                //so this should maybe like increase the buff by 1 to the neighbors or something?
                //need to recalculate the queue afterwords
                BattleManager.current.HandleHit(true);

                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }
            else
            {
                //u fucked up
                //Debug.Log("bad");
                BattleManager.current.HandleHit(false);
                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }

        }
    }
}
