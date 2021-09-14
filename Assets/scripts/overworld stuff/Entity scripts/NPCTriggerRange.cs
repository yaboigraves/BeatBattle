using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTriggerRange : MonoBehaviour
{
    public NPC nPC;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //check to see if this NPC has some greeting text

            //TODO: reimpliment this later
            nPC.NPCGreet();


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //check to see if this npc has some goodbye text
            nPC.NPCGoodbye();
        }
    }

}
