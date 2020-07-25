using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineVirtualCamera myCamera;
    Transform playerTransform;

    void Start()
    {
        myCamera = GetComponent<CinemachineVirtualCamera>();

        //TODO: game manager tells the camera manager this
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        CameraManager.current.transferCamera(myCamera);

        if (playerTransform != null)
        {
            CameraManager.current.setCameraFollow(playerTransform);
        }
        else
        {
            print("player transform not found by camera");
        }
    }
}
