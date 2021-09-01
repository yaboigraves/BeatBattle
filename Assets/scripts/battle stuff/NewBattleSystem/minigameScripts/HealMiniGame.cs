using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealMiniGame : MiniGame
{

    //so heal minigame will be like a 4 lane catch the icon thing, and the indicators do the same thing and travel down to the bottom
    //you can only move the bucket on every beat, so we have to open and close the window

    //so what are we gonna need
    //-1 overall container
    //-4 lanes for the indicators to move down
    //-1 catcher thing


    //gonna need a callback function for assigning stuff to happen every beat

    int moveInput;
    public RectTransform catcher;

    public float toleranceTime = 0.3f;

    public GameObject indicator;

    public Transform[] lanes;

    List<NIndicator> indicators = new List<NIndicator>();

    public int currentCatcherLane = 1;


    float inputLockTime = 0.075f;
    bool inputLocked;

    //
    float beatOffset;
    private void Start()
    {
        base.LoadStuff();
        //TimeManager.beatCallbacks.Add(MoveCallback);
        beatOffset = lanes[0].GetComponent<RectTransform>().rect.height / 8;
    }
    //



    private void Update()
    {
        if (state == MiniGameState.Inactive)
        {
            return;
        }


        //update the indicators
        List<NIndicator> killList = new List<NIndicator>();
        foreach (NIndicator n in indicators)
        {
            NIndicator.IndicatorState state = n.UpdateIndicator();

            if (state == NIndicator.IndicatorState.Expired)
            {
                //destroy the indicator
                killList.Add(n);
            }
            else if (state == NIndicator.IndicatorState.PastMoving)
            {
                //add it to the kill list only if the hand is in the same lane
                if (n.lane == currentCatcherLane)
                {
                    killList.Add(n);
                    Debug.Log("Catch!!");

                    //heal the player 1 point
                    BattleManager.current.HealPlayer(1);

                }
            }
        }

        //process the kill list
        for (int i = 0; i < killList.Count; i++)
        {
            //kill the gameobject
            Destroy(killList[i].gameObject);

            //kill the list entry
            indicators.Remove(killList[i]);

        }




        if (Input.GetKeyDown(KeyCode.A) && CheckLegalInput())
        {
            catcher.transform.Translate(Vector3.left * 120);
            //moveInput = -1;

            currentCatcherLane--;

        }
        else if (Input.GetKeyDown(KeyCode.D) && CheckLegalInput())
        {
            catcher.transform.Translate(Vector3.right * 120);
            //moveInput = 1;
            currentCatcherLane++;

        }
    }



    bool CheckLegalInput()
    {
        if (inputLocked)
        {
            return false;
        }
        //look at the current time and see if its +/- some tolerance value from the next beats time
        //left off here 6/18

        //so first we need to find the next closest beat
        //its either going to be the current one or the next one 
        //find if the distance between the last beat or the distance to the next beat is smaller

        double beatDistance = 0;

        //so we need to iron out some essential info
        //need to know the dsptime of the current beat, and need to know the dsp time of one beat

        if (Mathf.Abs((float)(AudioSettings.dspTime - TimeManager.currentBeatDSPTime)) <= Mathf.Abs((float)(AudioSettings.dspTime - (TimeManager.currentBeatDSPTime + TimeManager.timePerBeat))))
        {
            beatDistance = Mathf.Abs((float)(AudioSettings.dspTime - TimeManager.currentBeatDSPTime));
        }
        else
        {
            beatDistance = Mathf.Abs((float)(AudioSettings.dspTime - (TimeManager.currentBeatDSPTime + TimeManager.timePerBeat)));
        }

        Debug.Log("beat distance was " + beatDistance);

        if (beatDistance < toleranceTime)
        {

            return true;
        }
        else
        {


            //penalize the player and lock input for a certain amount of time
            StartCoroutine(lockInputRoutine());

            return false;

        }
    }

    double lockEndTime;
    IEnumerator lockInputRoutine()
    {
        inputLocked = true;
        lockEndTime = TimeManager.currentBeatDSPTime + inputLockTime;
        catcher.gameObject.GetComponent<Image>().color = Color.red;
        yield return new WaitUntil(() => AudioSettings.dspTime >= lockEndTime);
        catcher.gameObject.GetComponent<Image>().color = Color.white;
        inputLocked = false;
        //

    }

    //so this may not even need to be setup like this but its a good start, the only issue is it's super weird to control cause of the delay
    //we should probably instead just wait for an input then check and see if its legal

    //later penalize you for fucking up
    public void MoveCallback()
    {
        if (moveInput == 1)
        {
            catcher.transform.Translate(Vector3.right * 120);

        }
        else
        {
            catcher.transform.Translate(Vector3.left * 120);
        }
        moveInput = 0;
    }

    public override void Preload(Sample sample)
    {
        base.Preload(sample);
        SpawnIndicators();

    }


    //later indicators should probably become a common thing shared among minigames, seems like alot are going to have some form
    void SpawnIndicators()
    {

        //clear old inciators

        foreach (NIndicator n in indicators)
        {
            Destroy(n.gameObject);
        }

        indicators.Clear();


        //so we're just going to drop some indicators down, they travel to the bottom of their lane
        //spawn like 4 of these randomly over 8 spots

        for (int i = 3; i < 8; i += 2)
        {
            //spawn an indicator in a random lane
            //pick the lane for it


            int lane = Random.Range(0, 4);

            // indicatorPosition.x = lanes[lane].position.x;
            // indicatorPosition.y = (lanes[lane].position.y - lanes[lane].GetComponent<RectTransform>().rect.height / 2) + (i * beatOffset);

            //so we're just gonna assume the lanes have children at 0 and 1 that represent the top and bottom

            //0-top
            //1- bottom

            Vector3 indicatorEndPosition = lanes[lane].GetChild(1).transform.position;

            Vector3 indicatorStartPosition = lanes[lane].GetChild(0).transform.position;

            //so the start position is the end position + the offset based on the beat
            //beat offset is the beat * the units per beat
            //units per beat is the distance between start and end positions y / 8

            float unitsPerBeat = lanes[lane].GetComponent<RectTransform>().rect.height / 8f;
            float beatOffset = i * unitsPerBeat;

            indicatorStartPosition.Set(indicatorEndPosition.x, indicatorEndPosition.y + beatOffset, indicatorEndPosition.z);


            GameObject ind = Instantiate(indicator, indicatorStartPosition, Quaternion.identity, lanes[lane]);

            //TODO: fix this bullshit lol
            ind.GetComponent<NIndicator>().SetIndicatorInfo(indicatorStartPosition, indicatorEndPosition, i, lane);
            //

            //ind.GetComponent<Canvas>().overrideSorting = true;
            indicators.Add(ind.GetComponent<NIndicator>());
        }

    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        SetIndicatorTimes();
    }

    void SetIndicatorTimes()
    {
        foreach (NIndicator n in indicators)
        {
            //TODO: setting to 0 for now tie in later if we keep this class
            n.SetStartTime(TimeManager.battleStartTime);
        }
    }



}
