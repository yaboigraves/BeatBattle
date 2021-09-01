using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//8/28 notes
/*
    so these need to scale with dynamic bpm
    -double check that the way they're getting it actually works lol


*/

public class NIndicator : MonoBehaviour
{
    //so we need to track the time that the thingy spawned, and how long it should take to get to the pads position

    Vector3 initialPosition, endPosition;
    int beatOfNote, beatCounter;
    public double startTime, endTime;

    //THIS IS IN PERCENTAGE 0-1
    public float toleranceAmount = 0.20f;

    public IndicatorState state;

    public TextMeshProUGUI countDownText;

    //used for the healing minigame
    public int lane;

    public enum IndicatorState
    {
        Waiting,
        Moving,
        PastMoving,
        Expired
    }

    public void SetIndicatorInfo(Vector3 initialPos, Vector3 endPos, int beatOfNote)
    {
        initialPosition = transform.position;
        endPosition = endPos;
        this.beatOfNote = beatOfNote;

        this.beatCounter = beatOfNote;
        countDownText = GetComponentInChildren<TextMeshProUGUI>();
        countDownText.text = beatOfNote.ToString();

        TimeManager.beatCallbacks.Add(UpdateCountdownText);
    }

    public void SetIndicatorInfo(Vector3 initialPos, Vector3 endPos, int beatOfNote, int lane)
    {
        initialPosition = transform.position;
        endPosition = endPos;
        this.beatOfNote = beatOfNote;
        this.lane = lane;
        this.beatCounter = beatOfNote;
        countDownText = GetComponentInChildren<TextMeshProUGUI>();
        countDownText.text = beatOfNote.ToString();

        TimeManager.beatCallbacks.Add(UpdateCountdownText);
    }



    //add some generalized way to check if the indicator should be getting cleaned up
    //if this returns false its time to clean the indicator up
    public IndicatorState UpdateIndicator()
    {
        //lerp down from our initial position we're assigned
        //so for now just lerp fuck time who cares

        float lerpProgress = (float)((AudioSettings.dspTime - startTime) / endTime);
        transform.position = Vector3.Lerp(initialPosition, endPosition, lerpProgress);



        if (lerpProgress > 1f + toleranceAmount)
        {
            return IndicatorState.Expired;
        }

        else if (lerpProgress > 1f)
        {
            return IndicatorState.PastMoving;
        }
        else
        {
            return IndicatorState.Moving;
        }


    }

    public void SetStartTime(double startTime, float bpm)
    {
        this.startTime = startTime;

        //so this just needs to be multiplied by the timeperbeat
        //so this should be set by the track not by the time manager hoenstly
        //so honestly, this should just use the beat timeline to know when they should stop rather than calculating it

        //so we know the beat of each note (0-15)
        //we can just look at whatever the current minigame position is in the beat timeline and get the time from there
        

        this.endTime = (double)beatOfNote * (60f / bpm);

        // Debug.Log(startTime);
        // Debug.Log(endTime);
    }


    public void UpdateCountdownText()
    {


        if (beatCounter == 0)
        {
            countDownText.text = "!!!";
        }
        else
        {
            countDownText.text = beatCounter.ToString();

        }

        beatCounter--;
    }


}
