using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    //so we do need one of these in every scene now
    GameObject player;
    //:)
    private void Start()
    {
        //check and see if theres a different spawn position set via the scene manager
        player = GameObject.FindGameObjectWithTag("Player");
        if (SceneManage.current.sceneSpawnPositions.ContainsKey(SceneManage.current.currentSceneName))
        {
            player.transform.position = SceneManage.current.sceneSpawnPositions[SceneManage.current.currentSceneName];
        }
        else
        {
            player.transform.position = transform.position;
        }
    }


}
