using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    so this also needs to check if the cutscene with the given id/name has been already triggered
    we look into the playerprogress data and just send it this cutscenes id/name and then check if it's been triggered

    probably dont want to have to manually create a variable for all these cutscenes though, so they can be saved 
    seperately in a dynamic list 
    save manager will hold a collection of all the cutscenes that have been triggered, which starts empty 
    as cutscenes trigger they get added to the list that way not every cutscene needs a manual save

*/


public class CutsceneTrigger : MonoBehaviour
{

    //check if we should even load our collider, if this cutscene has been run before dont load it



    public Cutscene cutscene;

    private void Start()
    {


        StartCoroutine(LateLateUpdate());
    }

    //runs after the second frame update
    IEnumerator LateLateUpdate()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (SaveManager.checkIfCutsceneRan(cutscene.cutsceneID))
        {
            //turn off the collider

            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //TODO: rewrite for new custscene manager


            //run the current cutscene if we're nto already in one 
            // if (CutsceneManager.current.inCutscene == false)
            // {
            //     // CutsceneManager.current.LoadCutscene(cutscene);
            //     // CutsceneManager.current.StartCutscene();

            //     //disable the collider so we cant possibly 
            //     //GetComponent<Collider>().enabled = false;
            // }
        }
    }

}
