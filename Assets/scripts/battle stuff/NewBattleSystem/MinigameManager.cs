using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this guys job is to handle loading, deloading, and managing the seperate minigame scenes
//we know basically all the minigame scenes that are going to be needed at the start of a battle, so just load them all
//start with them set inactive, and then set them active when needed
//only need to deload them at the end of a battle, we can just recycle these and reinit them when we have to


//so minigames are just a ui loaded in from another scene
//basically we need some way of presenting each minigame once its their time to come on
//need some way of transitioning between these scenes with some kind of preview so you know what minigame is coming next

//once a minigame loads it shoudl register with the manager
//each minigame is basically just gonna need its own kind of custom script to manage it anyways, so make a bass minigame class





public class MinigameManager : MonoBehaviour
{
    public static MinigameManager current;
    public List<string> miniGameSceneNames;

    public bool minigamesLoaded = false;

    //list of all the minigames currently loaded in the battle
    public List<MiniGame> loadedMiniGames;

    private void Awake()
    {
        current = this;
    }


    //TODO: this needs to be done in a coroutine otherwise it wont work properly with async operations
    public IEnumerator LoadMinigames()
    {
        foreach (Sample s in NBattleManager.current.playerSamples)
        {
            miniGameSceneNames.Add(s.miniGameSceneName);
        }

        AsyncOperation[] sceneLoads = new AsyncOperation[miniGameSceneNames.Count];

        foreach (string sceneName in miniGameSceneNames)
        {
            sceneLoads[0] = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //sceneLoads[0].allowSceneActivation = false;
        }

        while (CheckScenesLoaded(sceneLoads))
        {
            yield return null;
        }

        //Debug.Log("done loading");
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

    public void registerMinigame(MiniGame miniGame)
    {
        loadedMiniGames.Add(miniGame);
        //we maybe need to pass some info to the minigame too
    }


    public void ActivateMinigame(string sceneName)
    {
        //look through the minigames loaded and set the one we're looking for active

        foreach (MiniGame game in loadedMiniGames)
        {

            if (game.miniGameSettings.minigameSample != null && game.miniGameSettings.minigameSample.miniGameSceneName == sceneName)
            {
                //activate the canvas

                game.miniGameCanvas.gameObject.SetActive(true);
                //Debug.Log("setting the minigame canvas active");
                return;

            }
        }
    }


}




