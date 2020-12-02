using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCatcher : MonoBehaviour
{
    // Start is called before the first frame update

    Transform playerSpawnPos;
    private void Start()
    {
        playerSpawnPos = GameObject.FindGameObjectWithTag("playerSpawn").transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //just chuck the player to their spawn for now, probably want to penalize the player somehow 
            //also make a ui fade

            other.gameObject.transform.position = playerSpawnPos.position;
        }
    }

}
