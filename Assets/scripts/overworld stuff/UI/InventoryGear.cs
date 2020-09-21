using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGear : MonoBehaviour
{
    PlayerInventory playerInventory;
    public Toggle toggle;
    public Gear gear;
    private void Start()
    {
        toggle = GetComponentInChildren<Toggle>();
        playerInventory = GameManager.current.player.GetComponent<PlayerInventory>();
    }
    public void SelectGear()
    {

        UIManager.current.SelectGear(gear);
    }

    public void toggleEquipGear()
    {
        if (toggle.isOn)
        {
            EquipGear();
        }
        else
        {
            UnequipGear();
        }
    }

    //pass this to the uimanager to equi[]
    public void EquipGear()
    {
        int currentPower = playerInventory.powerUse;
        int maxPower = playerInventory.maxPower;

        //so if you try to equip some gear we need to check and see if we have the power for it 
        if (gear.powerCost + currentPower <= maxPower)
        {
            playerInventory.EquipGear(gear);
        }
    }

    //TODO: REFACTOR THIS LIKE EQUIP GEAR, DEFER RESPONSIBILITY TO PLAYERINVENTORY
    public void UnequipGear()
    {
        playerInventory.powerUse -= gear.powerCost;
        UIManager.current.UpdatePowerUse(playerInventory.powerUse);

        //remove the gears effect from the list 
        playerInventory.UnEquipGear(gear);
        playerInventory.UnequipGearEffect(gear.gearFunction);
    }

    //this is used to see if our toggle should stay interactable given the current power usage
    public void CheckIfToggleInteractable(int currentPower, int maxPower)
    {
        //if the toggle is on dont make it uninteractable 
        if (toggle.isOn)
        {
            return;
        }

        if (gear.powerCost + currentPower > maxPower)
        {
            toggle.interactable = false;
        }
        else
        {
            toggle.interactable = true;
        }
    }

    public void ToggleToggle(bool on)
    {
        toggle.isOn = on;
    }
}
