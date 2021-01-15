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

    //so this probably cant be a bool just in case we pass through multiple checkpoints
    //going to convert this to an int instead

    //if there is a pause scheduled then this is going to be 1, if theres no pause scheduled it will be lower than 1
    //every checkpoint we pass will then increase this by 1

    int checkPoints = 1;



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

    public void SetCutscene(PlayableDirector director)
    {
        this.director = director;
    }

    public void startCutscene(string[] parameters)
    {
        checkPoints = 1;
        director.Play();

    }
    //every checkpoint we pass should decrease the pauseScheduler

    //this is emitted from the cutscene when we need to pause stuff and wait for yarn to progress forward
    public void PauseCutscene()
    {
        //so if we hit a pause we lose a checkpoint 

        checkPoints--;

        //if u got 0 checkpoints left we pause

        if (checkPoints <= 0)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }
    }

    //this is called from yarn and when that happens we can resume the timeline

    public void cutsceneCheckpoint(string[] parameters)
    {

        //if you're at 0 checkpoints we're going to unpause

        if (checkPoints <= 0)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1d);

        }

        //when you hit a checkpoint you get one more
        checkPoints++;
    }

}
