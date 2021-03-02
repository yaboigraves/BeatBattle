using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FMODUnity;
using System;

public class PlaylistDemo : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string[] fmodPaths;
    // Start is called before the first frame update

    private FMOD.Studio.EventInstance instance;


    void Start()
    {
        StartCoroutine(PlayMusic(fmodPaths));
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    IEnumerator PlayMusic(string[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(a[i]);

            if (PlaybackState(instance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                FMOD.Studio.EventDescription eD;
                instance.getDescription(out eD);

                int userPropertyCount;
                eD.getUserPropertyCount(out userPropertyCount);

                FMOD.Studio.USER_PROPERTY[] userProperties = new FMOD.Studio.USER_PROPERTY[userPropertyCount];

                for (int j = 0; j < userPropertyCount; j++)
                {
                    eD.getUserPropertyByIndex(j, out userProperties[j]);
                }


                instance.start();
                instance.release();

                while (PlaybackState(instance) != FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    yield return null;



                }
            }
        }
    }
}