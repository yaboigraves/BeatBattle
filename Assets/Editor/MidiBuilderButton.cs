using UnityEditor;
using UnityEngine;
//make sure unity engine isnt imported or this shits the bed

[CustomEditor(typeof(Track))]
public class MidiBuilderButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();

        Track track = (Track)target;

        if (GUILayout.Button("Build Track Data"))
        {
            track.BuildTrack();
        }
    }
}
