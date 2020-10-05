using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : PickupableObject
{
    public GameItem item;


    private void Start()
    {
        Hover();
    }

    public override void Pickup(PlayerInventory inventory)
    {

        inventory.GetItem(item);
        base.Pickup(inventory);

        //put the item sprite above the players head for a moment
    }


}
