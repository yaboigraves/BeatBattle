using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//dmg minigame

//lets just prototype this out as a one button hit the button on beat kind of game
//nice and simple indicators that spawn on a bar that shows you how many beats you have to wait to hit it
//that gives us some nice shit we can do like having some that may offset themselves/forward/backward in the pocket
//maybe have a dont hit these colored ones
//maybe you have echo notes that come back a bar later
//maybe you have ghost notes that appear only like one beat before they need to be hit so you have less reaction time



//so we need something to spawn indicators, something to manage indicators moving, and something to handle input for this


//so i think its probably best to just mash all the crap for the damage minigame in here
//subscripting out is gonna make extension harder

//first get something that spawns an indicator every n seconds n times based on the settings
public class DamageMiniGame : MiniGame
{
    public GameObject indicator;
    public Transform indicatorContainer;
    float beatOffset;
    RectTransform rectTransform;

    public List<NIndicator> indicators;

    public float hitToleranceTime = 1;

    private void Start()
    {
        base.LoadStuff();
        //load out some indicators
        //the distance between them can be statically set for now
        //actually we should have a ui panel for it that we use the size of it and divide it by like 8 to get that much space

        //figure out what our settings are, i guess we can just take default ones
        rectTransform = indicatorContainer.GetComponent<RectTransform>();

        beatOffset = rectTransform.rect.height / this.miniGameSettings.numBeats;
        SpawnIndicators();
    }

    void SpawnIndicators()
    {
        //just spawn one every 2 beats for now
        for (int i = 3; i < miniGameSettings.numBeats; i += 2)
        {
            //instantiate an indicator for each position
            GameObject ind = Instantiate(indicator, indicatorContainer.transform.position - (Vector3.up * (rectTransform.rect.height / 2)), Quaternion.identity, indicatorContainer);
            //move the indicator up 
            ind.transform.Translate(Vector3.up * (beatOffset * i));
            NIndicator indie = ind.GetComponent<NIndicator>();
            indie.SetIndicatorInfo(Vector3.up * (beatOffset * i), indicatorContainer.transform.position - (Vector3.up * (rectTransform.rect.height / 2)), i);
            indicators.Add(indie);
        }
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        SetIndicatorTimes();


    }

    void SetIndicatorTimes()
    {
        foreach (NIndicator n in indicators)
        {
            n.SetStartTime(AudioSettings.dspTime);
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

                // //check if we should delete the indicator
                // if (AudioSettings.dspTime - ind.endTime > hitToleranceTime)
                // {
                //     Debug.Log("bad");
                //     // Destroy(indicators[0].gameObject);
                //     // indicators.RemoveAt(0);
                // }
            }


            //check if the next indicators time has expired
            if (AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime) > hitToleranceTime)
            {
                Debug.Log("bad");
                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && indicators.Count > 0)
        {
            //check and see if this is a good input
            //Debug.Log(Mathf.Abs((float)(AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime))));

            if (Mathf.Abs((float)(AudioSettings.dspTime - (indicators[0].startTime + indicators[0].endTime))) < hitToleranceTime)
            {
                //goood hit
                //delete the indicator and its gameobject
                Debug.Log("good hit");
                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }
            else
            {
                //u fucked up
                //Debug.Log("bad");
                Destroy(indicators[0].gameObject);
                indicators.RemoveAt(0);
            }

        }
    }
}
