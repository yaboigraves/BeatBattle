using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
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


    public static Dictionary<string, List<System.Double>>[] loadMidis(string trackName, bool isTransition)
    {
        string path;
        if (isTransition)
        {
            path = "/audio/Midi/BattleTransitions";
        }
        else
        {
            path = "/audio/Midi/BattleTracks";
        }

        DirectoryInfo battleTracksInfo = new DirectoryInfo("Assets" + path);
        FileInfo[] info = battleTracksInfo.GetFiles("*.mid");

        List<Dictionary<string, List<System.Double>>> midiData = new List<Dictionary<string, List<System.Double>>>();
        foreach (FileInfo f in info)
        {
            if (f.Name.Contains(trackName))
            {


                //Debug.Log("trying to create python file execution");
                //extract the bpm from the fname
                string bpm = f.Name.Split('_')[2];
                Debug.Log(bpm);

                //remove the file extension 

                if (bpm.Contains("."))
                {
                    bpm = bpm.Substring(0, bpm.LastIndexOf('.'));


                }

                //check if theres another one





                Debug.Log(bpm);

                midiData.Add(parseMidi(f.Name, float.Parse(bpm), path));
            }
        }

        return midiData.ToArray();
    }





    public static Dictionary<string, List<System.Double>> parseMidi(string filename, float bpm, string prePath)
    {
        var engine = Python.CreateEngine();



        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        //searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
        engine.SetSearchPaths(searchPaths);



        //dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
        dynamic py = engine.ExecuteFile(Application.dataPath + @"/audio/Midi/MidiParser.py");
        dynamic midiParser = py.MidiParser();

        //Debug.Log(Application.dataPath + prePath + filename);
        //return (midiParser.parse(Application.dataPath + @"\midis\" + filename, bpm.ToString()));
        return (midiParser.parse(Application.dataPath + prePath + "/" + filename, bpm.ToString()));
    }

    public static Dictionary<string, List<System.Double>> parseQuickMixTrack(string trackName, float bpm)
    {


        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        //searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
        engine.SetSearchPaths(searchPaths);



        //dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
        dynamic py = engine.ExecuteFile(Application.dataPath + @"/scripts/audio/midis/MidiParser.py");
        dynamic midiParser = py.MidiParser();

        //return (midiParser.parse(Application.dataPath + @"\midis\" + filename, bpm.ToString()));
        return (midiParser.parse(Application.dataPath + @"/midis/" + trackName, bpm.ToString()));
    }

    public static Dictionary<string, List<System.Double>> parseLongMixTrack(string trackName)
    {
        return null;
    }

}
