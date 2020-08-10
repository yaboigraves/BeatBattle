using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Track", menuName = "Tracks")]
public class TestTrack : ScriptableObject
{
    public AudioClip trackClip;
    public string artist, trackName;
    public float bpm;
    //need to take in the number of bars the loops take up
    public int numBars;
    public bool isBattleTrack;
    public List<float> kickBeats = new List<float>();
    public List<float> snareBeats = new List<float>();

}
