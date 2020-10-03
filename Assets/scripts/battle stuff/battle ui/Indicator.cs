using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //so this manages a chunk of indicators not just one indicator now
    public float bpm;
    public float moveSpeed;
    // public Transform bars, indicators;
    bool activated;
    public float beatOfThisNote;
    Vector3 start;
    Vector3 end;

    void Start()
    {
        bpm = BattleTrackManager.current.currentBpm;

        //commented this to experiment with timescale rather than calculating movespeed 
        //moveSpeed = bpm / 60;

        //uniform move speed of 60bpm 
        moveSpeed = 1;

        //startPos = transform.position;
        beatOfThisNote = transform.position.y - 100 + 1;

        if (beatOfThisNote == 0)
        {
            Debug.LogWarning("INDICATOR POSITION 0 DETECTED, DONT DO THIS SET IT TO 0.1");
            beatOfThisNote = 0.01f;
        }

        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //end is 99 because we want to go 1 unit below the pad
        end = new Vector3(transform.position.x, 99, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        activated = BattleManager.current.battleStarted;

        if (activated)
        {
            transform.position = Vector3.Lerp(start, end, TrackTimeManager.current.songPositionInBeats / beatOfThisNote) + transform.parent.position;
        }
    }

    //so the countin does 4 beats of updating, and rather than moving the indicators it moves the indicator container
}
