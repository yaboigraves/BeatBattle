using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this guys job is to handle loading, deloading, and managing the seperate minigame scenes
//we know basically all the minigame scenes that are going to be needed at the start of a battle, so just load them all
//start with them set inactive, and then set them active when needed
//only need to deload them at the end of a battle, we can just recycle these and reinit them when we have to

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager current;
    public string[] minigameSceneNames;

    private void Awake()
    {
        current = this;
    }


    //TODO: this needs to be done in a coroutine otherwise it wont work properly with async operations
    public IEnumerator LoadMinigames()
    {
        //so this should look through all the players gear and the enemys gear and figure out what to load
        //basically we're just gonna make a queue of minigames based on whatever the player has organized
        //playerGame1 -> enemyGame -> playerGame2 -> enemyGame -> playerGame3 -> enemyGame -> playerGame1 -> .....


        //the order of these minigames can be changed dynamically, so if the player swaps stuff around this needs to be able to be changed hot

        AsyncOperation[] sceneLoads = new AsyncOperation[minigameSceneNames.Length];

        foreach (string sceneName in minigameSceneNames)
        {
            sceneLoads[0] = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            sceneLoads[0].allowSceneActivation = false;
        }


    }
}
