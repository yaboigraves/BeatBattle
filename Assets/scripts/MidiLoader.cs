using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;

public static class MidiLoader
{

    public static Dictionary<string, List<System.Double>> parseMidi(string filename)
    {
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);


        dynamic py = engine.ExecuteFile(Application.dataPath + @"\scripts\audio\midis\MidiParser.py");
        dynamic midiParser = py.MidiParser();

        return (midiParser.parse(Application.dataPath + @"\scripts\audio\midis\cunty.mid", "88"));
    }

}
