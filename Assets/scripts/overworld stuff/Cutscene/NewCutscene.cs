using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "New Track", menuName = "Cutscene/Cutscene")]

public class NewCutscene : ScriptableObject
{
    public TimelineAsset cutscene;
    public int cutsceneID;

    public bool isUnique, isBlocking;

    public PlayableDirector director;

    // Start is called before the first frame update



}
