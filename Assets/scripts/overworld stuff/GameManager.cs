using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    //todo: manage spawning of managers
    //the managers should not be tied to a particular scene, and if they are tie them to this one object
    //game manager will spawn all the other managers
    public GameObject playerObj;
    public static GameManager current;

    Player player;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            current = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            Instantiate(playerObj);
        }
    }

    public void setManagerReferences(Player player)
    {
        this.player = player;
        UIManager.current.player = player;
        SceneManage.current.player = player;
        CameraManager.current.setCameraFollow(player.transform);
    }
}
