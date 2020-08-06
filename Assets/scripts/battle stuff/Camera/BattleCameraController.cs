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
    void Start()
    {
        //SceneManage.current.mainCamera.SetActive(false);

        enemyCam.SetActive(false);
        playerCam.SetActive(true);
    }

    void Awake()
    {
        //StartCoroutine(trackSwitch());
        current = this;
    }

    // Update is called once per frame
    IEnumerator trackSwitch()
    {
        yield return new WaitForSeconds(Random.Range(6, 12));
        if (playerCam.activeSelf == true)
        {
            playerCam.SetActive(false);
            enemyCam.SetActive(true);
        }

        else
        {
            enemyCam.SetActive(true);
            playerCam.SetActive(false);
        }

        StartCoroutine(trackSwitch());
    }

    public void trackSwitcher()
    {
        //LEFT OFF HERE 
        //TODO: FIGURE OUT WHY THE FUCK TRACK SWITCHING ISNT ACTUALLY SETTING CAMERA TO DIFFERENT SETTING
        if (playerCam.activeSelf == true)
        {

            //print("looking at enemy now");
            playerCam.SetActive(false);
            enemyCam.SetActive(true);
        }

        else
        {
            //print("looking at player now");
            enemyCam.SetActive(true);
            playerCam.SetActive(false);
        }
    }
}
