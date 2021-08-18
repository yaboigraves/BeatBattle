using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BattleUIManager : MonoBehaviour
{
    public Transform turnQueuePanel, setCustomizationPanel, sampleRepoContent, sampleRepoPanel;
    public GameObject turnInfoPrefab;
    public static BattleUIManager current;
    public List<Transform> turnOrderQueue;
    public TextMeshProUGUI playerHealthText, enemyHealthText;
    public GameObject sampleIconPrefab;
    //ENEMY PROTOTYPE SHIT
    public EventSystem uiEventSystem;
    GameObject[] sampleIconObjects;
    public GameObject[] turnQueueIconObjects;
    ActionIcon[] actionIcons;

    //so for the ui stuff for selecting samples we need to move the currently interactable section between
    //the turn queue and the sample repo when you scroll through it

    //when you select a player action the menu should appear, and then you can pick from the samples for that action


    //so once the sample repo is opened, we need to track what actual turnactionicon we're changing
    //this can be done in the button onclick? probably
    public GameObject currentlySelectedTurnAction;

    public GameObject goButton;


    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //enable the set customizer panel
        InitSampleLibraryPanel();
        InitSetCustomizationPanel();

        //print("adding update metronome");
        TimeManager.beatCallbacks.Add(UpdateMetronome);


    }

    public void SetCurrentlySelectedTurnAction(GameObject action)
    {
        currentlySelectedTurnAction = action;
        //Debug.Log("set the action");
    }

    //so we need some kind of visualization for two things
    //-some kind of ui for the set that provides slots between enemy actions where you build the set
    //-some kind of ui for a repository of samples that you can plug in to the above ui slots

    public void InitSampleLibraryPanel()
    {
        sampleIconObjects = new GameObject[BattleManager.current.playerSamples.Length];
        for (int i = 0; i < BattleManager.current.playerSamples.Length; i++)
        {
            GameObject icon = Instantiate(sampleIconPrefab);
            icon.transform.SetParent(sampleRepoContent.transform, false);
            icon.transform.GetComponentInChildren<TextMeshProUGUI>().text = BattleManager.current.playerSamples[i].sampleName[0].ToString();

            // if (i == 0)
            // {
            //     uiEventSystem.firstSelectedGameObject = icon;
            // }

            Sample s = BattleManager.current.playerSamples[i];

            icon.GetComponent<SampleIcon>().sample = s;

            icon.GetComponent<Button>().onClick.AddListener(delegate { SetActionSample(s); });
            sampleIconObjects[i] = icon;


        }
    }

    //TODO: this needs to take an argument of whatever the actual sample object is, so we can pass it around
    public void SetActionSample(Sample sample)
    {
        //Debug.Log("Setting action sample");
        //turn off the sample library
        //move the selection to the next action in the queue
        ToggleTurnQueueObjects(true);
        sampleRepoPanel.gameObject.SetActive(false);

        //so we need to set both the queue ui to display what action was modified

        //first take the current action and set it to whatever was selected
        //take the currently selected icon


        //set the action icon info of the selected object
        currentlySelectedTurnAction.GetComponent<ActionIcon>().sample = sample;
        currentlySelectedTurnAction.GetComponent<ActionIcon>().actionSet = true;

        currentlySelectedTurnAction.transform.GetComponentInChildren<TextMeshProUGUI>().text = sample.sampleName;



        //TODO: set this actually in the queue

        //set the current selection in the event system to whatever we just set
        uiEventSystem.SetSelectedGameObject(currentlySelectedTurnAction);
        //unset the currently selected action
        currentlySelectedTurnAction = null;

        if (checkIfAllActionsSet())
        {
            //turn on the go button
            goButton.SetActive(true);
        }

        //check and see if its time to enable the go button if the player has set all their samples
    }

    bool checkIfAllActionsSet()
    {
        foreach (ActionIcon i in actionIcons)
        {
            if (!i.actionSet)
            {
                return false;
            }
        }
        return true;
    }

    void ToggleTurnQueueObjects(bool toggle)
    {
        foreach (GameObject o in turnQueueIconObjects)
        {
            o.GetComponent<Button>().interactable = toggle;
        }
    }

    public void OpenSampleLibrary()
    {
        //Debug.Log("opening sample library");

        //set all of the turnqueue stuff as not interactable

        ToggleTurnQueueObjects(false);

        sampleRepoPanel.gameObject.SetActive(true);
        uiEventSystem.SetSelectedGameObject(sampleIconObjects[0]);
    }

    //pull all the samples from the battle manager and load them into the panel
    public void InitSetCustomizationPanel()
    {

        actionIcons = new ActionIcon[turnQueueIconObjects.Length];

        //grab all the actionicons
        for (int i = 0; i < turnQueueIconObjects.Length; i++)
        {
            actionIcons[i] = turnQueueIconObjects[i].GetComponent<ActionIcon>();
        }

        setCustomizationPanel.gameObject.SetActive(true);

    }

    public void InitTurnQueue(List<BattleAction> turnQueue)
    {
        turnOrderQueue = new List<Transform>();
        for (int i = 0; i < turnQueue.Count; i++)
        {
            GameObject turnInfo = Instantiate(turnInfoPrefab);
            turnInfo.transform.SetParent(turnQueuePanel);
            turnInfo.transform.position = new Vector3(Screen.width / 2 + (i * 200), 75, 0);

            if (turnQueue[i].playerOrEnemy == true)
            {
                turnInfo.GetComponent<TurnInfo>().SetInfo(((PlayerBattleAction)turnQueue[i]).sample.sampleName, ((PlayerBattleAction)turnQueue[i]).sample.numericValue.ToString());
            }
            else
            {
                turnInfo.GetComponent<TurnInfo>().SetInfo("Enemy Attack", ((EnemyBattleAction)turnQueue[i]).dmg.ToString());

            }

            if (turnQueue[i].playerOrEnemy)
            {
                turnInfo.GetComponent<Image>().color = Color.green;
            }
            else
            {
                turnInfo.GetComponent<Image>().color = Color.red;
            }

            turnOrderQueue.Add(turnInfo.transform);

        }
    }

    public void UpdateTurnQueue()
    {

        //TODO: rewrite this so that it shifts 2 elements over
        // for (int i = turnOrderQueue.Count - 1; i > 0; i--)
        // {
        //     turnOrderQueue[i].transform.position = turnOrderQueue[i - 1].transform.position;
        //     turnOrderQueue[i + 1].transform.position = turnOrderQueue[i].transform.position;
        // }

        for (int i = turnOrderQueue.Count - 1; i > 1; i -= 1)
        {
            //so the 0 and 1 position dont move at all
            // turnOrderQueue[i].transform.position = turnOrderQueue[i - 1].transform.position;

            //so every iteration we move the player turn over 2 and the enemy turn over 2
            turnOrderQueue[i].transform.position -= (Vector3.right * 400);

        }

        Destroy(turnOrderQueue[0].gameObject);
        turnOrderQueue.RemoveAt(0);
        Destroy(turnOrderQueue[0].gameObject);
        turnOrderQueue.RemoveAt(0);


    }


    //ok now lets make these actual health bars
    public void UpdateHealth()
    {
        playerHealthText.text = BattleManager.current.PlayerHealth.ToString();
        enemyHealthText.text = BattleManager.current.EnemyHealth.ToString();
    }

    //go button
    public void FinishSetSetup()
    {
        //turn off the set customization panel
        goButton.SetActive(false);
        setCustomizationPanel.gameObject.SetActive(false);

        //turn on the rest of the battleui

        //use the configuration of the set in the ui to then construct the actual turnQueue
        //pass an array of samples to the battlemanager
        Sample[] playerSet = new Sample[actionIcons.Length];

        for (int i = 0; i < actionIcons.Length; i++)
        {
            playerSet[i] = actionIcons[i].sample;
        }

        //load up everything
        BattleManager.current.InitQueue(playerSet);


        //so at this point the player set needs to get
    }

    int currentBeat = 0;

    public TextMeshProUGUI metronomeText;
    public void UpdateMetronome()
    {
        currentBeat++;
        if (currentBeat > 4)
        {
            currentBeat = 1;
        }

        metronomeText.text = currentBeat.ToString();

    }
}
