using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInventoryItem : MonoBehaviour
{
    //so we need some way to equip these to the hotbar
    //dragging them would be cool for now just click them and they get slotted into one of the four slots like a que 
    public Item item;

    public void EquipBattleItem()
    {
        //tell the battleui manager that we want to equip this item
        BattleUIManager.current.EquipItem(item, 0);
    }

}
