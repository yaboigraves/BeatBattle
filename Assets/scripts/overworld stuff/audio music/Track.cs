using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//temporarily removing monobehaivour from this
//it never should really be instantiated as an object
//if this works trackjsonparser can be deleted 
[System.Serializable]
public class Track
{
    public string artist, trackName;
    public float bpm;

    //need to take in the number of bars the loops take up
    public int numBars;

    public List<float> kickBeats = new List<float>();
    public List<float> snareBeats = new List<float>();

    public bool isBattleTrack;

}
