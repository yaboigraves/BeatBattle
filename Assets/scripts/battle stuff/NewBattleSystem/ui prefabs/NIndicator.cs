using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NIndicator : MonoBehaviour
{
    //so we need to track the time that the thingy spawned, and how long it should take to get to the pads position

    Vector3 initialPosition, endPosition;
    int beatOfNote;
    public double startTime, endTime;

    public void SetIndicatorInfo(Vector3 initialPos, Vector3 endPos, int beatOfNote)
    {
        initialPosition = transform.position;
        endPosition = endPos;
        this.beatOfNote = beatOfNote;
    }

    public void UpdateIndicator()
    {
        //lerp down from our initial position we're assigned
        //so for now just lerp fuck time who cares


        transform.position = Vector3.Lerp(initialPosition, endPosition, (float)((AudioSettings.dspTime - startTime) / endTime));
    }

    public void SetStartTime(double startTime)
    {
        this.startTime = startTime;

        //so this just needs to be multiplied by the timeperbeat
        this.endTime = (double)beatOfNote * TimeManager.GetTimePerBeat();

        // Debug.Log(startTime);
        // Debug.Log(endTime);
    }



}
