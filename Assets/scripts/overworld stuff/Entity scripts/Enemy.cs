using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int health, maxHealth;
    //public Track[] battleTracks;
    Collider col;
    public string enemyName;

    //public string[] battleTrackJsons;

    public Track[] battleTracks;




    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        foreach (Track track in battleTracks)
        {
            //print(Array.ConvertAll(track.kickBeats.indicatorData.Split(','), float.Parse));
            track.kickBeats.indicatorPositions = Array.ConvertAll(track.kickBeats.indicatorData.Split(' '), float.Parse);
            track.snareBeats.indicatorPositions = Array.ConvertAll(track.snareBeats.indicatorData.Split(' '), float.Parse);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //TODO: remember to re enable this if the player flees fighting
            //make sure we're not already in a battle
            if (!SceneManage.current.inBattle)
            {
                col.enabled = false;
                SceneManage.current.TransitionToBattle(this.gameObject, battleTracks[UnityEngine.Random.Range(0, battleTracks.Length)]);
            }
        }
    }
}
