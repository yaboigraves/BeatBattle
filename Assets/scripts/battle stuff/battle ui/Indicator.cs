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
    public Vector3 start;
    public Vector3 end;

    //so there are a couple different types of indicators
    //no type means its just a normal indicator (either defense or attack)

    public bool attackOrDefend;
    public string indicatorType;

    public SpriteRenderer sprite;


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
        beatOfThisNote = Mathf.Abs(transform.position.x);

        if (beatOfThisNote == 0)
        {
            Debug.LogWarning("INDICATOR POSITION 0 DETECTED, DONT DO THIS SET IT TO 0.1");
            beatOfThisNote = 0.01f;
        }

        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //end is 99 because we want to go 1 unit below the pad
        //end = new Vector3(transform.position.x, 99, transform.position.z);

        end = new Vector3(0, 0, 0);
    }

    public void SetIndicatorType(bool attackOrDefend, string indicType)
    {
        this.attackOrDefend = attackOrDefend;

        //so make a 1/4 chance for the indicator to actually even get the type set rather than setting 
        //makes pad processing easier 

        if (Random.Range(0, 3) > 1)
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
            transform.position = Vector3.Lerp(start, end, LightweightTrackTimeManager.current.songPositionInBeats / beatOfThisNote);
        }
    }

}
