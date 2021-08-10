using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
// using UnityEditor.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    //so we do need one of these in every scene now
    GameObject player;
    public Mesh sSymbol;
    public Material symbolMat;
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


    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white; ;
    //     // Gizmos.DrawSphere(transform.position, 0.25f);
    //     Gizmos.DrawMesh(sSymbol, -1, transform.position, Quaternion.Euler(90, (float)EditorApplication.timeSinceStartup * 450, 0));
    // }

}
