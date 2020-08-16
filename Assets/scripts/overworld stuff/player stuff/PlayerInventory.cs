using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //TODO: setup equipabble tracks, this will probably require some thinking out
    public int coins;

    public void getCoin()
    {
        coins++;
        UIManager.current.updateCoinsText(coins);
    }
}
