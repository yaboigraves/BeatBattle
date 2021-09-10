using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Minigame", menuName = "BeatBattle/Minigame", order = 0)]
[System.Serializable]
public class MiniGameScriptableObject : ScriptableObject
{
    public MiniGameSettings settings;

    //so this is a reference to the minigame object that we're going to spawn in the scene

    //9/10 left off here: so yeah basically this is a weird problem
    //need to make it so that this scriptable object can A) be used to help spawn the actual minigame object in the scene
    //B) helps us map the minigames to the scenes properly despite the scene names basically all being the same sometimes
    
    MiniGame miniGame;

    public string sceneName;
}
