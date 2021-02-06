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

    double finalLerpStatus;

    public bool isBar;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

    }
    void Start()
    {


        bpm = BattleTrackManager.current.currentBpm;

        //commented this to experiment with timescale rather than calculating movespeed 
        //moveSpeed = bpm / 60;

        //uniform move speed of 60bpm 
        moveSpeed = 1;



        //startPos = transform.position;
        if (isBar)
        {
            //account for delay
            beatOfThisNote = Mathf.Floor(transform.position.y);
            start = transform.position;
            end = Vector2.zero;
        }
        else
        {
            beatOfThisNote = transform.position.y;
        }

        if (beatOfThisNote == 0)
        {
            Debug.LogWarning("INDICATOR POSITION 0 DETECTED, DONT DO THIS SET IT TO 0.1");
            beatOfThisNote = 0.01f;
        }

        // start = new Vector3(transform.position.x, beatOfThisNote, transform.position.z);
        // //end is 99 because we want to go 1 unit below the pad
        // end = new Vector3(transform.position.x, 0, transform.position.z);


    }

    //TODO: so this should just initialize the info for where the indicator should be etc rather than scraping it from transform info
    public void SetIndicatorPosition(Vector3 start)
    {
        transform.position = start;
        this.start = start - transform.parent.position;
        this.end = new Vector3(start.x - transform.parent.position.x, 0, start.z - transform.parent.position.z);
        // if (mixNum == 2)
        // {
        //     print(start);
        //     start = start + new Vector3(4, 0, 0);
        //     end = end + new Vector3(4, 0, 0);
        // }
        //start = new Vector3(transform.position.x, beatOfThisNote, transform.position.z);
    }

    public void SetIndicatorType(bool attackOrDefend, string indicType = "")
    {
        this.attackOrDefend = attackOrDefend;

        //so make a 1/4 chance for the indicator to actually even get the type set rather than setting 
        //makes pad processing easier 

        if (Random.Range(0, 3) > 1)
        {
            //TODO: re-enable
            //this.indicatorType = indicType;
        }


        switch (indicatorType)
        {
            case "Heady":

                sprite.color = Color.green;

                break;

            default:
                if (this.attackOrDefend)
                {
                    sprite.color = Color.blue;
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
        activated = BattleManager.current.battleStarted;

        double lerpStatus = TrackTimeManager.songPositionInBeats / beatOfThisNote;
        //main travel lerp
        if (activated && lerpStatus < 1)
        {

            //TODO: so this cant be dependent on the song position in beats because it can change on the fly now, going to need an alternate way to calculate this/ probably going to need to use dsp time 
            //all the indicators essentially need to speed up while maintaining their same position on a bpm switchup

            transform.position = Vector3.Lerp(start, end, (float)TrackTimeManager.songPositionInBeats / beatOfThisNote) + transform.parent.position;
        }
        else if (activated && lerpStatus >= 1)
        {
            finalLerpStatus = TrackTimeManager.songPositionInBeats - beatOfThisNote;
            //so now what we're going to do is lerp for one more unit over one more bar

            transform.position = Vector3.Lerp(end, end - new Vector3(0, end.y + 1, 0), (float)finalLerpStatus) + transform.parent.position;
        }


        //once we reach the destination we're just going to lerp to one unit below over one more bar


    }

}
