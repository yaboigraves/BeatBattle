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

    BattleManager.WaitCallback endTurnCallback = BattleManager.current.battle.ChangeTurn;




    //TODO: diagnose bug where theres a weird
    public void InitializeTimeline(Sample[] samples)
    {
        //figure out the amount of beats
        int numBeats = 0;
        currentBeatIndex = 0;

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


            //ok, so if its the first beat of the next sample, it actually occurs based on the old sec per beat not the new one
            for (int b = 0; b < samples[i].sampleTrack.numBars * 4; b++)
            {

                if (i > 0 && b == 0)
                {
                    secPerBeat = 60f / samples[i - 1].sampleTrack.oldBPM;
                }
                else
                {
                    secPerBeat = 60f / samples[i].sampleTrack.oldBPM;
                }

                //oh this is calculating wrong, we should
                double beatTime;

                if (i == 0 && b == 0)
                {
                    beatTime = 0;
                }
                else
                {
                    beatTime = secPerBeat + timeline[beat - 1].time;
                }




                timeline[beat] = new BeatNode(beatTime);
                // Debug.Log(beatTime);


                //need to find a way to know if we're at the end
                if (b > (samples[i].sampleTrack.numBars * 4) - 1)
                {
                    //Debug.Log("added a end turn");
                    timeline[beat].AddCallback(endTurnCallback);
                }

                beat++;
            }
        }
        // Debug.Log("completed initializing beat timeline");
        // Debug.Log(numBeats);
    }

    public void AddBeatCallback(int beatIndex, BattleManager.WaitCallback function)
    {

    }

    public void AddEveryBeatCallback(BattleManager.WaitCallback function)
    {
        foreach (BeatNode n in timeline)
        {
            n.AddCallback(function);
        }
    }
}


public class BeatNode
{
    public double time;
    public List<BattleManager.WaitCallback> callbackFunctions = new List<BattleManager.WaitCallback>();
    public BeatNode(double time)
    {
        this.time = time;
    }

    public void AddCallback(BattleManager.WaitCallback callback)
    {
        callbackFunctions.Add(callback);
    }

    public void Invoke()
    {
        //invokes all the callbacks assigned to the node
        foreach (BattleManager.WaitCallback callback in callbackFunctions)
        {
            Debug.Log("doing invoke at " + time);
            // Debug.Log("invoked " + callback.Method.Name);
            callback.Invoke();
        }
    }
}
