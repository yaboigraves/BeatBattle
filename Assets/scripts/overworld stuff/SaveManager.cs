using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    /*  SETTINGS NAMES
        -musicVolume

    */
    public static SaveManager current;

    private void Awake()
    {
        current = this;
    }



    public void LoadSettings()
    {
        //set the track volume equal to whatever is loaded in memory 
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        TrackManager.current.UpdateTrackVolume(musicVolume);
        //tell the ui to reflect this change
        UIManager.current.volumeSlider.value = musicVolume;

    }

    public void UpdateVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("musicVolume", newVolume);
    }


}
