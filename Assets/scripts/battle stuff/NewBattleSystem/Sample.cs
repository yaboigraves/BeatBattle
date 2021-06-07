using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sample", menuName = "BeatBattle/Sample", order = 0)]
[System.Serializable]
public class Sample : ScriptableObject
{

    public string sampleName;

    public string functionName;

    //This is a placeholder for whatever effect this causes

    public int numericValue;

    //percentage modifier of the numeric value

    public SampleType sampleType;


}


public enum SampleType
{
    damage,
    heal,
    block
}
