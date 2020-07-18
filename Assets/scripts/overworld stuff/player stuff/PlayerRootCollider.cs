using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootCollider : MonoBehaviour
{
    //used to check if we can jump
    public bool onGround;

    //numgrounds we're touching
    int numGrounds;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            numGrounds++;
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if (numGrounds > 0)
            {
                numGrounds--;
            }

            if (numGrounds <= 0)
            {
                onGround = false;
            }
        }
    }
}
