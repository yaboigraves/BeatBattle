using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //TODO: setup equipabble tracks, this will probably require some thinking out

    //so tracks are items that you can pickup from enemies after you defeat them
    //once you have a track you can then use that track in combat if you choose to on your turn
    //you pick the track on the enemies turn and then that track is loaded as a short loop 
    //this will require tracks to be setup as 4 - 8 bar loops 

    //firstly we need to store these tracks inside the inventory, this will be done via a simple array for now 
    //later these loops will need to be changeable from within the inventory screen 

    //once in battle, we need to integrate a swap between the players turn and the enemies turn
    //this means that battle indicators need to be spawned in dynamically rather than all at once

    //in battle, once the players turn is finished the enemy then plays out its loop after a 1 bar delay 

    //at any point the player can cycle through their tracks and select a loop to play on their next turn
    //perhaps certain loops will have certain elements that make them stronger in situations?

    public Item testItem;
    public int coins;
    public Track[] playerTracks;
    public Track[] battleEquippedTracks;
    public List<Item> items = new List<Item>();
    public Gear[] playerGear;
    public int powerUse, maxPower = 10;

    private void Start()
    {
        items.Add(testItem);
        items.Add(testItem);
        items.Add(testItem);
        items.Add(testItem);
        items.Add(testItem);
        //testItem.Use();
        //UIManager.current.UpdateItemInventory(testItem);
        foreach (Track t in playerTracks)
        {
            UIManager.current.UpdateTrackInventory(t);
        }

        foreach (Item i in items)
        {
            UIManager.current.UpdateItemInventory(i);
        }

        for (int i = 0; i < 40; i++)
        {
            UIManager.current.UpdateGearInventory(playerGear[0]);
        }

    }

    public void getCoin()
    {
        coins++;
        UIManager.current.updateCoinsText(coins);
    }

    public void EquipTrack(Track t, bool equip)
    {
        //find the first empty slot 
        if (equip)
        {
            //check if its already equipped 
            for (int i = 0; i < battleEquippedTracks.Length; i++)
            {
                if (battleEquippedTracks[i] == t)
                {
                    return;
                }
            }

            for (int i = 0; i < battleEquippedTracks.Length; i++)
            {
                if (battleEquippedTracks[i] == null)
                {
                    battleEquippedTracks[i] = t;
                    return;
                }
            }
        }
        else
        {
            //check if its already equipped 
            for (int i = 0; i < battleEquippedTracks.Length; i++)
            {
                if (battleEquippedTracks[i] == t)
                {
                    battleEquippedTracks[i] = null;
                    return;
                }
            }
        }
    }
}
