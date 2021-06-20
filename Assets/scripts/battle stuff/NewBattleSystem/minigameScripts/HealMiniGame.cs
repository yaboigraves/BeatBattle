using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float toleranceTime = 0.4f;

    public GameObject indicator;

    public Transform[] lanes;

    List<NIndicator> indicators;


    float beatOffset;
    private void Start()
    {
        base.LoadStuff();
        //TimeManager.beatCallbacks.Add(MoveCallback);
        beatOffset = lanes[0].GetComponent<RectTransform>().rect.height / 8;
    }

    private void Update()
    {
        if (state == MiniGameState.Inactive)
        {
            return;
        }

        //update the indicators

        foreach (NIndicator n in indicators)
        {
            n.UpdateIndicator();
        }


        if (Input.GetKeyDown(KeyCode.A) && CheckLegalInput())
        {
            catcher.transform.Translate(Vector3.left * 120);
            //moveInput = -1;

        }
        else if (Input.GetKeyDown(KeyCode.D) && CheckLegalInput())
        {
            catcher.transform.Translate(Vector3.right * 120);
            //moveInput = 1;

        }
    }

    bool CheckLegalInput()
    {
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

        //Debug.Log("beat distance was " + beatDistance);

        if (beatDistance < toleranceTime)
        {

            return true;
        }

        return false;






    }


    //so this may not even need to be setup like this but its a good start, the only issue is it's super weird to control cause of the delay
    //we should probably instead just wait for an input then check and see if its legal

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

    public override void Preload()
    {
        base.Preload();
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

        for (int i = 1; i < 8; i += 2)
        {
            //spawn an indicator in a random lane
            //pick the lane for it

            Vector3 indicatorPosition = Vector3.zero;
            int lane = Random.Range(0, 4);

            indicatorPosition.x = lanes[lane].position.x;
            indicatorPosition.y = (lanes[lane].position.y - lanes[lane].GetComponent<RectTransform>().rect.height / 2) + (i * beatOffset);




            GameObject ind = Instantiate(indicator, indicatorPosition, Quaternion.identity, lanes[lane]);

            //TODO: fix this bullshit lol
            ind.GetComponent<NIndicator>().SetIndicatorInfo(indicatorPosition, indicatorPosition - Vector3.down * 400, i);



            indicators.Add(ind.GetComponent<NIndicator>());
        }

    }



}
