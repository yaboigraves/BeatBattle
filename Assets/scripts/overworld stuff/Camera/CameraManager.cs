using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager current;
    public Camera mainCamera;
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera currentCamera;

    public CinemachineVirtualCamera itemPickupCam;


    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        if (currentCamera == null)
        {
            currentCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        }
    }

    public void setCameraFollow(Transform followThis)
    {
        currentCamera.Follow = followThis;
        // current.m_Follow = playerTransform;
    }

    public void transferCamera(CinemachineVirtualCamera camera)
    {
        currentCamera = camera;
        setCameraFollow(GameManager.current.playerObj.transform);
    }

    public void enablePickupItemCamera(bool enable)
    {
        if (itemPickupCam == null)
        {
            itemPickupCam = GameManager.current.player.pickupItemCam;
        }

        if (enable)
        {
            itemPickupCam.Priority = 20;
        }
        else
        {
            itemPickupCam.Priority = 0;
        }


    }



}
