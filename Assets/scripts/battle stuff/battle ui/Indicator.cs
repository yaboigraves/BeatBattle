using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //so this manages a chunk of indicators not just one indicator now
    public float bpm;
    public float moveSpeed;
    // public Transform bars, indicators;
    public bool activated;
    public float beatOfThisNote;
    public Vector3 start;
    public Vector3 end;

    //so there are a couple different types of indicators
    //no type means its just a normal indicator (either defense or attack)

    public bool attackOrDefend;
    public string indicatorType;

    public SpriteRenderer sprite;

    public double lerpStatus, finalLerpStatus;

    public bool isBar;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        bpm = BattleTrackManager.current.currentBpm;

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

        if (activated && lerpStatus < beatOfThisNote)
        {

            lerpStatus = TrackTimeManager.songPositionInBeats / beatOfThisNote;
            transform.position = Vector3.Lerp(start, end, (float)lerpStatus) + transform.parent.position;
        }

        //TODO: reipliment this
        // else if (activated && lerpStatus >= beatOfThisNote)
        // {
        //     finalLerpStatus = TrackTimeManager.songPositionInBeats - beatOfThisNote;
        //     //so now what we're going to do is lerp for one more unit over one more bar

        //     transform.position = Vector3.Lerp(end, end - new Vector3(0, end.y + 1, 0), (float)finalLerpStatus) + transform.parent.position;
        // }


        //once we reach the destination we're just going to lerp to one unit below over one more bar


    }

}
