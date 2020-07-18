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

    private void Awake()
    {
        current = this;
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


}
