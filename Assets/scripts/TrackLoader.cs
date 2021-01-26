using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;
using System.IO;
using UnityEditor;
public static class TrackLoader
{

    public static AudioClip[] loadAudioClips(string trackName)
    {
        DirectoryInfo battleTracksInfo = new DirectoryInfo("Assets/audio/AudioFiles/BattleTracks");
        FileInfo[] info = battleTracksInfo.GetFiles("*.wav");

        Debug.Log(trackName);

        //go over all the filesnames and check if it includes the trackname
        //if so add it to the list of clips
        List<AudioClip> audioClips = new List<AudioClip>();
        foreach (FileInfo f in info)
        {
            if (f.FullName.Contains(trackName))
            {

                //if the filename has the trackname in it we're going to load the audio clip
                AudioClip c = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/audio/AudioFiles/BattleTracks/" + f.Name, typeof(AudioClip));

                //TODO: THIS STUFF SHOULD REALLY BE GOING DOWN IN MIDILOADER, WHICH SHOULD BE RENAMED TRACKLOADER
                //this stuff should really be going down in the midiloader (whic)
                audioClips.Add(c);
            }
        }
        return audioClips.ToArray();
    }

    public static Dictionary<string, List<System.Double>> parseMidi(string filename, float bpm)
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
        return (midiParser.parse(Application.dataPath + @"/midis/" + filename, bpm.ToString()));
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
