using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//8/28 refactor

/*


so for now, when we load a new track at the end of a turn the loading of the minigame is now more or 
less just 
-loading the minigame scene
-loading the indicator timing data (in beats)
-spawning indicators
-getting ready to get everything moving

*/

/*GOALS
indicator spawning
generalized minigame (we can re-use the one button minigame battle one)
variable length
*/



public class MinigameManager : MonoBehaviour
{
    public static MinigameManager current;


    public bool minigamesLoaded = false;

    //list of all the minigames currently loaded in the battle
    //so this is a dictionary that associates a hash code with a minigame now

    //ok so actually, if we know the minigames then we can load the scene
    public MiniGame[] miniGames;

    public MiniGame activeMiniGame;



    private void Awake()
    {
        current = this;
    }

    public void LoadMinigames(Sample[] playerSamples)
    {
        //so basically now just loop through all the scenes and load their minigame scene
        //best way to do this is stage it in coroutiens that loads each one synchronously so they can register properly
        StartCoroutine(LoadMinigamesRoutine(playerSamples));
    }



    //ok, so the plan then is to basically just use like build index or somethign to instantiate a minigame object
    //then make sure its loaded into the proper scene, and allso the samples get a reference as this happens

    public MiniGame miniGameReferenceLocker = null;


    IEnumerator LoadMinigamesRoutine(Sample[] samples)
    {
        minigameSceneCounter = 0;
        List<string> miniGameSceneNames = new List<string>();
        miniGames = new MiniGame[samples.Length];


        foreach (Sample s in samples)
        {
            miniGameSceneNames.Add(s.miniGameSceneName);
        }

        AsyncOperation[] sceneLoads = new AsyncOperation[miniGameSceneNames.Count];

        for (int i = 0; i < miniGameSceneNames.Count; i++)
        {
            sceneLoads[i] = SceneManager.LoadSceneAsync(miniGameSceneNames[i], LoadSceneMode.Additive);
            //sceneLoads[0].allowSceneActivation = false;

            //so now we can force the minigame to register properly
            //so once a minigame loads it should register with the manager, and we wait to load the next scene till the array of scenes is full


            while (!sceneLoads[i].isDone && miniGames[i] == null)
            {
                yield return null;
            }



            //Debug.Log("ok scene loaded done for scene " + i.ToString());
            //so now we need to wait for the minigame reference to be established
            //the actual minigame scene object will send in the object to be initialized, and then we set it back to null

            //so now that we have the minigame we plug in the sample info etc for it and establish it in the players samples
            //so i guess we just maintain an array of minigames that represents like the order here?
            //probably need to actually setup some kind of cohesive thing in the battlemanager actually
            //so we can just use the playerturn in the minigame to scroll through an array of minigames

            //minigames can just store their scene

            //Debug.Log("ok we got the minigame reference");
            // Debug.Log(miniGames[i].GetInstanceID());

            //so now i guess we can plug in the info from the sample
            //miniGames[i].LoadStuff();


        }

        // while (CheckScenesLoaded(sceneLoads))
        // {
        //     yield return null;
        // }

        Debug.Log("done loading");
        // //so after the scenes are done loading, now we gotta assign them samples

        minigamesLoaded = true;
    }

    bool CheckScenesLoaded(AsyncOperation[] ops)
    {
        foreach (AsyncOperation a in ops)
        {
            if (!a.isDone)
            {
                return false;
            }
        }

        return true;
    }

    int minigameSceneCounter = 0;
    //so when we register a minigame lets see what the scene indexes are
    public void registerMiniGame(MiniGame mg)
    {

        //Debug.Log(minigameScenes.Length);
        //so so we know the hash co
        // loadedMiniGames.Add(miniGame, minigameScene);
        //just find the next null slot
        miniGames[minigameSceneCounter] = mg;
        minigameSceneCounter++;
    }

    public void DeactivateActiveMinigame()
    {
        if (activeMiniGame != null)
        {
            activeMiniGame.miniGameCanvas.gameObject.SetActive(false);
        }
    }

    public void ActivateMinigameScene(string sceneName)
    {
        //turn off the active minigame if it exists
        if (activeMiniGame != null)
        {
            activeMiniGame.miniGameCanvas.gameObject.SetActive(false);
        }

        //look through the minigames loaded and set the one we're looking for active

        //ok so this doesnt actually work bc the scene names can be the same,
        //so we can i guess just use the instance id or something instead?
        MiniGame game = miniGames[BattleManager.current.playerTurn];

        if (game)
        {
            //activate the canvas
            game.miniGameCanvas.gameObject.SetActive(true);
            //Debug.Log("setting the minigame canvas active");

            // Debug.Log(game.GetInstanceID());
            activeMiniGame = game;
            activeMiniGame.StartMiniGame();
            return;
        }
    }

    //lets reurpose this
    //so this gets called where the activate gets called right now
    //basically we give one beat of time for all the indicators to spawn and shit
    public void PreloadMiniGame(Sample playerSample)
    {

        //so what we need here is some way for the minigame to be in the sample or something
        //minigames are objects in the scene that actually load
        //what if we spawn these dynamically instead? any saved stuff can be in the scriptable object
        MiniGame game = miniGames[BattleManager.current.playerTurn];

        //for now we dont even fuckin bother looking at the sample


        game.SetBeatTimes(playerSample.sampleTrack.tracks[0].kickBeats);
        game.Preload(playerSample);

        //TODO: rewrite activateminigamescene to use something unique to the scene or sample

        ActivateMinigameScene(playerSample.miniGameSceneName);

        //so once the scene is active lets fuckin spawn some indicators based on the kick data
        //generally speaking this should probably be handled by the actual minigame itself, so lets make an object for those
    }

    // private MiniGame findMiniGameByName(string minigameName)
    // {
    //     return loadedMiniGames.Find(g => g.miniGameSettings.minigameSample.miniGameSceneName == minigameName);
    // }

    // private MiniGame findMiniGameByScene(Scene gameScene)
    // {
    //     return loadedMiniGames[gameScene];
    // }



    public void ReportHit(bool hit)
    {


        if (hit)
        {
            if (activeMiniGame.report.NotesCorrect == 0 || !BattleUIManager.current.dmgReportText.IsActive())
            {
                //turn on the text
                BattleUIManager.current.ToggleReportText(true);
            }

            activeMiniGame.report.NotesCorrect++;
        }
        else
        {

        }
    }

    public void ResetMinigameScenes()
    {

        foreach (MiniGame g in miniGames)
        {
            SceneManager.UnloadSceneAsync(g.minigameScene);
        }

        //after these are all done yeet the list

        miniGames = null;
    }
}




