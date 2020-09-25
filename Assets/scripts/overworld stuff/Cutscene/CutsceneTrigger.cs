using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Cutscene cutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //run the current cutscene if we're nto already in one 
            if (CutsceneManager.current.inCutscene == false)
            {
                CutsceneManager.current.LoadCutscene(cutscene);
                CutsceneManager.current.StartCutscene();
            }
        }
    }

}
