using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public float bpm;
    public float moveSpeed;

    bool activated;

    void Awake()
    {
        bpm = TrackManager.current.currentBpm;
    }

    // Start is called before the first frame update
    void Start()
    {
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
            transform.Translate(0, Time.deltaTime * -moveSpeed, 0);
        }

    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "pad")
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
