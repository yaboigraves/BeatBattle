using System.Collections;
using System.Collections.Generic;
// using IronPython.Hosting;
using System.Linq;

using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Scripting.Python;
#endif



public static class TrackLoader
{
    public static AudioClip[] loadAudioClips(string trackName, bool isTransition)
    {
        string path;
        if (isTransition)
        {

            path = "Assets/audio/AudioFiles/BattleTransitions";
        }
        else
        {
            path = "Assets/audio/AudioFiles/BattleTracks";
        }


        DirectoryInfo battleTracksInfo = new DirectoryInfo(path);
        FileInfo[] info = battleTracksInfo.GetFiles("*.wav");

        //Debug.Log(trackName);

        //go over all the filesnames and check if it includes the trackname
        //if so add it to the list of clips
        List<AudioClip> audioClips = new List<AudioClip>();
        foreach (FileInfo f in info)
        {
            if (f.Name.Contains(trackName))
            {
                if (isTransition)
                {
                    //Debug.Log("TRANSITION DEBUG");
                    //Debug.Log(path + "/" + f.Name);
                    //Debug.Log("");
                }
                //if the filename has the trackname in it we're going to load the audio clip

#if UNITY_EDITOR
                AudioClip c = (AudioClip)AssetDatabase.LoadAssetAtPath(path + "/" + f.Name, typeof(AudioClip));

                audioClips.Add(c);
#endif
            }
        }
        return audioClips.ToArray();
    }

    //ok so this needs to be loaded based on some basic folder
    //folders already setup which is nice
    public static Dictionary<string, List<System.Double>>[] readMidis(string midiFileName, bool isTransition, float bpm)
    {
        string path;
        if (isTransition)
        {
            path = "/Dropbox/MFX/audio/Midi/BattleTransitions/";
        }
        else
        {
            path = "/Dropbox/MFX/audio/Midi/BattleTracks/";
        }


        DirectoryInfo battleTracksInfo = new DirectoryInfo("Assets" + path);
        FileInfo[] info = battleTracksInfo.GetFiles("*.mid");


        List<Dictionary<string, List<System.Double>>> midiData = new List<Dictionary<string, List<System.Double>>>();

        foreach (FileInfo f in info)
        {

            if (f.Name.Contains(midiFileName))
            {
                Debug.Log(f.Name);

                parseMidi(f.Name, bpm, path);

            }
        }

        return midiData.ToArray();
    }

    //so we need to know what directory to look in for this
    public static Dictionary<string, List<float>> readMidiTextFile(TrackData t, bool isTransition)
    {
        //actually this needs to load multiple files so keep that in mind, we're going to want to run this once per track
        //TODO: make dis work, also kinda gotta figure outexactly what its going to return
        //so we just gotta find where the path is first
        Dictionary<string, List<float>> midiData = new Dictionary<string, List<float>>();
        string path;
        if (!isTransition)
        {
            path = Application.dataPath + @"/Dropbox/MFX/audio/Midi/BattleTracks/" + t.midiFileName + ".mid.txt";

        }
        else
        {
            path = Application.dataPath + @"/Dropbox/MFX/audio/Midi/BattleTransitions/" + t.midiFileName + ".mid.txt";

        }

        //open the file at that location first of all
        Debug.Log(path);
        string[] lines = System.IO.File.ReadAllLines(path);
        //ok so now we just packacge these up into float arrays
        //package each of these up, depends how many there are
        string[] laneNames = new string[] { "ch1", "ch2", "ch3", "ch4" };

        for (int i = 0; i < lines.Length; i++)
        {
            List<float> beats = TrackLoader.convertBeatsToFloat(lines[i].Split(' ').ToList());
            // Debug.Log("adding " + laneNames[i]);
            midiData.Add(laneNames[i], beats);
        }



        return midiData;
    }

    static List<float> convertBeatsToFloat(List<string> beats)
    {
        List<float> fBeats = new List<float>();

        for (int i = 1; i < beats.Count - 1; i++)
        {
            string b = beats[i];
            fBeats.Add(float.Parse(b));
        }

        return fBeats;
    }



    //TODO: this needs to support transition paths as well
    public static void parseMidi(string filename, float bpm, string prePath)
    {



        // PythonRunner.SpawnClient(Application.dataPath + @"/Dropbox/MFX/audio/Midi/MidiParser.py", true, args);


        PythonRunner.RunString(@$"
from mido import MidiFile
import mido
#import midi
import sys
# from System.Collections.Generic import *
import io

import UnityEngine

UnityEngine.Debug.Log('{filename}')

filePath = '{Application.dataPath + @prePath + filename}'

mid = MidiFile(filePath)

bpm = {bpm}

if mid:
    UnityEngine.Debug.Log('mid exists')
else:
    UnityEngine.Debug.Log('mid does not exists')

kickMessages = []
hatMessages = []
snareMessages = []
percMessages = []

totTime = 0




for i, track in enumerate(mid.tracks):
    totTime -= track[3].time
    # for msg in track:
    for j in range(3, len(track) - 1):
        #UnityEngine.Debug.Log(track[j].type)
        totTime += track[j].time
        if(track[j].type == 'note_on'):
            if (track[j].note == 84):
              
                kickMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
            if (track[j].note == 86):
                snareMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
            if (track[j].note == 88):
                hatMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
            if (track[j].note == 90):
                percMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))

# time * beats/second 

UnityEngine.Debug.Log({bpm})
UnityEngine.Debug.Log(kickMessages[1] * {bpm}/60)
#write to file in format 
#k 0 1 2 3 etc
#s 0 6 4 3 etc
#h 4 6 8 4 etc
#p 5 8 9 2 etc

midiDataFile = open('{Application.dataPath + @prePath + filename + ".txt"}','w+')
#write the kickbeats
kickStr = 'k '
for k in kickMessages:
    kickStr += str(  round((k* {bpm} / 60),3)  ) + ' '
kickStr += '\n'

snareStr = 's '
for s in snareMessages:
    snareStr += str(  round((s* {bpm} / 60),3)  ) + ' '
snareStr += '\n'

hatStr = 'h '
for h in hatMessages:
    hatStr += str(  round((h* {bpm} / 60),3)  ) + ' '
hatStr += '\n'

percStr = 'p '
for p in percMessages:
    percStr += str(  round((p* {bpm} / 60),3)  ) + ' '
percStr += '\n'

midiDataFile.write(kickStr + snareStr + hatStr + percStr)

midiDataFile.close()

#try and fine the file with the name loaded i guess?");

    }



    // public static Dictionary<string, List<System.Double>> parseQuickMixTrack(string trackName, float bpm)
    // {
    //     // var engine = Python.CreateEngine();

    //     // ICollection<string> searchPaths = engine.GetSearchPaths();

    //     // //Path to the folder of greeter.py
    //     // searchPaths.Add(Application.dataPath);
    //     // //Path to the Python standard library
    //     // //searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
    //     // searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
    //     // engine.SetSearchPaths(searchPaths);



    //     // //dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
    //     // dynamic py = engine.ExecuteFile(Application.dataPath + @"/scripts/audio/midis/MidiParser.py");
    //     // dynamic midiParser = py.MidiParser();

    //     // //return (midiParser.parse(Application.dataPath + @"\midis\" + filename, bpm.ToString()));
    //     // return (midiParser.parse(Application.dataPath + @"/midis/" + trackName, bpm.ToString()));

    //     return null;
    // }

    // public static Dictionary<string, List<System.Double>> parseLongMixTrack(string trackName)
    // {
    //     return null;
    // }

}
