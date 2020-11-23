using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }


    public void LoadGame()
    {
        //for now we just load the desert scene
        SceneManager.LoadScene("desertBase", LoadSceneMode.Single);

        //later we need to look into the scene manager and try and pull where the player was when they closed the game
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
