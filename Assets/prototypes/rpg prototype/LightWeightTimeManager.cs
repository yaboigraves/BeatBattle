using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWeightTimeManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static LightWeightTimeManager current;
    public float currentSongDSPStartTime;
    public float songPositionInBeats, songPosition;
    float tartTime;

    Track currentTrack;

    public delegate void BeatBufferFunction();
    public delegate void MessageBufferFunction(string msg);

    //hack bc cant package the strings with the function for some reason
    public List<string> messageBuffer;
    public List<MessageBufferFunction> messageFuncBuffer;
    public List<BeatBufferFunction> beatBuffer;

    //this is for stuff that you want to delay over a certain amount of beats
    public List<(float bufferPosition, BeatBufferFunction function)> longBeatBuffer;

    //this is for functions you want to run on the next bar (the next (1))
    public List<BeatBufferFunction> barBuffer;

    public int currentBeat = 1;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        beatBuffer = new List<BeatBufferFunction>();
        longBeatBuffer = new List<(float bufferPosition, BeatBufferFunction function)>();
        barBuffer = new List<BeatBufferFunction>();
        messageBuffer = new List<string>();
        messageFuncBuffer = new List<MessageBufferFunction>();
    }

    // Update is called once per frame

    float lastBeat = 0;
    void Update()
    {

        songPosition = (float)(AudioSettings.dspTime - currentSongDSPStartTime);
        songPositionInBeats = (songPosition / (60 / currentTrack.bpm));

        if (Mathf.Floor(songPositionInBeats) > lastBeat)
        {
            TickUpdate();
        }

        //check if we need to send out a tick update

    }


    public void TickUpdate()
    {
        lastBeat = Mathf.Floor(songPositionInBeats);
        currentBeat = (currentBeat % 4) + 1;

        if (currentBeat == 1)
        {
            processBarBuffer();
        }

        for (int i = 0; i < messageFuncBuffer.Count; i++)
        {
            messageFuncBuffer[i].Invoke(messageBuffer[i]);
        }

        messageFuncBuffer.Clear();
        messageBuffer.Clear();

        //now we can tell any stuff waiting in the beat buffer its time to run
        //loop through all the elements in the tick buffer
        for (int i = 0; i < beatBuffer.Count; i++)
        {
            beatBuffer[i].Invoke();
        }

        beatBuffer.Clear();

        processLongBeatBuffer();
    }

    public void processBarBuffer()
    {
        for (int i = 0; i < barBuffer.Count; i++)
        {
            barBuffer[i].Invoke();
        }

        barBuffer.Clear();


    }

    public void processLongBeatBuffer()
    {
        //go through all the things in the beatbuffer and

        // foreach ((float, BeatBufferFunction) bufferEntry in longBeatBuffer)
        // {
        //     float beat = bufferEntry.Item1;
        //     bufferEntry.Item1 = beat;
        // }

        for (int i = 0; i < longBeatBuffer.Count; i++)
        {
            float beat = longBeatBuffer[i].bufferPosition;



            if (beat <= 1)
            {
                //call the function 
                longBeatBuffer[i].function.Invoke();
                //remove it from the buffer 
                longBeatBuffer.RemoveAt(i);
            }
            else
            {
                (float, BeatBufferFunction) newBufferEntry;
                beat--;
                newBufferEntry.Item1 = beat;
                newBufferEntry.Item2 = longBeatBuffer[i].function;
                longBeatBuffer[i] = newBufferEntry;
            }



            //if the long buffer has been completly finished we can go on to the next phase of the battle

        }
    }

    public void SetCurrentDSPStartTime(Track track)
    {
        currentTrack = track;
        currentSongDSPStartTime = (float)AudioSettings.dspTime;
        songPosition = 0;
        lastBeat = 0;
    }

    

    



}
