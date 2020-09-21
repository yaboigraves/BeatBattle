using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameStateData
{
    public PlayerData playerData;

    public GameStateData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

}

[System.Serializable]
public class PlayerData
{
    public string playerName = "Yancey";
    public int skrillaCount, playerHealth, playerMaxHealth;

    //array of all the equipped tracks
    public string[] tracks = new string[0];
    public string[] gear = new string[0];

}