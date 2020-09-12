using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTrack : MonoBehaviour
{
    public Toggle toggle;
    public Track track;
    public void SelectTrack()
    {
        UIManager.current.SelectTrack(track);
    }

    public void EquipTrack(Toggle toggle)
    {
        //so this needs to first check if we got space to equip the track
        //then add the track to the players equipped tracks
        GameManager.current.player.GetComponent<PlayerInventory>().EquipTrack(track, toggle.isOn);
    }

    public void ToggleToggle(bool on)
    {
        toggle.isOn = on;
    }
}
