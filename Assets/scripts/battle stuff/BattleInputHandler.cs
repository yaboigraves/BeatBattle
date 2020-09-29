using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    this script handles inputs from the player in battle


*/



public class BattleInputHandler : MonoBehaviour
{
    public static BattleInputHandler current;







    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


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
