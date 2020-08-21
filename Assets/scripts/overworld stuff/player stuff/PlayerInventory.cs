using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //TODO: setup equipabble tracks, this will probably require some thinking out

    //so we need a list of items 


    public Item testItem;
    public int coins;


    private void Start()
    {
        testItem.Use();
    }


    public void getCoin()
    {
        coins++;
        UIManager.current.updateCoinsText(coins);
    }
}
