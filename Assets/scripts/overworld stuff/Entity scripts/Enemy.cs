using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                SceneManage.current.TransitionToBattle(this.gameObject, battleTracks[Random.Range(0, battleTracks.Length)]);
            }
        }
    }
}
