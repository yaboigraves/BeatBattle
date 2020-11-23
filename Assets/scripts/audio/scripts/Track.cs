using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Track", menuName = "Tracks/Track")]

[Serializable]
public class Track : GameItem
{
    public AudioClip trackClip;
    public string artist;
    public float bpm;
    //need to take in the number of bars the loops take up
    public int numBars;
    public bool isBattleTrack;
    public IndicatorData kickBeats;
    public IndicatorData snareBeats;




    public TrackStats trackStats;

    //public List<float> kickBeats = new List<float>();
    //public List<float> snareBeats = new List<float>();

}

[Serializable]
public class IndicatorData
{
    [TextArea]
    public string indicatorData;
    public float[] indicatorPositions;
    public IndicatorData()
    {
        if (indicatorData != null)
        {
            indicatorPositions = Array.ConvertAll(indicatorData.Split(' '), float.Parse);
        }
    }

    public void initData()
    {
        if (indicatorData != null)
        {
            indicatorPositions = Array.ConvertAll(indicatorData.Split(' '), float.Parse);
        }
    }
}


//this is a struct to contain stat info about the track
[Serializable]
public struct TrackStats
{
    //heat or chill
    public enum vibeType
    {
        Heady,
        Chill
    };

    public vibeType trackVibe;

    public int vibePerHit;
}

