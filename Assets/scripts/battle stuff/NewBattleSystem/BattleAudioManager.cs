using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//so this is an absolutely confusing mess right now
//lets rewrite it to basically just handle switching audio sources and stuff
//were going to call it manually once, and after that at the end of every turn we're going to hand it the queue info and next track and let it setup and do shit


public class BattleAudioManager : MonoBehaviour
{
    public static BattleAudioManager current;

    //ok, so this def needs to be made into some kind of 'track' object that contains bpm info and the audio clip

    //we gotta juggle this though
    AudioSource musicAudioSource1, musicAudioSource2, currentAudioSource;

    public Track audioTrack, nextAudioTrack;




    private void Awake()
    {
        current = this;
        musicAudioSource1 = transform.GetChild(0).GetComponent<AudioSource>();
        musicAudioSource2 = transform.GetChild(1).GetComponent<AudioSource>();

    }

    private void Start()
    {
        // InitializeSongInfo();
    }

    //this cant really happen until we get the queue setup
    public void InitializeBattleAudio()
    {
        //set the bpm
        //set the audio source


        // musicAudioSource1.clip = audioTrack.trackClip;
        // TimeManager.SetCurrentSongInfo(audioTrack.oldBPM);

        //so this is going to be the manual call for this

        //battle manager needs to give us the current track and the next track

        //so we're gonna grab the track at the front and the track next
        audioTrack = BattleManager.current.battle.getCurrentTrack();
        nextAudioTrack = BattleManager.current.battle.getNextTrack();
        musicAudioSource1.clip = audioTrack.trackClip;
        musicAudioSource2.clip = nextAudioTrack.trackClip;
    }



    public void StartSong()
    {
        //this may need to be halted until the audio source can reliably be known to be playing
        //print("test");
        double battleStartTime = AudioSettings.dspTime + 0.5f;
        TimeManager.SetBattleStart(battleStartTime);
        TimeManager.SetCurrentSongInfo(audioTrack.oldBPM);

        musicAudioSource1.PlayScheduled(battleStartTime);
        //musicAudioSource2.PlayScheduled(battleStartTime + (TimeManager.timePerBeat * ((2 + BattleManager.current.battle.getCurrentTrack().numBars) * 4)));

        currentAudioSource = musicAudioSource1;

        nextBeatTime = TimeManager.battleStartTime + TimeManager.timePerBeat;
    }

    //so this gets called after we pop a track off
    public void LoadNextTrack()
    {
        //so this means a turn has ended.
        //turn off the current audio source
        // currentAudioSource.Stop();

        //the idea is the other audio source has already been scheduled to play at this point

        //Debug.Log("loading next track");
        // Debug.Break();
        nextAudioTrack = BattleManager.current.battle.getCurrentTrack();

        //so we need to do a slightly more complicated system for managing audio
        //need to check which audiosource we're currently using and then schedule the track on the next source

        if (currentAudioSource == musicAudioSource1)
        {
            //queue up the next song on audio source two
            musicAudioSource2.clip = nextAudioTrack.trackClip;
            //musicAudioSource2.PlayScheduled();
        }
        else
        {
            //queue up the next song on audio source one
            musicAudioSource1.clip = nextAudioTrack.trackClip;
        }
    }


    private void Update()
    {
        if (BattleManager.current.battle.currentState != BattleState.Prebattle)
        {
            //doesnt really make sense for this to be here but ok
            //CheckForBeat();
            TimeManager.CheckForBeat();
        }

    }


    public double lastBeatTime = 0, nextBeatTime = 0;
    //checks to see if we should trigger the beat callback

    //maybe move this somewhere else, probably time manager really doesnt need to be in here

    //so rather than maintaining a messy variable, lets just use the beat timeline 
    //we can track each beat as an index in the beattimelines array
    void CheckForBeat()
    {

        if (AudioSettings.dspTime >= nextBeatTime)
        {
            //print("nce");
            TimeManager.BeatCallBack();
            nextBeatTime = TimeManager.currentBeatDSPTime;

            //if we're at the end of a song, queue up the next one
            //going to need to track the length of the current track

            //so at the beggining of each turn we should queue up the next audio to play
            //this is going to require a second audio source, we can call these mix1 and mix2

        }
    }

    //ok just rewrite fucking everything :)

    //so this gets called at the end of every turn
    public void AudioUpdate()
    {
        //schedule the next song to play in one beat, schedule
        //currentAudioSource.SetScheduledEndTime(TimeManager.GetNextBeatDSPTime());
        currentAudioSource.Stop();

        //set the audio sources

        audioTrack = nextAudioTrack;
        nextAudioTrack = BattleManager.current.battle.getNextTrack();

        //so now we need to do some switch switches
        if (currentAudioSource == musicAudioSource1)
        {
            musicAudioSource2.clip = audioTrack.trackClip;
            musicAudioSource2.PlayScheduled(TimeManager.GetNextBeatDSPTime());
            currentAudioSource = musicAudioSource2;
        }
        else
        {
            musicAudioSource1.clip = audioTrack.trackClip;
            musicAudioSource1.PlayScheduled(TimeManager.GetNextBeatDSPTime());
            currentAudioSource = musicAudioSource1;
        }

        //so this actually needs to happen starting on the next beat,
        // TimeManager.SetCurrentSongInfo(audioTrack.oldBPM);
        TimeManager.BPMSwitch(audioTrack.oldBPM);






    }

}


// [System.Serializable]
// public class SongInfo
// {
//     public AudioClip clip;
//     public float bpm;
//     public int lengthInBeats;
// }