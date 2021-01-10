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

        Time.timeScale = newTrack.bpm / 60;

        //GameObject indicContainer = Instantiate(indicatorContainer, Vector3.zero, Quaternion.identity, transform);
        //TrackTimeManager.current.currIndicatorContainer = indicContainer;

        //instantiate uhhh 4 bars of bars so 16 total

        //TODO: this dont really need to be dynamically instantiated they can just be part of the container prefab

        for (int i = 0; i <= 128; i++)
        {
            GameObject _bar = Instantiate(bar, Vector3.up * (100 + i), Quaternion.identity, barContainer.transform);
        }

        for (int i = 0; i < track.kickBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(kickLane.transform.position.x, 3.1f + 100 + ((float)track.kickBeats[i]), 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, kickLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.snareBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(snareLane.transform.position.x, 3.1f + 100 + ((float)track.snareBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, snareLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.hatBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(hatLane.transform.position.x, 3.1f + 100 + ((float)track.hatBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, hatLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }

        for (int i = 0; i < track.percBeats.Count; i++)
        {
            Vector3 kickPos = new Vector3(percLane.transform.position.x, 3.1f + 100 + ((float)track.percBeats[i]), 0);
            GameObject indic = Instantiate(indicator, kickPos, Quaternion.identity, percLane.transform);
            indic.GetComponent<Indicator>().SetIndicatorType(BattleManager.current.playerTurn, newTrack.trackStats.trackVibe.ToString());
        }



        //lastIndicatorContainer = indicContainer;
    }
}
