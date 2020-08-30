using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public Item item;
    public void SelectItem()
    {
        //for now this just uses the item but what it should do is set the item to go to the preview area where we can use it
        //and see its description
        UseItem();
    }

    public void UseItem()
    {
        item.Use();
        //now we tell the playerinventory to delete its shit
        GameManager.current.player.GetComponent<PlayerInventory>().items.Remove(item);
        Destroy(this.gameObject);
    }


}
