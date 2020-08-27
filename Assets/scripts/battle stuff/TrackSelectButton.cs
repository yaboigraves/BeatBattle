using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSelectButton : MonoBehaviour
{
    public Track track;
    public void SelectTrack()
    {
        BattleUIManager.current.SetPlayerTrack(track);
    }

}
