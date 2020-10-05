using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickupableObject
{
    // Start is called before the first frame update
    void Start()
    {
        Hover();
    }

    public override void Pickup(PlayerInventory inventory)
    {
        inventory.getCoin();


        //destroy 
        base.Pickup(inventory);
    }


}
