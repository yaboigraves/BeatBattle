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

    public string currentTrackName, currentTrackArtist;
    public float currentBpm;

    public int currentTrack = 0;

    public bool paused;

    public float bpmTimer, beatDeltaTime;
    public int beat;

    public bool inBattle;

    public int battleBarsPerTurn = 2;
    int battleTurn;

   

    // Start is called before the first frame update

    void Awake()
    {
        if (current == null)
        {
            current = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        currAudio = GetComponent<AudioSource>();
        //print(backgroundAudioTracks[currentTrack].audioSource);
        Track testTrack = TrackJsonParser.parseJSON(backgroundAudioTrackJsons[0]);
        currTrack = testTrack;

        //print(currTrack);
        //UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
        UpdateCurrentTrack(currTrack);
    }

    // Update is called once per frame
    void Update()
    {

        if (currAudio.clip != null && currAudio.time >= currAudio.clip.length)
        {
  
            print("moving to next track");
            currentTrack = (currentTrack + 1) % backgroundAudioTracks.Length;
            UpdateCurrentTrack(backgroundAudioTracks[currentTrack]);
        }


        //TODO: fix this ugly god forsaken mess
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

    public void UpdateCurrentTrack(Track newTrack)
    {

        print(newTrack.trackName);
        //TODO: filestructure will need to be organized based on level

        string path;

        if(newTrack.isBattleTrack){
            path = @Application.dataPath + "/audio/battleTracks/" + newTrack.trackName + ".wav";
        }
        else{
            path = @Application.dataPath + "/audio/backgroundTracks/" + newTrack.trackName + ".wav";
        }

        //print(path);
    
        AudioClip audioClip = WavUtility.ToAudioClip (path);
        //print(audioClip.length);
      
        currAudio.clip = audioClip;
    
        //print(newTrack.audioSource.clip);
        //currAudio.clip = newTrack.audioSource.clip;
        currentTrackName = newTrack.trackName;
        currentTrackArtist = newTrack.artist;
        currentBpm = newTrack.bpm;
        currAudio.Play();
        //tell the ui manager to update
        UIManager.current.updateCurrentTrack(newTrack);

        beatDeltaTime = (1 / currentBpm) * 60;
        bpmTimer = 0;
        beat = 0;
        //StartCoroutine(beatTick());
        currTrack = newTrack;
    }

    //TODO: fix this shit
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
}



