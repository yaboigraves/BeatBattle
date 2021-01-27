using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Track", menuName = "Tracks/Track")]

[Serializable]
public class Track : GameItem
{

    public string trackName;

    [Header("AUDIO FILES")]
    public AudioClip trackClip;

    public TrackData[] tracks;

    public TransitionData[] trackTransitions;

    [Header("")]
    public string artist;
    public float bpm;
    //need to take in the number of bars the loops take up
    public int numBars;
    public bool isBattleTrack;
    //public IndicatorData kickBeats;
    //public IndicatorData snareBeats;
    public TrackStats trackStats;

    public List<double> kickBeats, snareBeats, hatBeats, percBeats;




    public void BuildTrack()
    {
        /*
            so this is not only going to build the midis but also set some other basic data
            main datapoint is the trackName which will be used to calculate all other stuff 

            FOR THE MOST PART THE TRACKLOADER DOES ALL THE HEAVY LIFTING TO KEEP THIS FILE FROM IMPORTING HELLA LIBS
        */

        if (trackStats.mixType == TrackStats.MixType.QuickMix)
        {
            //TODO: check if this works in builds dynamically
            AudioClip[] trackClips = TrackLoader.loadAudioClips(trackName, false);
            AudioClip[] transitionClips = TrackLoader.loadAudioClips(trackName, true);

            Debug.Log("transiction clip length" + transitionClips.Length.ToString());

            //so to get all the midi data the trackloader is also going to need a function to calculate that all
            Dictionary<string, List<System.Double>>[] tracksMidiData = TrackLoader.loadMidis(trackName, false);
            Dictionary<string, List<System.Double>>[] transitionsMidiData = TrackLoader.loadMidis(trackName, true);

            //go through all the trackclips and create a track data for them
            TrackData[] tData = new TrackData[trackClips.Length];
            TransitionData[] transData = new TransitionData[transitionClips.Length];


            for (int i = 0; i < trackClips.Length; i++)
            {
                tData[i].trackClip = trackClips[i];
            }

            for (int i = 0; i < transitionClips.Length; i++)
            {
                transData[i].transitionClip = transitionClips[i];
            }

            for (int i = 0; i < tracksMidiData.Length; i++)
            {
                tData[i].kickBeats = tracksMidiData[i]["kick"];
                tData[i].hatBeats = tracksMidiData[i]["hat"];
                tData[i].snareBeats = tracksMidiData[i]["snare"];
                tData[i].percBeats = tracksMidiData[i]["perc"];
            }


            for (int i = 0; i < transitionsMidiData.Length; i++)
            {
                if (i >= transData.Length)
                {
                    Debug.LogError("NOT ENOUGH DATA SPOTS FOR NEW MIDI");
                    break;
                }
                transData[i].kickBeats = transitionsMidiData[i]["kick"];
                transData[i].hatBeats = transitionsMidiData[i]["hat"];
                transData[i].snareBeats = transitionsMidiData[i]["snare"];
                transData[i].percBeats = transitionsMidiData[i]["perc"];
            }

            tracks = tData;
            trackTransitions = transData;

            //after we do all this we need to load the transition data as well for the track, basically the same process 
            //but the filepath differs
        }
        else if (trackStats.mixType == TrackStats.MixType.LongMix)
        {

        }



        // Dictionary<string, List<double>> messageData = (TrackLoader.parseMidi(midiFileName, bpm));


        // kickBeats = messageData["kick"];
        // snareBeats = messageData["snare"];
        // hatBeats = messageData["hat"];
        // percBeats = messageData["perc"];

        // //TODO: so this is also going to need to read all the transitions midi data and set that as well

        // for (int i = 0; i < trackTransitions.Length; i++)
        // {

        //     TransitionData t = trackTransitions[i];
        //     Dictionary<string, List<double>> transitionData = (TrackLoader.parseMidi("transition-midi/" + t.transitionMidiName, bpm));
        //     Debug.Log(transitionData["kick"].Count);
        //     t.kickBeats = transitionData["kick"];
        //     t.snareBeats = transitionData["snare"];
        //     t.hatBeats = transitionData["hat"];
        //     t.percBeats = transitionData["perc"];
        //     trackTransitions[i] = t;
        // }




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

public struct TransitionData
{
    public AudioClip transitionClip;
    public string transitionMidiName;
    public List<double> kickBeats, snareBeats, hatBeats, percBeats;
    public float bpm;

}

[Serializable]
public struct TrackData
{
    public AudioClip trackClip;
    public List<double> kickBeats, snareBeats, hatBeats, percBeats;
    public float bpm;

}
