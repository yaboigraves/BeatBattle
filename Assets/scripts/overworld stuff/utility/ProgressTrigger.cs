using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//pretty simple little object that can be used to trigger any function when the player walks into it
//only works if the player hasnt already triggered this progress point
public class ProgressTrigger : MonoBehaviour
{
    public UnityEvent triggerEvents;
    public string progressPointName;
    public bool progressPointNewValue;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            print("Setting proggresss");
            bool triggerEvent = SaveManager.gameStateData.progressData.SetProgressPoint(progressPointName, progressPointNewValue);

            //if triggerEvent is true that means we trigger whatever event needs to be done
            if (triggerEvent)
            {
                triggerEvents.Invoke();
            }

        }
    }

}
