using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    this script handles inputs from the player in battle


*/



public class BattleInputHandler : MonoBehaviour
{
    public static BattleInputHandler current;

    //this variables represents a percentage out of 100 value of the window in one beat that you can make an input and get the bonus
    //so if this variable is 1 that means you have a window of 0.5+ and 0.5- from on the beat 
    //this variable is the radius of the range not the amount
    //if we're within that percentage of the length of the bar time then we still hit the indicator bonus

    [Range(0.0f, 1f)]
    public float timeWindow;

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public bool CheckIfOnTime()
    {
        float timeTolerance = timeWindow / 2;

        return false;
    }

    // Update is called once per frame
    void Update()
    {

        //TODO: 
        //1. make it so you cant use items until the battle starts, 
        //2. check if item use is on beat (will need to coordinate with the battle track manager)


        if (battleItemRebinding)
        {

            //TODO: need a binding for these (item quickbar 1-4)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BattleUIManager.current.EquipItem(rebindingItem, 1);
                battleItemRebinding = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BattleUIManager.current.EquipItem(rebindingItem, 2);
                battleItemRebinding = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                BattleUIManager.current.EquipItem(rebindingItem, 3);
                battleItemRebinding = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                BattleUIManager.current.EquipItem(rebindingItem, 4);
                battleItemRebinding = false;
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //check if we are on time 

                BattleUIManager.current.TryUseItemInSlot(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BattleUIManager.current.TryUseItemInSlot(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                BattleUIManager.current.TryUseItemInSlot(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                BattleUIManager.current.TryUseItemInSlot(4);
            }

        }

    }

    void CheckIfOnBeat()
    {

    }

    bool battleItemRebinding = false;
    Item rebindingItem;
    public void BattleRebindItem(Item item)
    {
        if (!battleItemRebinding)
        {
            battleItemRebinding = true;
            rebindingItem = item;
        }

    }
}
