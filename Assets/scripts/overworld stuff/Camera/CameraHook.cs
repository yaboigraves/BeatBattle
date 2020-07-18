using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHook : MonoBehaviour
{
    public CinemachineVirtualCamera thisCamera;
    void Start()
    {
        thisCamera = GetComponent<CinemachineVirtualCamera>();
        CameraManager.current.transferCamera(thisCamera);
    }



}
