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
    public List<string> miniGameSceneNames;

    public bool minigamesLoaded = false;

    //list of all the minigames currently loaded in the battle
    public List<MiniGame> loadedMiniGames;

    public MiniGame activeMiniGame;


    private void Awake()
    {
        current = this;
    }

    //TODO: this needs to be done in a coroutine otherwise it wont work properly with async operations
    public IEnumerator LoadMinigames()
    {
        foreach (Sample s in BattleManager.current.playerSamples)
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
        //so after the scenes are done loading, now we gotta assign them samples

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

    //so when we register a minigame lets see what the scene indexes are
    public void registerMinigame(MiniGame miniGame, int buildIndex)
    {

        //so so we know the hash co
        loadedMiniGames.Add(miniGame);
        Debug.Log(miniGame.GetHashCode());
        //so when we register the video game we should also
        //miniGame.miniGameSettings.minigameSample.miniGameSceneName
        //we maybe need to pass some info to the minigame too
        Debug.Log(buildIndex);
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
        MiniGame game = findMiniGameByName(sceneName);

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

        MiniGame game = findMiniGameByName(playerSample.miniGameSceneName);
        game.SetBeatTimes(playerSample.sampleTrack.randomTrackData.kickBeats);
        game.Preload(playerSample);

        //TODO: rewrite activateminigamescene to use something unique to the scene or sample

        ActivateMinigameScene(playerSample.miniGameSceneName);

        //so once the scene is active lets fuckin spawn some indicators based on the kick data
        //generally speaking this should probably be handled by the actual minigame itself, so lets make an object for those
    }

    private MiniGame findMiniGameByName(string minigameName)
    {
        return loadedMiniGames.Find(g => g.miniGameSettings.minigameSample.miniGameSceneName == minigameName);
    }

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
}




