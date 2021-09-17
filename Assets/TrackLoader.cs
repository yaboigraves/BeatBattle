using System.Collections;
using System.Collections.Generic;
// using IronPython.Hosting;


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
    public static Dictionary<string, List<System.Double>>[] loadMidis(string trackName, bool isTransition, float bpm)
    {
        string path;
        if (isTransition)
        {
            path = "/Dropbox/MFX/audio/Midi/BattleTransitions";
        }
        else
        {
            path = "/Dropbox/MFX/audio/Midi/BattleTracks";
        }

        DirectoryInfo battleTracksInfo = new DirectoryInfo("Assets" + path);
        FileInfo[] info = battleTracksInfo.GetFiles("*.mid");


        List<Dictionary<string, List<System.Double>>> midiData = new List<Dictionary<string, List<System.Double>>>();

        foreach (FileInfo f in info)
        {
            if (f.Name.Contains(trackName))
            {
                // Debug.Log("trying to create python file execution for " + trackName);
                //extract the bpm from the fname
                // string bpm = f.Name.Split('_')[2];
                // Debug.Log(bpm);

                //remove the file extension 

                // if (bpm.Contains("."))
                // {
                //     bpm = bpm.Substring(0, bpm.LastIndexOf('.'));
                // }

                //check if theres another one

                // Debug.Log(bpm);

                //TODO : Readd once compatible
                midiData.Add(parseMidi(f.Name, bpm, path));



            }
        }

        return midiData.ToArray();
    }



    //TODO: reappreoach/reimpliment

    public static Dictionary<string, List<System.Double>> parseMidi(string filename, float bpm, string prePath)
    {


        PythonRunner.RunString(@"
from mido import MidiFile
import mido
#import midi
import sys
# from System.Collections.Generic import *

import UnityEngine


class MidiParser:

    # so in reality all we need to do is just take in some different args
    def parse(self, filePath, bpmArg):
        totTime = 0

        # midi file
        # mid = MidiFile('midis/sonic midi shitfuck.mid')
        print(filePath)
        # so this is going to need to know when the filesystem needs it to look in the tracks or transitions folder
        mid = MidiFile(filePath)
        
        if mid:
            UnityEngine.Debug.Log('mid exists')
        else:
            UnityEngine.Debug.Log('mid does not exists')


        # bpm
        bpm = float(bpmArg)

        # times of midi events in millis for each track
        kickMessages = []
        hatMessages = []
        snareMessages = []
        percMessages = []

        # 96 bpm
        # 4 beats per bar
        # 24 bars per minute
        # 60/24 = 2.5 bars
        # each midi is 2 bars

        # gotta figure out how to calculate this based on bpm

        for i, track in enumerate(mid.tracks):
            totTime -= track[3].time
            # for msg in track:
            for j in range(3, len(track) - 1):
                UnityEngine.Debug.Log(track[j].type)
                totTime += track[j].time
                if(track[j].type == 'note_on'):
                    if (track[j].note == 72):
                        kickMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
                    if (track[j].note == 74):
                        snareMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
                    if (track[j].note == 76):
                        hatMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))
                    if (track[j].note == 41):
                        percMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm / 60))

        # note this gives the time of the last note off event, not the actual end of the midi track
        # print(mido.tick2second(totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * 2)
        UnityEngine.Debug.Log('eeeee')
        UnityEngine.Debug.Log(len(kickMessages))
        # print(snareMessages)


        #return messagesDictionary


mP = MidiParser()
UnityEngine.Debug.Log('here I go')
UnityEngine.Debug.Log(mP.parse('" + Application.dataPath + @"/Dropbox/MFX/audio/Midi/BattleTracks/" + filename + "',0))"

);

        Debug.Log(Application.dataPath + @"/Dropbox/MFX/audio/Midi/BattleTracks/" + filename);

        // string scriptPath = Application.dataPath + @"\Dropbox\MFX\audio\Midi\MidiParser.py";
        // PythonRunner.RunFile(scriptPath);


        // var engine = Python.CreateEngine();


        // ICollection<string> searchPaths = engine.GetSearchPaths();

        // //Path to the folder of greeter.py
        // searchPaths.Add(Application.dataPath);
        // //Path to the Python standard library
        // //searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        // searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
        // engine.SetSearchPaths(searchPaths);



        // //dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
        // dynamic py = engine.ExecuteFile(Application.dataPath + @"/audio/Midi/MidiParser.py");
        // dynamic midiParser = py.MidiParser();

        // //Debug.Log(Application.dataPath + prePath + filename);
        // //return (midiParser.parse(Application.dataPath + @"\midis\" + filename, bpm.ToString()));
        // return (midiParser.parse(Application.dataPath + prePath + "/" + filename, bpm.ToString()));

        return null;
    }

    public static Dictionary<string, List<System.Double>> parseQuickMixTrack(string trackName, float bpm)
    {
        // var engine = Python.CreateEngine();

        // ICollection<string> searchPaths = engine.GetSearchPaths();

        // //Path to the folder of greeter.py
        // searchPaths.Add(Application.dataPath);
        // //Path to the Python standard library
        // //searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        // searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
        // engine.SetSearchPaths(searchPaths);



        // //dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
        // dynamic py = engine.ExecuteFile(Application.dataPath + @"/scripts/audio/midis/MidiParser.py");
        // dynamic midiParser = py.MidiParser();

        // //return (midiParser.parse(Application.dataPath + @"\midis\" + filename, bpm.ToString()));
        // return (midiParser.parse(Application.dataPath + @"/midis/" + trackName, bpm.ToString()));

        return null;
    }

    public static Dictionary<string, List<System.Double>> parseLongMixTrack(string trackName)
    {
        return null;
    }

}
