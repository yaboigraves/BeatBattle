using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
    general manager for manipulating settings for the battle

*/


public class BattleDebugConsole : MonoBehaviour
{
    public GameObject debugPanel;
    public TMP_Dropdown trackSelector;

    // Start is called before the first frame update
    void Start()
    {
        InitTrackSelector();
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
        // //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        // //so instead of relading the scene (which will fuck with track selection) just going to clear all the indicators and reset vibe and start fresh

        // //stuff to do in this function
        // //-clear all the indicators and bars
        // //-reset the vibe

        // IndicatorManager.current.ClearIndicators();
        // IndicatorManager.current.ClearBars();

        // //after all this shit is done tell the battlemanager to reload the currently selected track and set the battle to not started
        // BattleManager.current.StopBattle();
        // BattleManager.current.setupBattle();
    }

    public void InitTrackSelector()
    {
        //go through all the tracks in the battletrack manager test tracks and create a dropdown option for each of them
        // trackSelector.ClearOptions();

        // foreach (Track t in BattleTrackManager.current.testPlayerTracks)
        // {
        //     trackSelector.options.Add(new TMP_Dropdown.OptionData(t.name));
        // }
    }

    public void SelectTrack(int selection)
    {
        //set the current track to the selected option and then reset the scene
        // BattleTrackManager.current.playerSelectedTrack = BattleTrackManager.current.testPlayerTracks[selection];
        // ReloadScene();
    }


}
