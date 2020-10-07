using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class QuestManager : MonoBehaviour
{
    //quest managers job is to handle dispatching quests
    public static QuestManager current;

    public DialogueRunner dialogueRunner;

    public InMemoryVariableStorage yarnVariableStorage;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        dialogueRunner.AddCommandHandler("startQuest", startQuest);
        dialogueRunner.AddCommandHandler("tryTurnInQuest", tryTurnInQuest);
        dialogueRunner.AddCommandHandler("giveQuestReward", giveQuestReward);
    }

    //big ol public dictionary full of the current quests
    //all quest givers add their quests to this 
    public Dictionary<string, QuestGiver> currentQuests = new Dictionary<string, QuestGiver>();

    public void startQuest(string[] parameters)
    {
        currentQuests[parameters[0]].GiveQuest();
    }

    public void tryTurnInQuest(string[] paramaters)
    {
        currentQuests[paramaters[0]].tryTurnInQuest();
    }

    public void markQuestDone(string questName)
    {
        yarnVariableStorage.SetValue(questName, 2);
    }

    public void giveQuestReward(string[] parameters)
    {
        currentQuests[parameters[0]].GiveQuestReward();
    }


}