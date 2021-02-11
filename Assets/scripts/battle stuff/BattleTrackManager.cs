
using System.Collections.Generic;
using UnityEngine;


public class BattleTrackManager : MonoBehaviour
{




    [Range(0.0f, 1.0f)]
    public float volume = 0.5f;

    public AudioSource mix1AudioSource, mix2AudioSource, transitionAudioSource;
    public Track currentTrack, nextTrack;
    public static BattleTrackManager current;

    public bool paused;
    public float currentBpm;
    public Track[] playerTracks;
    public Track[] testPlayerTracks;
    public Track[] testEnemyTracks;
    //if this is enabled play a sound when the metronome ticks
    public bool metronomeAudio;
    public int totalBeats;
    public Track playerSelectedTrack;
    //used by the track time manager to hopefully setup bar waits between tracks coming out
    public float countInBeats = 4;


    //=====================================

    public Queue<Track> trackQueue = new Queue<Track>();



    private void Awake()
    {
        current = this;
        if (TrackManager.current == null)
        {

            // playerSelectedTrack = testPlayerTracks[0];
            // currentTrack = playerSelectedTrack;
            // audioClip = testPlayerTracks[0].tracks[0].trackClip;
        }
        else
        {
            // playerTracks = GameManager.current.player.GetComponent<PlayerInventory>().battleEquippedTracks;
            // currentTrack = playerTracks[0];
            // playerSelectedTrack = playerTracks[0];
            // audioClip = currentTrack.trackClip;
        }


    }


    private void Start()
    {

        paused = true;
    }

    public void StartBattle()
    {
        paused = false;

        //audioSource.Play();
        BattleManager.current.battleStarted = true;

        //so from right here, going to start weaving in the track time manager
        TrackTimeManager.startTrackTimer();
    }

    public void StopBattle()
    {
        //for now just pauses but should play the win audio
        mix1AudioSource.Pause();
    }

    public void StartCountIn()
    {
        TrackTimeManager.beatWait(4);
    }

    public void StartQuickMixBattle()
    {
        //set the current track by dequeing from the queue
        currentTrack = trackQueue.Peek();
        //set the current bpm
        currentBpm = currentTrack.randomTrackData.bpm;
        TrackTimeManager.SetTrackData(currentTrack.randomTrackData);

        //set the audiosclip for mix1 
        mix1AudioSource.clip = currentTrack.randomTrackData.trackClip;

        //set the audioclip for the transition 
        transitionAudioSource.clip = currentTrack.randomTransitionData.trackClip;

        //queue up the first track and then the transition audio
        double startTime = AudioSettings.dspTime + (4 * (60d / currentTrack.randomTrackData.bpm));
        mix1AudioSource.PlayScheduled(startTime);
        mix1AudioSource.SetScheduledEndTime(startTime + mix1AudioSource.clip.length);

        //transitionAudioSource.PlayScheduled(AudioSettings.dspTime + (32 * (60d / currentTrack.randomTrackData.bpm)));
        //mix2AudioSource.PlayScheduled(AudioSettings.dspTime + (36 * (60d / currentTrack.randomTrackData.bpm)));


        //mix2AudioSource.PlayScheduled((float)AudioSettings.dspTime + 10 + currentTrack.randomTransitionData.numBeats * (60f / currentTrack.randomTransitionData.bpm));

        //TODO: double check this is working bc this might need a 4+ in front if 
        // TrackTimeManager.setBeatsBeforeNextPhase(4);
        //wait time 


        TrackTimeManager.beatWait(4);
        TrackTimeManager.AddEvent("nextPhase", 4 * (60 / currentTrack.randomTrackData.bpm));

    }

    //so this function is going to need to be called at the end of any phase to queue up the next track and set the phase data

    public void NextBattlePhase()
    {
        BattleManager.current.SetBattleStarted(true);
        if (trackQueue.Count <= 0)
        {
            Debug.Log("END OF TRACK QUEUE");
            Debug.Break();
        }

        Debug.Log("NEXT PHASE");



        //so this needs to also setup dynamic audio file playing on transitions, as well as 
        //managing the audio sources

        string currentPhase = BattleManager.current.battlePhase;

        double nextPhaseTime;

        switch (currentPhase)
        {

            //MIX1 MOVING INTO TRANSITION
            case "mix1":
                Debug.Log("NEXT PHASE : MIX1 -> TRANSITION");
                //Debug.Break();
                //transition started, going into mix2 next
                BattleManager.current.SetBattlePhase("transition");
                //so the bpm could switchup here, set the bpm to the next tracks transitions bpm

                //TODO: this probably needs to be scheduled
                //TrackTimeManager.SetTrackData(nextTrack.randomTransitionData);

                //so we're in the transition now, need to queue up mix2's audio to play
                //nextPhaseTime = nextTrack.randomTransitionData.numBeats * (60 / currentTrack.randomTransitionData.bpm);
                nextPhaseTime = nextTrack.randomTransitionData.trackClip.length;


                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);
                // TrackTimeManager.AddEvent("bpmSwitch", nextPhaseTime);

                mix2AudioSource.PlayScheduled((AudioSettings.dspTime) + nextPhaseTime);
                mix2AudioSource.SetScheduledEndTime((AudioSettings.dspTime) + nextPhaseTime + mix2AudioSource.clip.length);
                break;

            //MIX2 MOVING INTO TRANSITION 
            case "mix2":
                Debug.Log("NEXT PHASE : MIX1 -> TRANSITION");

                BattleManager.current.SetBattlePhase("transition");

                //so the bpm could switchup here, set the bpm to the next tracks transitions bpm
                //TrackTimeManager.SetTrackData(nextTrack.randomTransitionData);

                //nextPhaseTime = nextTrack.randomTransitionData.numBeats * (60 / currentTrack.randomTransitionData.bpm);
                nextPhaseTime = nextTrack.randomTransitionData.trackClip.length;


                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);
                // TrackTimeManager.AddEvent("bpmSwitch", nextPhaseTime);

                mix1AudioSource.PlayScheduled((AudioSettings.dspTime) + nextPhaseTime);
                mix1AudioSource.SetScheduledEndTime(AudioSettings.dspTime + nextPhaseTime + mix1AudioSource.clip.length);
                break;


            //TRANSITION MOVING INTO MIX1 OR MIX2
            case "transition":
                currentTrack = trackQueue.Dequeue();

                //update the bpm
                //TrackTimeManager.SetTrackData(currentTrack.randomTrackData);

                Debug.Log("Current Track Dequeued As : " + currentTrack.name);

                if (trackQueue.Count > 0)
                {
                    nextTrack = trackQueue.Peek();
                }

                //we're assuming at this point that the audio source is stopped
                if (BattleManager.current.lastMix == "mix1")
                {
                    Debug.Log("NEXT PHASE : TRANSITION -> MIX2");
                    //Debug.Break();

                    BattleManager.current.SetBattlePhase("mix2");
                    mix1AudioSource.clip = nextTrack.randomTrackData.trackClip;
                }
                else if (BattleManager.current.lastMix == "mix2")
                {
                    Debug.Log("NEXT PHASE : TRANSITION -> MIX1");
                    //Debug.Break();

                    BattleManager.current.SetBattlePhase("mix1");
                    mix2AudioSource.clip = nextTrack.randomTrackData.trackClip;
                }

                //nextPhaseTime = currentTrack.randomTrackData.numBeats * (60 / currentTrack.randomTrackData.bpm);

                //experimenting with using length instead
                nextPhaseTime = currentTrack.randomTrackData.trackClip.length;

                TrackTimeManager.AddEvent("nextPhase", nextPhaseTime);
                TrackTimeManager.AddEvent("bpmSwitch", nextPhaseTime);

                //load the audio source for the next transition
                transitionAudioSource.clip = nextTrack.randomTransitionData.trackClip;

                //schedule said audio source to play 
                Debug.Log("SCHEDULING TRANSITION AUDIO TO PLAY AT " + (AudioSettings.dspTime + nextPhaseTime));
                transitionAudioSource.PlayScheduled(AudioSettings.dspTime + nextPhaseTime);
                transitionAudioSource.SetScheduledEndTime(AudioSettings.dspTime + nextPhaseTime + transitionAudioSource.clip.length);
                break;
        }
    }

    public int nextTurnStart;

    //this is for setting the battle track for the longmix mode
    public double[] beatTimesArray;
    public void setupQuickMix()
    {
        //ok so we need some way of indicating in each track which transition and which track are going to be played
        //therefor the tracks are going to need two integer indexes for each which are randomly set here

        for (int i = 0; i < BattleManager.current.numQuickMixTracks; i++)
        {

            //to ensure no duplicates for now just testing
            //TODO: remove this lol
            Track t = testPlayerTracks[Random.Range(0, testPlayerTracks.Length)];


            t.randomTrackData = t.tracks[Random.Range(0, t.tracks.Length)];
            t.randomTransitionData = t.trackTransitions[Random.Range(0, t.trackTransitions.Length)];
            trackQueue.Enqueue(t);
        }

        IndicatorManager.current.setupQuickMixIndicators(trackQueue);
        beatTimesArray = TrackTimeManager.CalculateTrackBeatTimeLine(trackQueue);
    }


    //so we need a centralized place for the time passage to be managed from

    public void setPlayerSelectedTrack(Track newTrack)
    {
        playerSelectedTrack = newTrack;
    }

    //this is essentially a start function

    private void Update()
    {
        mix1AudioSource.volume = volume;
        mix2AudioSource.volume = volume;
        transitionAudioSource.volume = volume;
    }
}
