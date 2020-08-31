using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public float bpm;
    public float moveSpeed;
    bool activated;

    void Start()
    {
        bpm = BattleTrackManager.current.currentBpm;
        moveSpeed = bpm / 60;
    }

    // Update is called once per frame
    void Update()
    {
        activated = BattleManager.current.battleStarted;
        //need to move based on the bpm?
        //if bpm is 96 then one beat passes every 96/60
        if (activated)
        {
            bpm = BattleTrackManager.current.currentBpm;
            transform.Translate(0, Time.deltaTime * -moveSpeed, 0);
        }

    }


}
