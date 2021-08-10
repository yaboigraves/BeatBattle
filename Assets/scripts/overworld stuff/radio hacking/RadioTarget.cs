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
        Debug.Log(pingEffect.ToString());


        switch (pingEffect)
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
                HackActivate activator = GetComponent<HackActivate>();

                if (activator != null)
                {
                    GetComponent<HackActivate>().Activate();
                }

                if (hackEffect != pingEffect) { break; }

                break;
            case HackEffect.NPCSway:
                NPC npc = GetComponent<NPC>();
                //Debug.Assert(npc != null, "NO NPC FOUND!!!");
                if (npc != null)
                {
                    npc.Sway();
                }



                if (hackEffect != pingEffect) { break; }

                break;
        }
        return false;
    }

    public bool HasRadio(Radio r)
    {
        return radiosInRange.Contains(r);
    }

}
