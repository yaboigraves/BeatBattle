using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;
public class DialogCameraController : MonoBehaviour
{
    public static DialogCameraController current;
    //this script is used by the ui manager to handle refocusing the camera in dialog
    public CinemachineVirtualCamera currentCamera;
    //camera is by default the playercamera
    public DialogueRunner dialogueRunner;
    Dictionary<string, CinemachineVirtualCamera> cameraPositions = new Dictionary<string, CinemachineVirtualCamera>();
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        if (dialogueRunner)
        {
            dialogueRunner.AddCommandHandler("testPing", testPing);
            dialogueRunner.AddCommandHandler("changeCamera", changeCamera);
        }
        else
        {

            dialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("testPing", testPing);
            dialogueRunner.AddCommandHandler("changeCamera", changeCamera);
        }

    }
    public void testPing(string[] parameters)
    {
        print("test ping from camera");
    }

    public void setCameraObjects(Dictionary<string, CinemachineVirtualCamera> newCameraPositions)
    {
        print("camera objects set");

        //print(newCameraPositions);

        // cameraPositions = newCameraPositions;

        // print(cameraPositions);

        //print(cameraPositions.Count);

        //TODO: optimize this so on a scene change we should truncate all the old positions

        foreach (KeyValuePair<string, CinemachineVirtualCamera> cams in newCameraPositions)
        {
            cameraPositions.Add(cams.Key, cams.Value);
        }
    }

    //do this on a scene load
    public void ClearDialogCameraPositions()
    {
        //cameraPositions.Clear();
    }


    //TODO: this still breaks sometimes so this needs to be further tested, it works for now
    //for some reason the camera positions get truncated so we need to check if its actually there
    public void changeCamera(string[] paramaters)
    {
        print("changing camera to camera :" + paramaters[0]);
        //first argument is the camera to switch to
        //check if their are any custom positions 
        foreach (KeyValuePair<string, CinemachineVirtualCamera> cameraPos in cameraPositions)
        {
            print(cameraPos.Key);
        }

        print(cameraPositions.Count);

        if (cameraPositions != null && cameraPositions.ContainsKey(paramaters[0]))
        {
            if (currentCamera == null)
            {
                //lower the priortiy 
                CameraManager.current.currentCamera.Priority = 0;

                currentCamera = cameraPositions[paramaters[0]];
                currentCamera.Priority = 15;
            }
            else
            {
                //set the currentcameras priority to 0 
                currentCamera.Priority = 0;
                currentCamera = cameraPositions[paramaters[0]];
                currentCamera.Priority = 15;
            }
        }
        else
        {
            print("no camera positions set or that camera doesnt exist");
        }

    }

    //call this shit when you're leaving dialog
    public void resetCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 0;
        }
    }




}
