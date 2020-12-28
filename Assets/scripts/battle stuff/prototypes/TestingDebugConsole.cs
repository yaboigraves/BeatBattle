using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TestingDebugConsole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePanel(){
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void ResetScene(){
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        CircularBattleManager.current.ResetBattle();
    }

    public void SelectTrack(int val){
        print(val);
        CircularBattleManager.current.UpdateTrackTest(val);
    }
}
