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

    Vector3 startPos;

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
        beatOfThisNote = transform.position.y - 100;

        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        end = new Vector3(transform.position.x, 100, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        //lerp notes 

        //so basically 




        activated = BattleManager.current.battleStarted;
        //need to move based on the bpm?
        //if bpm is 96 then one beat passes every 96/60
        if (activated)
        {
            //bpm = BattleTrackManager.current.currentBpm;
            //transform.Translate(0, Time.deltaTime * -moveSpeed, 0);
            // transform.position = startPos + Vector3.down * TrackTimeManager.current.songPositionInBeats;

            //just lerp down by 


            transform.position = Vector3.Lerp(start, end, TrackTimeManager.current.songPositionInBeats / beatOfThisNote);

        }
    }

    //this will go through all the indicator children and update their color 

}
