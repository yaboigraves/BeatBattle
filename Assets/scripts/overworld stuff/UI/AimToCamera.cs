using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CameraManager.current.currentCamera.transform.position, -Vector3.up);
    }
}
