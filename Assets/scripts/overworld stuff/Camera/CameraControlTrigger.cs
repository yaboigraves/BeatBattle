using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControlTrigger : MonoBehaviour
{
    public CameraAction action = new CameraAction();
    BoxCollider collider;

    private void OnValidate()
    {
        collider = GetComponent<BoxCollider>();

    }

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

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
            else if (action.actionType == ActionType.PAN)
            {
                cam.CamPan(action.zoomAmount, action.zoomTime);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, collider.size);
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