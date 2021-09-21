using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sample", menuName = "BeatBattle/Sample", order = 0)]
[System.Serializable]

//so each sample is also going to contain a track object which has all the midi data and shit in it

public class Sample : ScriptableObject
{

    public MiniGameScriptableObject miniGameScriptableObject;
    public string sampleName;

    public string functionName;

    //This is a placeholder for whatever effect this causes

    public int numericValue;

    //percentage modifier of the numeric value

    public SampleType sampleType;


    public string miniGameSceneName;

    //so this can be used
    //1.to actually load the correct song
    //2.to load the midi data that we're going to feed into the minigame


    public Track sampleTrack;

    public Sample DeepCopy(Sample s)
    {

        s.sampleName = sampleName;
        s.functionName = functionName;
        s.numericValue = numericValue;
        s.sampleTrack = sampleTrack;
        s.sampleType = sampleType;
        s.miniGameSceneName = miniGameSceneName;
        return s;

    }

}


public enum SampleType
{
    damage,
    heal,
    block
}
