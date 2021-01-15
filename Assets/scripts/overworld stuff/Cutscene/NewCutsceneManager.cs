using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Yarn.Unity;
using UnityEngine.Playables;

public class NewCutsceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static NewCutsceneManager current;
    public DialogueRunner dialogueRunner;

    //assuming that if you have a cutscene in a dialogue there's a pause somewhere
    public bool pauseScheduled = true;



    public PlayableDirector director;
    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        dialogueRunner.AddCommandHandler("startCutscene", startCutscene);
        dialogueRunner.AddCommandHandler("cutsceneCheckpoint", cutsceneCheckpoint);



    }

    public void startCutscene(string[] parameters)
    {
        pauseScheduled = true;
        director.Play();

    }

    //this is emitted from the cutscene when we need to pause stuff and wait for yarn to progress forward
    public void PauseCutscene()
    {
        //so we're only gonna pause if their is still a pause scheduled

        if (pauseScheduled)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            pauseScheduled = false;
        }
        else
        {
            //if theres no pause scheduled then we re assume theres gonna be a pause
            pauseScheduled = true;
        }
    }

    //this is called from yarn and when that happens we can resume the timeline

    /*
    TODO: if the user spams then we need to create some kind of queue, like if the cutscene hasn't finished
    yet and the player moves to the next cutscene it will pause before the trigger is reached.
    -so if the cutscene hasnt even finished and the users blowing through the dialogue we need to ignore the checkpoint?

    */
    public void cutsceneCheckpoint(string[] parameters)
    {
        //so if their is a pause scheduled and we get here then we're just gonna say no more pause
        if (pauseScheduled)
        {
            pauseScheduled = false;
        }
        else
        {
            //if theres no pause scheduled and we get to a checkpoint i think we assume theirs gonna be a pause
            pauseScheduled = true;
            director.playableGraph.GetRootPlayable(0).SetSpeed(1d);

        }

        //if theres no pause scheduled and we get here

    }



}
