using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BattleCameraController : MonoBehaviour
{

    public static BattleCameraController current;
    public GameObject playerCam;
    public GameObject enemyCam;
    public CameraTrackSwitcher playerSwitch;
    public CameraTrackSwitcher enemySwitch;


    // Start is called before the first frame update


    void Awake()
    {
        //StartCoroutine(trackSwitch());
        current = this;
    }

    void Start()
    {
        //SceneManage.current.mainCamera.SetActive(false);

        enemyCam.SetActive(false);
        playerCam.SetActive(true);
    }



    public void trackSwitcher(bool lookAtPlayer)
    {
        playerCam.SetActive(lookAtPlayer);
        enemyCam.SetActive(!lookAtPlayer);
    }

    public void CameraSwitchup()
    {
        //20% chance every beat for a switchup?
        // if (Random.Range(0, 100) <= 75)
        // {
        //     //no switch
        //     return;
        // }

        //figure out which camera is active 

        if (enemyCam.activeSelf)
        {
            enemySwitch.NextCamTrack();
        }
        else if (playerCam.activeSelf)
        {
            playerSwitch.NextCamTrack();
        }
    }
}
