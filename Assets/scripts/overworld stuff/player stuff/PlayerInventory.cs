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
    public List<Track> playerTracks;
    public Track[] battleEquippedTracks;
    public List<Item> items = new List<Item>();
    public List<Gear> playerGear;
    public List<Gear> equippedGear = new List<Gear>();
    public int powerUse, maxPower = 10;
    //delegate list of gear effects
    public List<GearEffects.GearEffect> gearEffects = new List<GearEffects.GearEffect>();


    //newitem display sprite 
    public SpriteRenderer itemDisplaySprite;

    private void Start()
    {
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

        for (int i = 0; i < 3; i++)
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

    public void EquipGear(Gear gear)
    {
        //TODO: this needs to be implemented here otherwise this will get confusing

        powerUse += gear.powerCost;
        UIManager.current.UpdatePowerUse(powerUse);
        EquipGearEffect(gear.gearFunction);

        equippedGear.Add(gear);
    }

    public void UnEquipGear(Gear gear)
    {
        equippedGear.Remove(gear);
    }


    public void EquipGearEffect(string effectName)
    {
        print("applying effect " + effectName);
        gearEffects.Add(GearEffects.gearEffectDictionary[effectName]);
    }

    public void UnequipGearEffect(string effectName)
    {
        print("Removing the " + effectName);
        gearEffects.Remove(GearEffects.gearEffectDictionary[effectName]);

    }

    public void GetItem(GameItem item)
    {
        //later this needs to figure out what type the item is

        //if the item is unique or you've never picked up one of these items before 
        //TODO: log picked up items in save so we know if to play the unique animation
        if (item.unique)
        {
            PickupItemCutscene(item);
        }
        else
        {
            SetPickupItemSprite(item);
        }

        if (item is Gear)
        {
            playerGear.Add((Gear)item);
            UIManager.current.UpdateGearInventory((Gear)item);
        }
        else if (item is Track)
        {
            playerTracks.Add((Track)item);
            UIManager.current.UpdateTrackInventory((Track)item);
        }
        else if (item is Item)
        {
            items.Add((Item)item);

            //tell the ui that we got an item 
            UIManager.current.UpdateItemInventory((Item)item);
        }
    }

    //used by stuff like playerprefs to load gear by its name
    public void LoadItemByName(string name)
    {
        foreach (Gear g in playerGear)
        {
            if (g.itemName == name)
            {
                EquipGear(g);

                UIManager.current.LoadGear(g);
                return;
            }
        }

        foreach (Track t in playerTracks)
        {
            if (t.itemName == name)
            {
                //equp the track
                EquipTrack(t, true);
                //also need to update the ui with the toggle
                UIManager.current.LoadTrack(t);
                return;
            }
        }
    }
    public float pickupWaitTime;

    public void PickupItemCutscene(GameItem item)
    {
        //tell the cutscene manager to handle moving the camera around and stuff
        CutsceneManager.current.PickupUniqueItemCutscene();

        TogglePickupItemSprite(true, item);
    }

    public void TogglePickupItemSprite(bool toggle, GameItem item = null)
    {
        if (toggle)
        {
            itemDisplaySprite.gameObject.SetActive(true);
            itemDisplaySprite.sprite = item.itemIcon;
        }
        else
        {
            itemDisplaySprite.gameObject.SetActive(false);
            itemDisplaySprite.sprite = null;
        }
    }

    public void SetPickupItemSprite(GameItem item)
    {
        StartCoroutine(pickupItemRoutine(item));
    }

    IEnumerator pickupItemRoutine(GameItem item)
    {
        itemDisplaySprite.gameObject.SetActive(true);
        itemDisplaySprite.sprite = item.itemIcon;
        yield return new WaitForSeconds(pickupWaitTime);
        itemDisplaySprite.sprite = null;
        itemDisplaySprite.gameObject.SetActive(false);
    }

    public bool CheckIfQuestItemInInventory(Item item)
    {
        if (items.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveInventoryItem(Item item)
    {
        items.Remove(item);
    }
}
