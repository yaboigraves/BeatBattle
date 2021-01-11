using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
    general manager for manipulating settings for the battle

*/


public class BattleDebugConsole : MonoBehaviour
{
    public GameObject debugPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLanePosition(string inp)
    {

    }

    public void ActivateConsole()
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
    }

    public void ReloadScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //so instead of relading the scene (which will fuck with track selection) just going to clear all the indicators and reset vibe and start fresh

        //stuff to do in this function
        //-clear all the indicators and bars
        //-reset the vibe

        IndicatorManager.current.ClearIndicators();
        IndicatorManager.current.ClearBars();

        BattleManager.current.StopBattle();

        BattleManager.current.setupBattle();


        //after all this shit is done tell the battlemanager to reload the currently selected track and set the battle to not started


    }
}
