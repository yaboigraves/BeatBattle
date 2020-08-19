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
    public Player player;

    InputHandler input;

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

            if (player == null)
            {
                //IMPORTANT: this needs to be done because the player object referenced in the inputhandler needs
                //to actually be the INSTANCE of the prefab object NOT the prefab itself
                //all this does is replace the referenced prefab with this particular INSTANCE of the prefab
                playerObj = Instantiate(playerObj);
            }
            input = GetComponent<InputHandler>();
        }
    }

    private void Start()
    {
        //  moved this to awake bc a bunch of stuffs start needs the player instantiated

        // if (player == null)
        // {
        //     //IMPORTANT: this needs to be done because the player object referenced in the inputhandler needs
        //     //to actually be the INSTANCE of the prefab object NOT the prefab itself
        //     //all this does is replace the referenced prefab with this particular INSTANCE of the prefab
        //     playerObj = Instantiate(playerObj);
        // }
        // input = GetComponent<InputHandler>();
    }

    public void setManagerReferences(Player player)
    {
        this.player = player;
        UIManager.current.player = player;
        SceneManage.current.player = player;
        CameraManager.current.setCameraFollow(player.transform);
    }
}
