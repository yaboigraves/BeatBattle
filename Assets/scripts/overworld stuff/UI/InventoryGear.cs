using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGear : MonoBehaviour
{
    public Gear gear;
    public void SelectGear()
    {
        //so this needs to update the name,sprite,and description of the item viewer panel
        //UseItem();
        //UIManager.current.SelectItem(item);
        //enable the use button
        UIManager.current.SelectGear(gear);
    }

    public void EquipGear()
    {

        int currentPower = GameManager.current.player.GetComponent<PlayerInventory>().powerUse;
        int maxPower = GameManager.current.player.GetComponent<PlayerInventory>().maxPower;
        //so if you try to equip some gear we need to check and see if we have the power for it 
        if (gear.powerCost + currentPower <= maxPower)
        {
            //we can equip the gear all good

        }
    }
}
