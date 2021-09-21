using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Track", menuName = "Tracks/Track")]

[System.Serializable]
public class Track : GameItem
{
    public string trackName;

    [Header("AUDIO FILES")]
    // public AudioClip trackClip;
    public TrackData[] tracks;
    public TrackData[] trackTransitions;
    public string artist;
    public float bpm;
    public int numBars;
    public TrackStats trackStats;


    //9/16 rewrite notes
    /*
        -ok so we gotta get this runnin and loading into the track
        so each individual track is gonna have some midi data it loads
        so each track needs a midifilename? i guess?
        //for now lets just get it loading one track
    */


    public void BuildTrack()
    {
        /*
            so this is not only going to build the midis but also set some other basic data
            main datapoint is the trackName which will be used to calculate all other stuff 

            FOR THE MOST PART THE TRACKLOADER DOES ALL THE HEAVY LIFTING TO KEEP THIS FILE FROM IMPORTING HELLA LIBS
        */


        // if (trackStats.mixType == TrackStats.MixType.QuickMix)
        // {

        // }
        // else if (trackStats.mixType == TrackStats.MixType.LongMix)
        // {

        // }

        //9/16 rewrite
        //go through all the trackdata
        foreach (TrackData track in tracks)
        {
            TrackLoader.readMidis(track.midiFileName, false, bpm);
            //then manually call the actual loader
        }

        //read all the midis for the transitions too

        foreach (TrackData trans in trackTransitions)
        {
            Debug.Log("loading transition data");
            TrackLoader.readMidis(trans.midiFileName, true, bpm);
        }


    }

    //so this is the one that will actually return the good stuff 
    public void LoadTrack()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            TrackData track = tracks[i];
            Debug.Log("loading tracks");

            Dictionary<string, List<float>> midiData = TrackLoader.readMidiTextFile(track, false);

            //load it into that track

            if (midiData.ContainsKey("ch1"))
            {

                track.ch1 = midiData["ch1"];

            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch2 = midiData["ch2"];
            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch3 = midiData["ch3"];
            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch4 = midiData["ch4"];
            }

            tracks[i] = track;
        }

        for (int i = 0; i < trackTransitions.Length; i++)
        {
            TrackData track = trackTransitions[i];
            Debug.Log("loading transitions");
            Dictionary<string, List<float>> midiData = TrackLoader.readMidiTextFile(track, true);

            //load it into that track

            if (midiData.ContainsKey("ch1"))
            {

                track.ch1 = midiData["ch1"];

            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch2 = midiData["ch2"];
            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch3 = midiData["ch3"];
            }
            if (midiData.ContainsKey("ch1"))
            {

                track.ch4 = midiData["ch4"];
            }

            trackTransitions[i] = track;
        }
    }


    public float parseBPM(string fileName)
    {
        //format is 
        //artist-name_song-name_bpm_[transition]._
        //transition bit is optional so if parsing the bpm will contain the file extension if its not a transition

        string bpm = fileName.Split('_')[2];

        // if (bpm.Contains("."))
        // {
        //     bpm = bpm.Remove(bpm.IndexOf('.'), 4);
        // }

        if (bpm.Contains("bpm"))
        {
            bpm = bpm.Remove(bpm.IndexOf('b'), 3);
        }

        Debug.Log("PARSING BPM : " + bpm);
        // Debug.Log(fileName);

        return float.Parse(bpm);
    }
}



//this is a struct to contain stat info about the track
[Serializable]
public struct TrackStats
{
    //heat or chill
    public enum VibeType
    {
        Heady,
        Chill
    };

    public enum MixType
    {
        QuickMix,
        LongMix
    };

    public VibeType trackVibe;
    public MixType mixType;
    public int vibePerHit;
}


[Serializable]
public struct TrackData
{
    public AudioClip trackClip;

    //8/28 rewrite notes: so for now we're just gonna keep this system but im going to manually enter this info
    //kick beats will be the de-facto testing channel, the idea is we want 4 channels possible for now but not necessarily tied to any drum

    public List<float> ch1, ch2, ch3, ch4;

    //so bpm is now loaded per track not per individual track
    // public float bpm;
    public int numBeats;

    public string midiFileName;
}
