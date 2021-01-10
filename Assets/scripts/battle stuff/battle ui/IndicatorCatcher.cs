using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCatcher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            //so we're just going to look at the information from the indicator rather than trying to do some bs with 
            //player turn and stuff

            Indicator indic = other.gameObject.GetComponent<Indicator>();

            // //so first we check if the indicator has a type 

            // switch (indic.indicatorType)
            // {
            //     //TODO: iterate on this
            //     // case "Heady":
            //     //     //if its heady the catcher getting it is actually good
            //     //     BattleManager.current.processPadHit(true);
            //     //     break;

            //     default:
            //         //if its not any type then we just look at whether or not it's an attack or defense note

            //         if (indic.attackOrDefend)
            //         {
            //             //attack note, currently no damage done to the player
            //         }
            //         else
            //         {
            //             //defense note, take damage
            //             BattleManager.current.playerTakeDamage(1);
            //         }

            //         break;
            // }

            BattleManager.current.vibe--;
            BattleUIManager.current.UpdateVibe(BattleManager.current.vibe);

            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "bar")
        {
            Destroy(other.gameObject);
        }

    }
}
