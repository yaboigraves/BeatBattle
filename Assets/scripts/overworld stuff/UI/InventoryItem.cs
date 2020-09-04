using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public Item item;
    public void SelectItem()
    {
        //so this needs to update the name,sprite,and description of the item viewer panel
        //UseItem();
        UIManager.current.SelectItem(item);
        //enable the use button
    }

    public void UseItem()
    {
        item.Use();
        //now we tell the playerinventory to delete its shit
        GameManager.current.player.GetComponent<PlayerInventory>().items.Remove(item);
        // Destroy(this.gameObject);

        //need to destroy the item in the ItemListContent 
        UIManager.current.DeleteInventoryItem(item);

        //reset the inventory item
        UIManager.current.ResetInventoryItem();
    }
}
