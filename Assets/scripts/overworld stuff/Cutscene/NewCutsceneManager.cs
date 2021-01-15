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
        director.Play();
    }

    //this is emitted from the cutscene when we need to pause stuff and wait for yarn to progress forward
    public void PauseCutscene()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0d);
    }

    //this is called from yarn and when that happens we can resume the timeline

    /*
    TODO: if the user spams then we need to create some kind of queue, like if the cutscene hasn't finished
    yet and the player moves to the next cutscene it will pause before the trigger is reached.
    -so if the cutscene hasnt even finished and the users blowing through the dialogue we need to ignore the checkpoint?

    */
    public void cutsceneCheckpoint(string[] parameters)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1d);
    }



}
