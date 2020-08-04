using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BattleSelectorManager : MonoBehaviour
{
    //this guy just manages the selection screen for picking battles for practice/testing
    //may be absorbed by the battle manager at some point?
    //not sure how they will tie togeteher

    //for now just make this work by itself then worry about tying it all together
    public static BattleSelectorManager current;

    private void Awake() {
        current = this;
    }

    private void Start() {
        string path = @Application.dataPath + "/audio/trackJsons/cunty.json";
        StreamReader reader = new StreamReader(path);
        string trackJson = reader.ReadToEnd();
        reader.Close();
        
        TrackJson testTrack = JsonUtility.FromJson<TrackJson>(trackJson);
        //then we need to load into the battle

        //couple ways to do this
        //refactor the trackmanager
        //create a dontdestroy object that just gets sent to the next scene and picked up by the battle manager
        //-try the dontdestroy object cause then the trackmanager doesnt have as much work to do managing shit and the battlemanger can just pick it p
        

    }



}
