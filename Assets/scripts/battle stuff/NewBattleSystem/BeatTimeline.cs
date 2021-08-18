using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//so this object will just manage beatnode objects
//big array of these nodes that we can assign delegates to like the beatcallbacks
//ech array entry contains a time that the event occurs (one node for every beat)
//and a delegate list that can get called on that beat
//idea is that since all the events occur on the grid, we can just have a precomputed list of all the things occuring

public class BeatTimeline
{
    public BeatNode[] timeline;

    public int currentBeatIndex = 0;

    public void InitializeTimeline(Sample[] samples)
    {
        //figure out the amount of beats
        int numBeats = 0;

        foreach (Sample s in samples)
        {
            numBeats += s.sampleTrack.numBars * 4;
        }
        timeline = new BeatNode[numBeats];


        for (int beat = 0, i = 0; i < samples.Length; i++)
        {
            //as we go through each sample we add a new beat node for each beat
            //per song we need to know
            //-seconds per beat
            double secPerBeat = 60f / samples[i].sampleTrack.oldBPM;


            for (int b = 0; b < samples[i].sampleTrack.numBars * 4; b++)
            {
                double beatTime = secPerBeat * beat;


                timeline[beat] = new BeatNode(beatTime);
                beat++;
            }
        }

        Debug.Log("completed initializing beat timeline");
        Debug.Log(numBeats);
    }

    public void AddBeatCallback(int beatIndex, BattleManager.WaitCallback function)
    {

    }
}


public class BeatNode
{
    public double time;
    public List<BattleManager.WaitCallback> callbackFunction;
    public BeatNode(double time)
    {
        this.time = time;
    }

    public void Invoke()
    {
        //invokes all the callbacks assigned to the node
    }
}
