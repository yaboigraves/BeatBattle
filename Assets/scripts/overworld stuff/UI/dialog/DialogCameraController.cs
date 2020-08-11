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
    Dictionary<string, CinemachineVirtualCamera> cameraPositions;
    private void Awake()
    {
        current = this;
        dialogueRunner.AddCommandHandler("testPing", testPing);
        dialogueRunner.AddCommandHandler("changeCamera", changeCamera);

    }
    public void testPing(string[] parameters)
    {
        print("test ping from camera");
    }

    public void setCameraObjects(Dictionary<string, CinemachineVirtualCamera> newCameraPositions)
    {
        cameraPositions = newCameraPositions;
    }

    public void changeCamera(string[] paramaters)
    {
        print("changing camera to camera :" + paramaters[0]);
        //first argument is the camera to switch to

        //check if their are any custom positions 
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

    public void InitDialogCamera(Dialogue currentDialogue)
    {

        if (currentDialogue.sentenceCameras.ContainsKey(0))
        {
            checkCameraUpdate(currentDialogue, 0);
        }

        else
        {
            //set the current camera to the player camera 
            currentCamera = CameraManager.current.currentCamera;
            currentCamera.Priority = 15;
        }
        //check if the camera needs to move otherwise set the currentCamera to the player camera 

    }

    public void StopDialogCamera()
    {
        //TODO: Reimpliment with yarn
        //currentCamera.Priority = 9;
        //currentCamera = null;
        //CameraManager.current.currentCamera.Priority = 15;
    }

    //takes in a sentence index and checks if theirs a camera switchup there
    public void checkCameraUpdate(Dialogue currentDialogue, int sentenceIndex)
    {

        //TODO: reimplement using yarn
        //if this index needs a switchup
        // if (currentDialogue.sentenceCameras.ContainsKey(sentenceIndex))
        // {
        //     //turn down the priority of the previous camera if it exists
        //     if (currentCamera != null)
        //     {
        //         currentCamera.Priority = 9;
        //     }

        //     //turn up the priority of the new camera
        //     currentCamera = currentDialogue.sentenceCameras[sentenceIndex];
        //     currentCamera.Priority = 15;
        // }
    }
}
