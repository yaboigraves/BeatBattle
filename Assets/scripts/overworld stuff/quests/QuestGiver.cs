using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    //so give quest needs to just mark the quest as in progress
    //once its in progress anytime we try and interact with this npc again, we check if theyre done with the quest 

    private void Start()
    {
        QuestManager.current.currentQuests.Add(quest.questName, this);
    }

    public void GiveQuest()
    {
        print("starting quest");
    }

    public void tryTurnInQuest()
    {
        print("trying to turn in quest");
        //this is a rather long process
        //first thing we need to do is check if the player inventory contains the quest item we're looking for 
        //if it does we know you can complete the quest 
        //then need to modify the yarn variable storage so the npc dialogue properly renders the reward text
        //switch to the cutscene manager if an item is being recieved
        bool questObjectiveDone = GameManager.current.player.inventory.CheckIfQuestItemInInventory((Item)quest.questObjective);

        if (questObjectiveDone)
        {
            GameManager.current.player.inventory.RemoveInventoryItem((Item)quest.questObjective);
            print("quest done!");
        }
        else
        {
            print("quest not done");
        }
    }
}