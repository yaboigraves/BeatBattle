using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControlTrigger : MonoBehaviour
{
    public CameraAction action = new CameraAction();

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            //tell the camera to either pan, zoom, or switch to focus on something else or something

            //query the camera manager for the players orbit camera
            OrbitCamera cam = CameraManager.current.playerOrbitCam;

            if (cam == null)
            {
                Debug.LogWarning("O SHIT NO CAMERA FOUND LOL");
                return;
            }

            if (action.actionType == ActionType.ZOOM)
            {
                cam.Zoom(action.zoomAmount, action.zoomTime, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //tell the camera to either pan, zoom, or switch to focus on something else or something

            //query the camera manager for the players orbit camera
            OrbitCamera cam = CameraManager.current.playerOrbitCam;

            if (cam == null)
            {
                Debug.LogWarning("O SHIT NO CAMERA FOUND LOL");
                return;
            }

            if (action.actionType == ActionType.ZOOM)
            {
                cam.Zoom(action.zoomAmount, action.zoomTime, false);
            }
        }
    }
}


[System.Serializable]
public class CameraAction
{
    public ActionType actionType;

    public bool resetOnExit = false;

    public float zoomAmount = 0;
    public float panAmount = 0;

    public float zoomTime = 1f;

}

public enum ActionType
{
    PAN,
    ZOOM
}