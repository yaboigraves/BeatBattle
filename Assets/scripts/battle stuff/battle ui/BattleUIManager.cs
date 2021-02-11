using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//keep in mind!
//doing any movement to the ui forces a redraw 
//to minimize the performance hit, move any dynamic ui elements onto their own canvas

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager current;
    public int maxPlayerHealth, maxEnemyHealth;
    public TextMeshProUGUI metronomeText, playerHealthText, enemyHealthText;

    [Header("UI Sounds")]
    AudioSource audioSource;
    public AudioClip[] metronomeCounts;
    public GameObject BattleEndUI;

    public GameObject trackSelectionContainer;
    public GameObject[] trackSelectButtons;

    public Slider enemyHealthSlider;

    public Slider playerHealthSlider;

    public Slider vibeSlider;

    //vibe slider is on a -50 to 50 scale rn (0-100)

    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BattleEndUI.SetActive(false);
        //setup the buttons for the players track options
        //InitTrackButtons();


    }

    public void InitHealthSliders()
    {
        enemyHealthSlider.maxValue = maxEnemyHealth;
        enemyHealthSlider.value = maxEnemyHealth;

        playerHealthSlider.maxValue = maxPlayerHealth;
        playerHealthSlider.value = BattleManager.current.playerHealth;

    }

    public void InitTrackButtons()
    {
        //first set all the buttons to inactive 
        foreach (GameObject button in trackSelectButtons)
        {
            button.SetActive(false);
        }

        //non test mode
        if (TrackManager.current != null)
        {
            //loop through all the player tracks and setup the buttons based on how many options there are
            for (int i = 0; i < BattleManager.current.playerTracks.Length; i++)
            {
                Track currTrack = BattleManager.current.playerTracks[i];
                if (currTrack != null)
                {
                    //turn on the button
                    trackSelectButtons[i].SetActive(true);
                    //set the text of the button
                    trackSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currTrack.itemName;
                    //set the track that this actually enables
                    trackSelectButtons[i].GetComponent<TrackSelectButton>().track = currTrack;
                }
            }
        }
        //test mode
        else
        {
            for (int i = 0; i < BattleTrackManager.current.testPlayerTracks.Length; i++)
            {
                Track currTrack = BattleTrackManager.current.testPlayerTracks[i];

                trackSelectButtons[i].SetActive(true);
                //set the text of the button
                trackSelectButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currTrack.itemName;
                //set the track that this actually enables
                trackSelectButtons[i].GetComponent<TrackSelectButton>().track = currTrack;
            }
        }

    }


    public void ToggleTrackSelectorOn(bool toggle)
    {
        trackSelectionContainer.SetActive(toggle);
    }

    //so the player needs to be able to select a battle track to use for the next loop, this then needs to be sent to 
    //the track manager and the battle manager so it can get setup for the next track
    //the trackname may not be the best argument to figure out what track was selected
    public void SetPlayerTrack(Track newTrack)
    {
        //find the track based on its trackname in the 
        BattleTrackManager.current.setPlayerSelectedTrack(newTrack);
    }

    public void setMaxHealths(int mPHealth, int mEHealth)
    {
        maxPlayerHealth = mPHealth;
        maxEnemyHealth = mEHealth;
        InitHealthSliders();
    }

    public void updatePlayerHealth(int newHealth)
    {
        playerHealthText.text = newHealth.ToString() + "/" + maxPlayerHealth.ToString();
        playerHealthSlider.value = newHealth;
    }

    public void updateEnemyHealth(int newHealth)
    {
        enemyHealthText.text = newHealth.ToString() + "/" + maxEnemyHealth.ToString();
        enemyHealthSlider.value = newHealth;
    }

    public void UpdateMetronome(double currentBeat, bool playAudio)
    {
        metronomeText.text = currentBeat.ToString();

        // if (playAudio)
        // {
        //     audioSource.clip = metronomeCounts[currentBeat];
        //     audioSource.Play();
        // }
    }

    public void EndBattleSequence(bool playerWon)
    {
        StartCoroutine(endBattleRoutine(playerWon));
        BattleEndUI.SetActive(true);
    }

    IEnumerator endBattleRoutine(bool playerWon)
    {
        print("beep boop end");
        yield return new WaitForSeconds(3);
        BattleManager.current.EndStopBattle(playerWon);
    }

    public TextMeshProUGUI vibeText;
    public void UpdateVibe(int vibe)
    {
        vibeText.text = vibe.ToString();
        vibeSlider.value = vibe + 50;
    }


    public GameObject damageNumber;
    public GameObject damageNumberContainer;
    public void CreateDamageNumber(int damage)
    {
        //find the screenspace position that the ui element needs to be spawned at 
        //for now just gonna spawn it at the enemies healthbar 
        FloatingStatusText dmg = Instantiate(damageNumber, enemyHealthSlider.transform.position + (Vector3.up * 40), Quaternion.identity, damageNumberContainer.transform).GetComponent<FloatingStatusText>();
        dmg.setDamage(damage);

    }

    //battle icon shit

    public GameObject itemIcon, gearIconContainer;

    public UIIcon[] gearUIIcons;

    public void CreateGearIcons(List<Gear> gears)
    {
        gearUIIcons = new UIIcon[gears.Count];

        int counter = 0;
        foreach (Gear gear in gears)
        {
            UIIcon gearIcon = Instantiate(itemIcon, gearIconContainer.transform.position, Quaternion.identity, gearIconContainer.transform).GetComponent<UIIcon>();
            gearIcon.SetIconItem(gear);

            gearUIIcons[counter] = gearIcon;
            counter++;
        }

    }

    //when a gear effect is called and enabled we go through the icons and find the one we need to turn on
    public void ToggleUiIconBorder(string functionName, bool toggle)
    {
        foreach (UIIcon icon in gearUIIcons)
        {
            icon.checkIfEffectActive(functionName, toggle);
        }
    }


    //battle item stuff 
    //basic idea for items in battles
    //you can access your big list of items at any time (will probably need some sorting options at some point)
    //you bind items to one of your four item slots
    //you then hit the bind or click the item icon to use the item 
    //using the item on beat will do the items effect but multiplied (each item will need an additional multiplier for this)

    //what do we gotta do to set that shit up 
    //the inventory is going to need a new array[4] of items that are your hotbar
    //pull all the items from the players inventory into the items list 
    //pull hotbar into hotbar slots

    //set the hotbar icons = to the item that is equipped there

    public UIIcon[] itemIcons;

    public GameObject battleUIItem;
    public GameObject itemListContainer;

    //shallow copy of players inventory items
    public List<Item> battleItems;

    public void LoadItemsList(List<Item> items)
    {
        battleItems = items;
        //go through the player inventory if player inventory existes 
        foreach (Item item in items)
        {
            GameObject newItemUI = Instantiate(battleUIItem, itemListContainer.transform);
            newItemUI.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            newItemUI.GetComponent<BattleInventoryItem>().item = item;
        }
    }

    public void EquipItem(Item item, int keyBound)
    {
        //so we need to go throug the list of itemicons 
        //if we find an empty iconslot then we just slot the item in there
        //otherwise we boot out the last element, shift all the other elements over one, and then plug the item into
        //first slot 

        //alternatively you hit the button then hit the key to bind it there(do this one instead)
        //so the input handler needs to be told that we're waiting for a rebind, we just tell it to listen for 1,2,3,4 

        //after the input handler recieves the event it then fires back to the actual equip function which then fucks with that slot
        if (keyBound == 0)
        {
            BattleInputHandler.current.BattleRebindItem(item);
        }
        else
        {
            switch (keyBound)
            {
                case 1:
                    itemIcons[0].SetIconItem(item);
                    break;
                case 2:
                    itemIcons[1].SetIconItem(item);
                    break;
                case 3:
                    itemIcons[2].SetIconItem(item);
                    break;
                case 4:
                    itemIcons[3].SetIconItem(item);
                    break;
            }
        }
    }

    public void TryUseItemInSlot(int slot)
    {
        if (!BattleManager.current.battleStarted)
        {
            return;
        }


        if (itemIcons[slot - 1].item != null)
        {
            if (itemIcons[slot - 1].item is Item)
            {
                //so we use the item now
                Item item = (Item)(itemIcons[slot - 1].item);

                //check if we're within the window for this to be on beat

                item.Use(checkIfInputOnBeat());

                //once we use the item we now need to remove it from both our battle inventory list and the normal list 
                //an optimization that can be done later is rather than removing it from the main inventory as well, when 
                //we leave a battle we can just copy the battles version over to the new version 

                RemoveItemFromInventory(item);

                itemIcons[slot - 1].ResetItem();
            }
        }
    }

    private void RemoveItemFromInventory(Item item)
    {
        //go through the inventory till we find an item of this type and remove it

        for (int i = 0; i < itemListContainer.transform.childCount; i++)
        {
            if (itemListContainer.transform.GetChild(i).GetComponent<BattleInventoryItem>().item == item)
            {
                Destroy(itemListContainer.transform.GetChild(i).gameObject);
                return;
            }
        }
    }

    //range out of 100% you can miss a beat by and still get a correct input
    public float inputOnBeatRange = 0.2f;
    public bool checkIfInputOnBeat()
    {
        //look at the current time in beats and see if we're within the range of a whole number 

        float nearestBeat = Mathf.Round((float)TrackTimeManager.songPositionInBeats);
        if (Mathf.Abs((float)TrackTimeManager.songPositionInBeats - nearestBeat) < inputOnBeatRange)
        {
            //print("good hit on beat");
            return true;
        }
        else
        {
            //print("hit not on beat");
            return false;
        }
    }



    public void LoadHotbarItems()
    {

    }

    //render on top of pad test
    public Transform[] padPositions;
    public GameObject testPadText;
    public Camera uiCamera;
    public void SpawnHitText(int padIndex, string statusMessage, Color color)
    {
        //render some shit at screen space on top of the pad
        FloatingStatusText ft = Instantiate(testPadText, uiCamera.WorldToScreenPoint(padPositions[padIndex].position), Quaternion.identity, transform.GetChild(0)).GetComponent<FloatingStatusText>();
        ft.setText(statusMessage, color);
    }

}
