using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{

    public GameObject selectedIcon;

    [SerializeField]
    int currentSong;
    //raise the image over the radio

    List<RadioTarget> radioTargets = new List<RadioTarget>();


    public void Select(bool toggle)
    {
        selectedIcon.SetActive(toggle);
    }

    public void SetSong(int song)
    {
        currentSong = song;
        Debug.Log("radio got message");

        //do a ping
        PingArea();
    }

    public void PingArea()
    {
        //notify all the things in range of the radio,

        //trigger collider tracks all the things in range

        //things in range are defined as RadioTargets and will recieve the ping and do something

        foreach (RadioTarget t in radioTargets)
        {
            t.Ping();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        RadioTarget t = other.gameObject.GetComponent<RadioTarget>();
        if (t != null)
        {
            radioTargets.Add(t);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        RadioTarget t = other.gameObject.GetComponent<RadioTarget>();
        if (t != null)
        {
            radioTargets.Remove(t);
        }
    }
}
