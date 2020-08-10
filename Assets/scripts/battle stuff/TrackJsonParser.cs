using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;


public static class TrackJsonParser
{
    //so basically this needs to read a given json file name (will be in a specified folder so find that based on path)
    //it reads the json file then constructs a trackjson object with all the info, and then it will return a TrackComponent

    //format is 
    //trackname : string 
    //artist : string 
    //bpm : float 
    //numbars : int 
    //kickbeats : array
    //snarebeats : array


    public static Track parseJSON(string trackname)
    {
        string path = @Application.dataPath + "/audio/trackJsons/" + trackname;
        StreamReader reader = new StreamReader(path);
        string trackJson = reader.ReadToEnd();
        reader.Close();

        TrackJson trackJ = JsonUtility.FromJson<TrackJson>(trackJson);

        //could maybe make a constructor for this but do that later once it works
        Track track = new Track();
        track.artist = trackJ.artist;
        track.trackName = trackJ.trackname;
        track.isBattleTrack = trackJ.isBattleTrack;

        if (track.isBattleTrack)
        {
            track.bpm = trackJ.bpm;
            track.numBars = trackJ.numbars;
            track.kickBeats = trackJ.kickbeats.ToList();
            track.snareBeats = trackJ.snarebeats.ToList();
        }

        return track;
    }
}
