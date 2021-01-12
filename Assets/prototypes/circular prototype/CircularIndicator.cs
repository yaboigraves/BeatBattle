using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularIndicator : MonoBehaviour
{
    //so this manages a chunk of indicators not just one indicator now
    public float bpm;
    public float moveSpeed;
    // public Transform bars, indicators;
    bool activated;
    public float beatOfThisNote;
    public Vector3 start;
    public Vector3 end;

    //so there are a couple different types of indicators
    //no type means its just a normal indicator (either defense or attack)
    public bool attackOrDefend;
    public string indicatorType;
    public SpriteRenderer sprite;


    //positions for heady beat movements
    public float firstSlerpStart, firstSlerpEnd;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (BattleTrackManager.current != null)
        {
            bpm = BattleTrackManager.current.currentBpm;
        }
        else
        {
            bpm = CircularBattleManager.current.testTrack.bpm;
        }

        //commented this to experiment with timescale rather than calculating movespeed 
        //moveSpeed = bpm / 60;

        //uniform move speed of 60bpm 
        moveSpeed = 1;

        //startPos = transform.position;

        //TODO: this depends on if its on a different horizontal plane, probably just the distance from 0
        //beatOfThisNote = Mathf.Abs(transform.position.x);

        if (beatOfThisNote == 0)
        {
            Debug.LogWarning("INDICATOR POSITION 0 DETECTED, DONT DO THIS SET IT TO 0.1");
            beatOfThisNote = 0.01f;
        }

        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //end is 99 because we want to go 1 unit below the pad
        //end = new Vector3(transform.position.x, 99, transform.position.z);

        //end = new Vector3(0, 0, 0);
    }

    public void SetIndicInfo(Vector3 end, float beatTime)
    {
        this.end = end;
        beatOfThisNote = beatTime;

        SetHeadyTriggerTimes();
    }

    public void SetIndicatorType(bool attackOrDefend, string indicType)
    {
        this.attackOrDefend = attackOrDefend;

        //so make a 1/4 chance for the indicator to actually even get the type set rather than setting 
        //makes pad processing easier 

        if (Random.Range(0, 10) > 8)
        {
            this.indicatorType = indicType;
        }

        switch (indicatorType)
        {
            case "Heady":

                sprite.color = Color.green;

                break;

            default:
                if (this.attackOrDefend)
                {
                    sprite.color = Color.red;
                }
                else
                {
                    sprite.color = Color.blue;
                }
                break;
        }
    }

    public float secondSlerpStart, secondSlerpEnd, lerpProgress, finalLerpStart, finalLerpEnd;

    public void SetHeadyTriggerTimes()
    {

        //so the slerp needs to start 2 beats away from the indicator
        //the beat of this note is the beat's distance from that indicator
        //so if our beat is located at beat 10
        //the slerp would need to start at 0.8( will this always be true?)
        //if the beat is located at 8 needs to slerp on beat 2 
        //that means 6 beats need to pass

        firstSlerpStart = (beatOfThisNote - 2) / beatOfThisNote;

        //the first slerp is going to end 1/2 a beat after it starts
        firstSlerpEnd = (beatOfThisNote - 1.5f) / beatOfThisNote;

        //so the second slerp starts at the first slerps end 
        secondSlerpStart = firstSlerpEnd;

        //the second slerp ends 1 away from the beat end 

        secondSlerpEnd = (beatOfThisNote - 1) / beatOfThisNote;

        finalLerpStart = secondSlerpEnd;
        finalLerpEnd = 1;


    }

    //heady lerp positional variables
    Vector3 slerpStartPos, slerpEndPos;
    bool firstSlerpStarted, secondSlerpStarted;
    bool finalLerpStarted;



    // Update is called once per frame
    void Update()
    {
        if (BattleManager.current != null)
        {
            activated = BattleManager.current.battleStarted;
        }
        else
        {
            activated = CircularBattleManager.current.battleStarted;
        }

        if (activated)
        {

            lerpProgress = LightweightTrackTimeManager.current.songPositionInBeats / beatOfThisNote;
            // normal indicator movement
            if (indicatorType == "Heady")
            {
                //so if the type is heady, once the indicator is two bar away it needs to slerp over to the other side then continue lerping to a new destination

                //these points in the song need to be precalculated doing it on the fly is a) stupid b) slow c)annoying
                //1.havent reached the slerp part so normal lerp

                if (lerpProgress < firstSlerpStart)
                {
                    transform.position = Vector3.Lerp(start, end, lerpProgress);
                }
                else if (lerpProgress >= firstSlerpStart && lerpProgress <= firstSlerpEnd)
                {

                    //so here we slerp starting from the left up to the 
                    //the slerp progress is 
                    float slerpProgress = (lerpProgress - firstSlerpStart) / (firstSlerpEnd - firstSlerpStart);
                    transform.position = Vector3.Slerp(Vector3.left * 2, new Vector3(0, 2.5f, 0), slerpProgress);

                }
                else if (lerpProgress >= firstSlerpEnd && lerpProgress < secondSlerpEnd)
                {
                    float slerpProgress = (lerpProgress - secondSlerpStart) / (secondSlerpEnd - secondSlerpStart);
                    transform.position = Vector3.Slerp(new Vector3(0, 2.5f, 0), new Vector3(2, 0f, 0), slerpProgress);

                    //lerp down to the 
                }
                else if (lerpProgress < 1)
                {
                    //the final lerp 
                    //lerp from 1 away on the opposite indicator to the new indicator destination

                    float yes = (lerpProgress - finalLerpStart) / (1 - finalLerpStart);
                    transform.position = Vector3.Lerp(new Vector3(2, 0, 0), new Vector3(1, 0, 0), yes);
                }
                else
                {
                    missedLerp();

                }


            }

            else if (lerpProgress < 1)
            {
                transform.position = Vector3.Lerp(start, end, lerpProgress);
            }

            else
            {
                missedLerp();
            }
        }
    }

    public bool missedLerpStarted = false;
    public float missedLerpStartBeat;
    Vector3 missedLerpStart;



    void missedLerp()
    {
        //so this is where we finally lerp to the center and get cleaned up
        //mark the time we started this final transition as the missLerpStart beat
        if (!missedLerpStarted)
        {
            missedLerpStarted = true;
            missedLerpStartBeat = LightweightTrackTimeManager.current.songPositionInBeats;
            missedLerpStart = transform.position;
        }

        float missedLerpProgress = (LightweightTrackTimeManager.current.songPositionInBeats - missedLerpStartBeat) / 1;

        transform.position = Vector3.Lerp(missedLerpStart, Vector3.zero, missedLerpProgress);

    }
}
