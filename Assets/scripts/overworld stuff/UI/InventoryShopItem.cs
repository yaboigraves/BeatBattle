using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShopItem : MonoBehaviour
{
    public GameItem item;

    public void SelectItem()
    {
        //tell the uimanager to update the selected shop item
        //grab the items name, its description, thumbnail, and cost
        UIManager.current.SelectShopItem(item);


    }

    public void BuyItem()
    {

    }
}
