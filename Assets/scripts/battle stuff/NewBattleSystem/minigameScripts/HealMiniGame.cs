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

    public float toleranceTime = 0.3f;


    private void Start()
    {
        base.LoadStuff();
        //TimeManager.beatCallbacks.Add(MoveCallback);
    }

    private void Update()
    {
        if (state == MiniGameState.Inactive)
        {
            return;
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

        Debug.Log("beat distance was " + beatDistance);

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



}
