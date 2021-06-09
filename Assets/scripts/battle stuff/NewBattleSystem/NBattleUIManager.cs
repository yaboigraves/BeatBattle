using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class NBattleUIManager : MonoBehaviour
{
    public Transform turnQueuePanel, setCustomizationPanel, sampleRepoContent, sampleRepoPanel;
    public GameObject turnInfoPrefab;

    public static NBattleUIManager current;

    public List<Transform> turnOrderQueue;

    public TextMeshProUGUI playerHealthText, enemyHealthText;

    public GameObject sampleIconPrefab;

    //ENEMY PROTOTYPE SHIT
    public EventSystem uiEventSystem;

    GameObject[] sampleIconObjects;

    public GameObject[] turnQueueIconObjects;


    //so for the ui stuff for selecting samples we need to move the currently interactable section between
    //the turn queue and the sample repo when you scroll through it

    //when you select a player action the menu should appear, and then you can pick from the samples for that action



    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //enable the set customizer panel
        InitSampleLibraryPanel();
        InitSetCustomizationPanel();
    }


    //so we need some kind of visualization for two things
    //-some kind of ui for the set that provides slots between enemy actions where you build the set
    //-some kind of ui for a repository of samples that you can plug in to the above ui slots

    public void InitSampleLibraryPanel()
    {
        sampleIconObjects = new GameObject[NBattleManager.current.playerSamples.Length];
        for (int i = 0; i < NBattleManager.current.playerSamples.Length; i++)
        {
            GameObject icon = Instantiate(sampleIconPrefab);
            icon.transform.SetParent(sampleRepoContent.transform, false);
            icon.transform.GetComponentInChildren<TextMeshProUGUI>().text = NBattleManager.current.playerSamples[i].sampleName[0].ToString();

            // if (i == 0)
            // {
            //     uiEventSystem.firstSelectedGameObject = icon;
            // }

            icon.GetComponent<Button>().onClick.AddListener(SetActionSample);
            sampleIconObjects[i] = icon;
        }
    }

    public void SetActionSample()
    {
        Debug.Log("Setting action sample");
        //turn off the sample library
        //move the selection to the next action in the queue
        ToggleTurnQueueObjects(true);
        sampleRepoPanel.gameObject.SetActive(false);


        //so we need to

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
        Debug.Log("opening sample library");

        //set all of the turnqueue stuff as not interactable

        ToggleTurnQueueObjects(false);

        sampleRepoPanel.gameObject.SetActive(true);
        uiEventSystem.SetSelectedGameObject(sampleIconObjects[0]);

    }

    //pull all the samples from the battle manager and load them into the panel
    public void InitSetCustomizationPanel()
    {

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

    public void UpdateHealth()
    {
        playerHealthText.text = NBattleManager.current.playerHealth.ToString();
        enemyHealthText.text = NBattleManager.current.enemyHealth.ToString();
    }


}
