using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{

    public GameObject selectedIcon;

    [SerializeField]
    int currentSong;
    //raise the image over the radio

    public List<RadioTarget> radioTargets = new List<RadioTarget>();


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
        List<RadioTarget> removalList = new List<RadioTarget>();
        foreach (RadioTarget t in radioTargets)
        {
            if (t.Ping())
            {
                removalList.Add(t);
            }
        }

        foreach (RadioTarget t in removalList)
        {
            //remove it from the list
            radioTargets.Remove(t);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        RadioTarget t = other.gameObject.GetComponent<RadioTarget>();
        if (t != null)
        {
            AddTarget(t, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        RadioTarget t = other.gameObject.GetComponent<RadioTarget>();
        if (t != null)
        {
            AddTarget(t, false);
        }
    }

    public void AddTarget(RadioTarget target, bool adding)
    {
        if (adding)
        {
            radioTargets.Add(target);
            target.AddRadio(this, true);
        }
        else
        {
            radioTargets.Remove(target);
            if (target.HasRadio(this))
            {
                target.AddRadio(this, false);
            }

        }
    }
}
