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
    //public IndicatorData kickBeats;
    //public IndicatorData snareBeats;
    public TrackStats trackStats;

    public string midiFileName;

    public List<double> kickBeats, snareBeats;


    public void BuildMidi()
    {
        /*
            asks the midiLoader (abstract/static class) to load the midi data from the midiFileName
            -these files need to be in some sort of standardized folder (midi folder ) in the assets folder
        */

        Dictionary<string, List<double>> messageData = (MidiLoader.parseMidi(midiFileName, bpm));

        kickBeats = messageData["kick"];
        snareBeats = messageData["snare"];
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

