using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;

public class PyhonTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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
        print(midiParser.parse(Application.dataPath + @"\scripts\audio\midis\cunty.mid", "88"));


        //Debug.Log(greeter.random_number(1, 5));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
