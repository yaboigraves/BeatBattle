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

    public TrackData[] trackTransitions;

    [Header("")]
    public string artist;
    public float oldBPM;
    //need to take in the number of bars the loops take up
    public int numBars;
    public bool isBattleTrack;
    //public IndicatorData kickBeats;
    //public IndicatorData snareBeats;
    public TrackStats trackStats;

    //these are set when a track is queued up
    public TrackData randomTrackData, randomTransitionData;

    // public List<double> kickBeats, snareBeats, hatBeats, percBeats;


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

            //Debug.Log("transiction clip length" + transitionClips.Length.ToString());

            //so to get all the midi data the trackloader is also going to need a function to calculate that all
            Dictionary<string, List<System.Double>>[] tracksMidiData = TrackLoader.loadMidis(trackName, false);
            Dictionary<string, List<System.Double>>[] transitionsMidiData = TrackLoader.loadMidis(trackName, true);

            //go through all the trackclips and create a track data for them
            TrackData[] tData = new TrackData[trackClips.Length];
            TrackData[] transData = new TrackData[transitionClips.Length];




            for (int i = 0; i < trackClips.Length; i++)
            {
                tData[i].trackClip = trackClips[i];
                tData[i].bpm = parseBPM(trackClips[i].name);

                //if we know the length in seconds convert it to a length in minutes 
                //the number of beats per minute * minutes
                tData[i].numBeats = Mathf.RoundToInt(tData[i].bpm * (tData[i].trackClip.length / 60));
            }

            for (int i = 0; i < transitionClips.Length; i++)
            {
                transData[i].trackClip = transitionClips[i];
                transData[i].bpm = parseBPM(transitionClips[i].name);
                transData[i].numBeats = Mathf.RoundToInt(transData[i].bpm * (transData[i].trackClip.length / 60));

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
    public List<double> kickBeats, snareBeats, hatBeats, percBeats;
    public float bpm;

    public int numBeats;

}
