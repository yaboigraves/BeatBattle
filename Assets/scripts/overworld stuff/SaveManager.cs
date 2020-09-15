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

    public void LoadDefaultSettings()
    {
        //go through some of the settings and if they're 0 or null set them to defaults

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

    public void RebindMidi(string midiID)
    {
        StopCoroutine("midiRebind");
        StartCoroutine(midiRebind(midiID));

    }

    public IEnumerator midiRebind(string midiName)
    {
        bool inputMade = false;

        while (!inputMade)
        {
            for (int i = 0; i <= 127; i++)
            {
                if (MidiJack.MidiMaster.GetKeyDown(i))
                {
                    //we got a key to rebind
                    //set that shit in the player prefs
                    PlayerPrefs.SetInt(midiName, i);
                    DebugManager.current.print("rebound " + midiName + " to " + i);

                    inputMade = true;
                    StopCoroutine("midiRebind");
                    break;
                }
            }
            yield return new WaitForEndOfFrame();

        }
    }


}
