using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health, maxHealth;
    public Track[] battleTracks;
    Collider col;
    public string enemyName;

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
            col.enabled = false;
            SceneManage.current.TransitionToBattle(this.gameObject, battleTracks[Random.Range(0, battleTracks.Length)]);

        }
    }
}
