using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this singletons job is to keep playing music in the overworld and then transition control to the battle audio controller when a fight happens
//its the big database of songs to pick from for battles, and for overworld background stuff
//experimenting with having this also manage battle audio, probably smart

public class TrackManager : MonoBehaviour
{
    public string[] backgroundAudioTrackJsons;
    public static TrackManager current;
    public Track[] backgroundAudioTracks;
    public Track[] battleAudioTracks;
    public AudioSource currAudio;
    public Track currTrack;
    public float currentBpm;
    public int currentTrack = 0;
    public bool paused;
    public float bpmTimer, beatDeltaTime;
    public int beat;
    public bool inBattle;
    public int battleBarsPerTurn = 2;
    int battleTurn;
    Coroutine songRoutine;

    void Awake()
    {
        if (current == null)
        {
            current = this;
            currAudio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitTracks();

        Track testTrack = TrackJsonParser.parseJSON(backgroundAudioTrackJsons[0]);
        currTrack = testTrack;

        UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
    }


    void InitTracks()
    {
        backgroundAudioTracks = new Track[backgroundAudioTrackJsons.Length];
        for (int i = 0; i < backgroundAudioTrackJsons.Length; i++)
        {
            backgroundAudioTracks[i] = TrackJsonParser.parseJSON(backgroundAudioTrackJsons[i]);
        }
    }


    void Update()
    {
        //TODO: fix this ugly god forsaken mess
        //probably redoable with coroutine
        if (!paused)
        {
            bpmTimer += Time.deltaTime;
            if (bpmTimer > beatDeltaTime)
            {
                bpmTimer = 0;
                beat++;

                if (beat > 3)
                {
                    beat = 0;

                    if (inBattle)
                    {
                        battleTurn++;
                        if (battleTurn >= battleBarsPerTurn)
                        {
                            // print("changiung turn");
                            BattleManager.current.changeTurn();
                            battleTurn = 0;
                        }
                    }
                }
            }
        }
    }

    public IEnumerator playSong()
    {
        currAudio.Play();
        yield return new WaitForSeconds(currAudio.clip.length);

        //for now it just picks a random track afterwords
        playRandomBackgroundTrack();
    }

    public void UpdateCurrentTrack(Track newTrack)
    {
        string path;

        if (newTrack.isBattleTrack)
        {
            path = "audio/battleTracks/";
        }
        else
        {
            path = "audio/backgroundTracks/";
        }

        AudioClip audioClip = (AudioClip)Resources.Load(path + newTrack.trackName);

        currAudio.clip = audioClip;
        currTrack = newTrack;

        currentBpm = newTrack.bpm;

        songRoutine = StartCoroutine(playSong());

        UIManager.current.updateCurrentTrack(newTrack);

        beatDeltaTime = (1 / currentBpm) * 60;
        bpmTimer = 0;
        beat = 0;
    }

    //so the problem is there's literally nothing in the background audio tracks
    //going to need to setup this array based on all the shit in the jsons array
    public void playRandomBackgroundTrack()
    {
        currentTrack = Random.Range(0, backgroundAudioTracks.Length);

        UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
    }

    public void PauseTrack()
    {
        currAudio.Pause();
        paused = true;
    }

    public void UnPauseTrack()
    {
        currAudio.UnPause();
        paused = false;
    }

    public void StopCurrentTrack()
    {
        StopCoroutine(songRoutine);
    }
}



