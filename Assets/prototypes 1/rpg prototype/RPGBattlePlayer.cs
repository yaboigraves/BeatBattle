using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPGBattlePlayer : MonoBehaviour
{
    public GameObject playerTurnPanel;

    public void StartTurn()
    {
        playerTurnPanel.SetActive(true);
    }

    public void EndTurn()
    {
        playerTurnPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
