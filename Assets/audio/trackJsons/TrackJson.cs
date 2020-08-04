using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is basically just a container for the json object read from the file
public class TrackJson
{
    public string trackname;
    public string artist;
    
    public bool isBattleTrack;

    
    public float bpm;
    public int numbars;
    public float[] kickbeats;
    public float[] snarebeats;

}
