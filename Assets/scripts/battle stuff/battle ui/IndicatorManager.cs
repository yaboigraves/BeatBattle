using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject indicatorContainer;

    public GameObject barContainer;

    public GameObject kickLane, snareLane, hatLane, percLane;

    public GameObject bar;
    public static IndicatorManager current;

    public float barSpawnPosition = 64;

    void Awake()
    {
        current = this;
    }

    GameObject lastIndicatorContainer;
    public void setupTurnIndicators(Track newTrack)
    {


        if (lastIndicatorContainer != null)
        {
            Destroy(lastIndicatorContainer);
        }

        Track track = newTrack;

        //TIMESCALE STUFF
        //so the uniform timescale is 60bpm. therefore the timescale we want is whatever the bpm of the curre

        //Time.timeScale = newTrack.oldBPM / 60;

        //GameObject indicContainer = Instantiate(indicatorContainer, Vector3.zero, Quaternion.identity, transform);
        //TrackTimeManager.current.currIndicatorContainer = indicContainer;

        //instantiate uhhh 4 bars of bars so 16 total

        //TODO: this dont really need to be dynamically instantiated they can just be part of the container prefab

        for (int i = 0; i <= barSpawnPosition; i++)
        {
            // GameObject _bar = Instantiate(bar, Vector3.up * (i) / 2, Quaternion.identity, barContainer.transform);
            spawnBar(i);
        }

        for (int i = 0; i < track.tracks[0].kickBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(kickLane.transform.position.x, ((float)track.tracks[0].kickBeats[i]), 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, kickLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.tracks[0].snareBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(snareLane.transform.position.x, ((float)track.tracks[0].snareBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, snareLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.tracks[0].hatBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(hatLane.transform.position.x, ((float)track.tracks[0].hatBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, hatLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.tracks[0].percBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(percLane.transform.position.x, ((float)track.tracks[0].percBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, percLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        //lastIndicatorContainer = indicContainer;
    }


    public void setupQuickMixIndicators(Queue<Track> trackQueue)
    {
        //assumes for now we're always playing the first transition variant

        Track[] tracks = trackQueue.ToArray();


        //tracks the number of tracks and transitions setup (12 bars)
        //indicator counts as a half
        int numBarsSetup = 0;


        // while (trackQueue.Count > 0)
        // {

        //     Track track = trackQueue.Dequeue();
        //     Debug.Log(BattleTrackManager.current.trackQueue.Count);
        //     setupQuickMixTrack(track.tracks[0], numBarsSetup);

        //     //TODO: this should probably be a value stored in the trackdata
        //     numBarsSetup += 14;
        //     setupQuickMixTrack(track.trackTransitions[0], numBarsSetup);
        //     //transitions are pretty much always gonna be 4 beats
        //     numBarsSetup += 3;
        // }

        for (int i = 0; i <= barSpawnPosition; i++)
        {
            // GameObject _bar = Instantiate(bar, Vector3.up * (i) / 2, Quaternion.identity, barContainer.transform);
            spawnBar(i);
        }


        foreach (Track track in tracks)
        {
            Debug.Log(BattleTrackManager.current.trackQueue.Count);
            setupQuickMixTrack(track.tracks[0], numBarsSetup);
            //TODO: this should probably be a value stored in the trackdata
            numBarsSetup += track.tracks[0].numBeats;
            setupQuickMixTrack(track.trackTransitions[0], numBarsSetup);
            //transitions are pretty much always gonna be 4 beats
            numBarsSetup += track.trackTransitions[0].numBeats;
        }

    }

    //initializes a single track with the given offset from 0
    public void setupQuickMixTrack(TrackData track, int offset)
    {

        for (int i = 0; i < track.kickBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(kickLane.transform.position.x, ((float)track.kickBeats[i] + offset), 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, kickLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn);
        }

        for (int i = 0; i < track.snareBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(snareLane.transform.position.x, ((float)track.snareBeats[i] + offset), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, snareLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn);
        }

        for (int i = 0; i < track.hatBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(hatLane.transform.position.x, ((float)track.hatBeats[i] + offset), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, hatLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn);
        }

        for (int i = 0; i < track.percBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(percLane.transform.position.x, ((float)track.percBeats[i] + offset), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, percLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn);
        }
    }

    public void ClearIndicators()
    {
        //TODO: loop through all the indicators lanes and kill all the indicators there

        for (int i = 0; i < kickLane.transform.childCount; i++)
        {
            Destroy(kickLane.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < snareLane.transform.childCount; i++)
        {
            Destroy(snareLane.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < hatLane.transform.childCount; i++)
        {
            Destroy(hatLane.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < percLane.transform.childCount; i++)
        {
            Destroy(percLane.transform.GetChild(i).gameObject);
        }
    }

    public void ClearBars()
    {
        //TODO: loop through all the bars and kill them all
        for (int i = 0; i < barContainer.transform.childCount; i++)
        {
            Destroy(barContainer.transform.GetChild(i).gameObject);
        }
    }

    //

    public void spawnBar(float beatPosition)
    {
        GameObject _bar = Instantiate(bar, Vector3.up * beatPosition, Quaternion.identity, barContainer.transform);

    }
}
