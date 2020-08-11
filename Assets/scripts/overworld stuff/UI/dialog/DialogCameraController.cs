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

    private void Awake()
    {
        current = this;
        dialogueRunner.AddCommandHandler("testPing", testPing);

    }
    public void testPing(string[] parameters)
    {
        print("test ping from camera");
    }

    public void InitDialogCamera(Dialogue currentDialogue)
    {


        //if there is a camera change for the first index change it
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
