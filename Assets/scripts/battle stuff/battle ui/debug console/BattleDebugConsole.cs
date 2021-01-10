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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
