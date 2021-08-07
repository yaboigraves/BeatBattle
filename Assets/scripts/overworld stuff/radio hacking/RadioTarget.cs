using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTarget : MonoBehaviour
{
    public HackEffect hackEffect;

    List<Radio> radiosInRange = new List<Radio>();

    public void AddRadio(Radio r, bool adding)
    {
        if (adding)
        {
            radiosInRange.Add(r);
        }
        else
        {
            radiosInRange.Remove(r);
        }
    }


    //returns true if the target needs to get cleared
    public bool Ping(HackEffect pingEffect)
    {





        switch (hackEffect)
        {
            case HackEffect.Explode:
                if (hackEffect != pingEffect) { break; }

                Explodeable splode = GetComponentInChildren<Explodeable>();
                Debug.Assert(splode != null, "NO SPLODER");
                splode.Explode();
                //remove it from the radio's list
                // ClearRadiosInRange();
                return true;

            //so we need to mark the radio for removal after we go through the list

            case HackEffect.Activate:
                if (hackEffect != pingEffect) { break; }

                break;
            case HackEffect.NPCSway:
                if (hackEffect != pingEffect) { break; }

                break;
        }
        return false;
    }

    // public void ClearRadiosInRange()
    // {
    //     //remove from all of the radios in range and clear the list

    //     foreach (Radio r in radiosInRange)
    //     {
    //         r.radioTargets.Remove(this);
    //     }
    //     radiosInRange.Clear();
    // }

    public bool HasRadio(Radio r)
    {
        return radiosInRange.Contains(r);
    }

}
