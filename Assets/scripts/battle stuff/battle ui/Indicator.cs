using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //so this manages a chunk of indicators not just one indicator now
    public float bpm;
    public float moveSpeed;

    public Transform bars, indicators;
    bool activated;

    void Start()
    {
        bpm = BattleTrackManager.current.currentBpm;

        //commented this to experiment with timescale rather than calculating movespeed 
        //moveSpeed = bpm / 60;


        //uniform move speed of 60bpm 
        moveSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        activated = BattleManager.current.battleStarted;
        //need to move based on the bpm?
        //if bpm is 96 then one beat passes every 96/60
        if (activated)
        {
            //bpm = BattleTrackManager.current.currentBpm;
            transform.Translate(0, Time.deltaTime * -moveSpeed, 0);
        }
    }

    //this will go through all the indicator children and update their color 
    public void UpdateColor(Color color)
    {
        foreach (SpriteRenderer sprite in indicators.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = color;
        }
    }
}
