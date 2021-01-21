using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


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

    public NewCutscene cutscene;
    PlayableDirector director;

    Collider collider;
    private void Start()
    {
        StartCoroutine(LateLateUpdate());
        director = GetComponent<PlayableDirector>();

        cutscene.director = director;

        collider = GetComponent<Collider>();
    }

    //runs after the second frame update
    IEnumerator LateLateUpdate()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //TODO: rewrite cutscene objects that shoudln't repeat as scriptable objects with timelines?
        if (SaveManager.checkIfCutsceneRan(cutscene.cutsceneID))
        {
            // turn off the collider

            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (CutsceneManager.current.inCutscene == false)
            {
                //if we're not in a cutscene play the cutscene

                //this means we pass the cutscene to the cutscene manager and tell it to play
                //NOTE: this is for now only going to work with blocking cutscenes 
                //CutsceneManager.current.SetCutscene(cutscene);
                // CutsceneManager.current.SetBlockingCutscene(cutscene.cutscene);

                //so we need to pass the director attached to this object to the manager
                // CutsceneManager.current.SetCutscene(director);
                // CutsceneManager.current.startCutscene();
                CutsceneManager.current.PlayCutscene(cutscene);

                if (cutscene.isUnique)
                {
                    //turn off the collider
                    this.collider.enabled = false;
                }


                //if the cutscene triggers then we're going to mark that in the save manager



            }

        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        }
    }

}
