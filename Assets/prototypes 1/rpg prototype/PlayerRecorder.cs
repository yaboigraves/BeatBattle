using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRecorder : MonoBehaviour
{
    /*
    this things job is to basically just capture players input during an attack or defend phase
    you should have degrees of hitting the note for now half and whole swing
    for now just if you hit it on then note its full or around it by a certain range half
    need to know the times that the indicators are going to flash and we just check if an input is around then
        return true or false and process damage in the
    */

    public List<float> currentBeatIndicatorPositions;
    public float dspTimeOfCurBar;

    public float hitTolerance = 0.1f;

    public BeatBarVisualizer barVisualizer;


    void Start()
    {
        currentBeatIndicatorPositions = new List<float>();
        this.enabled = false;
    }

    public void StartRecord(List<float> beatTimes)
    {
        //turn on the visualizer

        //doing this when the ability is activated instead
        //barVisualizer.gameObject.SetActive(true);

        barVisualizer.InitSlider();

        //barVisualizer.SetGradient(beatTimes,hitTolerance);
        currentBeatIndicatorPositions = beatTimes;
        dspTimeOfCurBar = (float)AudioSettings.dspTime;
        this.enabled = true;
    }

    public void StopRecord()
    {
        this.enabled = false;
        //turn off the visualizer
        barVisualizer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        barVisualizer.UpdateSlider();

        if (Input.GetKeyDown(KeyCode.Space) && currentBeatIndicatorPositions.Count > 0)
        {
            if (Mathf.Abs(LightWeightTimeManager.current.currentBeat - currentBeatIndicatorPositions[0]) < hitTolerance)
            {
                //full good hit
                RPGBattleManager.current.ProcessHit(1);
            }
            else if ((Mathf.Abs(LightWeightTimeManager.current.currentBeat - currentBeatIndicatorPositions[0]) < hitTolerance + (hitTolerance / 2)))
            {
                //half good hit
                RPGBattleManager.current.ProcessHit(0.5f);
            }
            else
            {
                //miss
                RPGBattleManager.current.ProcessHit(0);
            }

            currentBeatIndicatorPositions.RemoveAt(0);
        }

        //go through all the indicators and check if the current beat is bigger than any of the indicators in the que (missed it)
        for (int i = 0; i < currentBeatIndicatorPositions.Count; i++)
        {
            if (currentBeatIndicatorPositions[i] < LightWeightTimeManager.current.currentBeat)
            {
                //this means we missed so send a miss
                RPGBattleManager.current.ProcessHit(0);
                currentBeatIndicatorPositions.RemoveAt(i);
            }
        }
    }


}
