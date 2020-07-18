using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public string artist, trackName;
    public float bpm;

    //need to take in the number of bars the loops take up
    public int numBars;

    public AudioSource audioSource;

    public List<float> kickBeats = new List<float>();
    public List<float> snareBeats = new List<float>();

    public bool isBattleTrack;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            print("track error in track" + name);
        }
        //now we just use readfile to compile the drumlists for the loop here

        if (isBattleTrack)
        {
            (List<float>, List<float>) beatIndicators = ReadFile.readTextFile(@Application.dataPath + "/beatTracks/" + audioSource.clip.name + ".txt");
            kickBeats = beatIndicators.Item1;
            snareBeats = beatIndicators.Item2;

            if (kickBeats != null && snareBeats != null)
            {
                print("indicator tracks setup good");
            }
        }
    }

}
