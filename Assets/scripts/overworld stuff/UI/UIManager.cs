using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Yarn.Unity;
using TMPro;

public class UIManager : MonoBehaviour
{
    //TODO: make ui state an object and then i can do away with the like 6 or 7 variables at a time
    //components for fading in and out
    [Header("Overworld UI ELements")]
    public GameObject trackInfoUI, playerInfoUI;
    //trackinfo stuff could maybe be a dictionary or something
    public TextMeshProUGUI trackTitleText, trackArtistText, playerHealthText, coinText;
    //public TextRender dialogueRenderer;
    public static UIManager current;
    //public GameObject dialoguePanel;
    Text dialogueText;
    public Player player;
    Canvas canvas;

    [Header("UI Fade effects")]
    public CanvasGroup faderCanvas;
    public float fadeTime;
    //ui elements
    //public Text coinsText;
    //Yarn Variables 
    [Header("Dialog Stuff")]
    public TextMeshProUGUI dialogTextContainer;
    public DialogueRunner dialogueRunner;

    public TextEffectManager textEffectManager;

    //this variable tracks what letter we're currently on in the dialogue (used for text effects)
    public int currentLetter;

    public GameObject inventoryMenus;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //dialogueText = dialoguePanel.transform.GetChild(0).GetComponent<Text>();
        coinText.text = "Skrilla : 0";
        playerHealthText.text = "5 / 5";
        faderCanvas.alpha = 0;

        dialogueRunner.AddCommandHandler("applyMarkup", applyMarkup);
        dialogueRunner.AddCommandHandler("clearMarkup", clearMarkup);

        //init the power usage 
        UpdatePowerUse(GameManager.current.player.inventory.powerUse, GameManager.current.player.inventory.maxPower);
    }

    public void increaseLetterCount()
    {
        currentLetter++;
    }

    public void resetLetterCount()
    {
        currentLetter = 0;
    }

    public void clearMarkup(string[] parameters)
    {
        resetLetterCount();
        textEffectManager.ClearEffects();
    }

    public void applyMarkup(string[] parameters)
    {


        List<(string, (int, int))> effects = new List<(string, (int, int))>();

        foreach (string effectString in parameters)
        {
            //these return -1 if they dont find anything
            int seperatingSymbolPosition = effectString.IndexOf('-');
            int commaPos = effectString.IndexOf(',');

            if (seperatingSymbolPosition != -1 && commaPos != -1)
            {
                string effectName = effectString.Substring(0, seperatingSymbolPosition);
                string num1 = effectString.Substring(seperatingSymbolPosition + 1, commaPos - 1 - seperatingSymbolPosition);
                string num2 = effectString.Substring(commaPos + 1, effectString.Length - 1 - commaPos);

                int startMarkupPos = int.Parse(num1);
                int stopMarkupPos = int.Parse(num2);

                //for now we just tell the text effect manager the starting and ending position of the wiggle

                effects.Add((effectName, (startMarkupPos, stopMarkupPos)));
            }
            else
            {
                Debug.LogError("effects markup rendered wrong");
            }
        }
        textEffectManager.UpdateTextEffects(effects);
    }

    // Update is called once per frame
    public void toggleCanvas()
    {
        canvas.enabled = !canvas.enabled;
    }

    public void NPCNextTalk()
    {
        FindObjectOfType<DialogueUI>().MarkLineComplete();
    }

    public void leaveDialogue()
    {
        dialogTextContainer.text = "";
        player.leaveDialogue();

        //turns off the priority of the main camera
        DialogCameraController.current.resetCamera();

        //set the playercamera back top main priority
        CameraManager.current.currentCamera.Priority = 15;
    }

    public void updateCurrentTrack(Track newTrack)
    {
        trackArtistText.text = newTrack.artist;
        trackTitleText.text = newTrack.itemName;

        //enable the trackinfo for a sec
        if (!trackInfoUI.activeSelf)
        {
            print("routine");
            StartCoroutine(showTrackInfoUI(trackInfoUI, -300));
        }

    }


    public void ShowTrackInfo()
    {
        if (!trackInfoUI.activeSelf)
        {
            StartCoroutine(showTrackInfoUI(trackInfoUI, -300));
        }

    }

    IEnumerator showTrackInfoUI(GameObject uiElement, float distance)
    {
        //so this needs to not actually move the element back until the toggle mouse enter for that element is off
        //tie those together somehow

        //turn the parent on
        uiElement.gameObject.SetActive(true);
        LeanTween.moveX(uiElement, uiElement.transform.position.x + distance, 1);
        //wait until the toggleonmouseenter for that component is false


        yield return new WaitForSeconds(3);
        while (uiElement.transform.parent.gameObject.GetComponent<ToggleOnMouseEnter>().openUIInfo)
        {
            yield return new WaitForEndOfFrame();
        }


        yield return new WaitForSeconds(3);


        LeanTween.moveX(uiElement, uiElement.transform.position.x - distance, 1);
        yield return new WaitForSeconds(1);
        uiElement.gameObject.SetActive(false);
    }

    public void updatePlayerHealthText(int newHealth)
    {
        playerHealthText.text = newHealth.ToString() + "/" + player.maxHealth;
    }

    IEnumerator screenWipeRoutine()
    {
        faderCanvas.alpha = 1;
        yield return new WaitForSeconds(fadeTime);
        faderCanvas.alpha = 0;
    }

    public void screenWipe()
    {
        StartCoroutine(screenWipeRoutine());
    }

    public void updateCoinsText(int numCoins)
    {
        if (!playerInfoUI.activeSelf)
        {
            StartCoroutine(showTrackInfoUI(playerInfoUI, 250));
        }
        else
        {
            //TODO: add a little bit more time to the coroutine somehow
        }

        coinText.text = "Skrilla: " + numCoins.ToString();
    }

    public void ToggleInventoryMenu()
    {
        inventoryMenus.SetActive(!inventoryMenus.activeSelf);
    }

    //TODO: scrolling menu basics for the inventory so we can asctually use items and equip tracks 
    [Header("Item Inventory Containers")]
    public GameObject inventoryItemContainer;
    public GameObject inventoryItemUIPrefab;
    public void UpdateItemInventory(Item newItem)
    {
        //so we need a container to append to that scrolls down
        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryItemContainer.transform);
        newItemUI.GetComponentInChildren<TextMeshProUGUI>().text = newItem.name;
        newItemUI.GetComponent<InventoryItem>().item = newItem;
    }

    [Header("Track Inventory Container")]
    public GameObject inventoryTrackContainer;
    public GameObject inventoryTrackUIPrefab;
    public void UpdateTrackInventory(Track newTrack)
    {
        GameObject newTrackUI = Instantiate(inventoryTrackUIPrefab, inventoryTrackContainer.transform);
        newTrackUI.GetComponentInChildren<TextMeshProUGUI>().text = newTrack.itemName;
        newTrackUI.GetComponent<InventoryTrack>().track = newTrack;
    }

    [Header("Item Inventory Stuff")]
    public TextMeshProUGUI itemName, itemDescription;
    public InventoryItem useButton;
    public void SelectItem(Item selectedItem)
    {
        itemName.text = selectedItem.itemName;
        itemDescription.text = selectedItem.itemDescription;
        useButton.item = selectedItem;
        useButton.gameObject.SetActive(true);
    }

    public void ResetInventoryItem()
    {
        itemName.text = "";
        itemDescription.text = "";
        useButton.item = null;
        //turn off the usebutton
        useButton.gameObject.SetActive(false);
    }

    public void DeleteInventoryItem(Item deleteThisItem)
    {
        //loop through the itemlist content till we find the corresponding item (may be a more efficient way to do this)
        //definitly probably is a better way but fuck it for now
        for (int i = 0; i < inventoryItemContainer.transform.childCount; i++)
        {
            if (inventoryItemContainer.transform.GetChild(i).GetComponent<InventoryItem>().item = deleteThisItem)
            {
                //we found the item we're looking for remove that bitch
                Destroy(inventoryItemContainer.transform.GetChild(i).gameObject);
                return;
            }
        }
        Debug.LogError("Tried to delete item but item not found");
    }

    [Header("Track Inventory Stuff")]
    public TextMeshProUGUI trackName, trackArtist, trackDescription;
    public void SelectTrack(Track track)
    {
        trackName.text = track.itemName;
        trackArtist.text = track.artist;
        trackDescription.text = "this is a placeholder track description";
    }

    public void ResetTrack()
    {
        trackName.text = "";
        trackArtist.text = "";
        trackDescription.text = "";
    }

    [Header("Gear Stuff")]

    public GameObject inventoryGearContainer;
    public GameObject inventoryGearUIPrefab;
    public TextMeshProUGUI gearNameText, gearDescriptionText, currentPowerText, maxPowerText;

    public void SelectGear(Gear gear)
    {
        gearNameText.text = gear.itemName;
        gearDescriptionText.text = gear.gearDescription;
    }

    public void UpdateGearInventory(Gear newGear)
    {
        GameObject newGearUI = Instantiate(inventoryGearUIPrefab, inventoryGearContainer.transform);
        newGearUI.GetComponent<InventoryGear>().gear = newGear;
    }

    public void UpdatePowerUse(int newPowerUse)
    {
        currentPowerText.text = newPowerUse.ToString();
        TurnTogglesInteractable();
    }

    public void UpdatePowerUse(int newPowerUse, int maxPowerUse)
    {
        currentPowerText.text = newPowerUse.ToString();
        maxPowerText.text = "/" + maxPowerUse.ToString();
        TurnTogglesInteractable();
    }


    //so every time i update the power cost, got to iterate through all the gear and if their cost is larger
    //than can possibly fit given the max power they need to be made uninteractable

    public void TurnTogglesInteractable()
    {
        int currentPower = GameManager.current.player.inventory.powerUse;
        int maxPower = GameManager.current.player.inventory.maxPower;
        for (int i = 0; i < inventoryGearContainer.transform.childCount; i++)
        {
            inventoryGearContainer.transform.GetChild(i).GetComponent<InventoryGear>().CheckIfToggleInteractable(currentPower, maxPower);
        }
    }

    //INVENORY SHIT

    [Header("Shop stuff")]
    public GameObject shopCanvas;
    public GameObject shopItemPrefab;
    public GameObject shopItemContainer;
    public Shop currentShop;

    public TextMeshProUGUI playerCoinsText;

    GameItem selectedItem;

    public void OpenShop(Shop shop)
    {
        currentShop = shop;

        //update the player coins 

        UpdateShopCoins(GameManager.current.player.inventory.coins);

        shopCanvas.SetActive(true);
        //loop through the shopInventory and append prefabs to the list 
        UpdateShopInventory();
    }

    //this function just refreshes the shop inventory based on what's in the array/list of items

    public void UpdateShopInventory()
    {
        //remove everything thats in there 

        for (int i = 0; i < shopItemContainer.transform.childCount; i++)
        {
            Destroy(shopItemContainer.transform.GetChild(i).gameObject);
        }

        foreach (GameItem item in currentShop.inventory)
        {
            GameObject shopItem = Instantiate(shopItemPrefab, shopItemContainer.transform);
            shopItem.GetComponent<InventoryShopItem>().item = item;

            shopItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
        }


    }



    public void CloseShop()
    {
        shopCanvas.SetActive(false);

        //clear the inventory of the current shop
        for (int i = 0; i < shopItemContainer.transform.childCount; i++)
        {
            Destroy(shopItemContainer.transform.GetChild(i).gameObject);
        }

        GameManager.current.player.inShop = false;


    }

    public TextMeshProUGUI shopItemName, shopItemDescription, shopItemCost;

    public void SelectShopItem(GameItem item)
    {
        shopItemName.text = item.itemName;
        shopItemDescription.text = item.description;
        shopItemCost.text = item.cost.ToString();
        selectedItem = item;
    }

    public void BuyItem()
    {
        //TODO: this only works for items right now, fix that

        //check if the players got enough money to buy the item (later)

        if (selectedItem.cost <= GameManager.current.player.inventory.coins)
        {
            GameManager.current.player.inventory.coins -= selectedItem.cost;

            //update the ui

            UpdateShopCoins(GameManager.current.player.inventory.coins);

            player.inventory.GetItem(selectedItem);

            //remove it from the shop stock if it's unique
            if (selectedItem.unique)
            {
                print("unique item");
                currentShop.inventory.Remove(selectedItem);

                //remove it from the ui
                UpdateShopInventory();


            }
        }
        else
        {
            //TODO: play a noise or something if you cant afford it
        }



    }

    public void UpdateShopCoins(int newCoins)
    {
        playerCoinsText.text = "X - " + newCoins.ToString();
    }

    //LOADING STUFF

    public void LoadTrack(Track t)
    {
        //look through all the inventory track objects until we find one with this track




        //TODO: this needs to be made more efficient, a ui manager variable or player inventory variable needs to be made 
        //that packages all relevant objects, for now just doin this dumb shit



        for (int i = 0; i < inventoryTrackContainer.transform.childCount; i++)
        {
            print(inventoryTrackContainer.transform.GetChild(i).name);
            InventoryTrack inTrack = inventoryTrackContainer.transform.GetChild(i).GetComponent<InventoryTrack>();


            if (inTrack != null && inTrack.track == t)
            {
                inTrack.ToggleToggle(true);
            }
        }

    }





    //SETTINGS MENU STUFF
    public Slider volumeSlider;
    public void UpdateVolumeSlider()
    {
        TrackManager.current.UpdateTrackVolume(volumeSlider.value);
    }


    public void UpdateMidiBind(string midiName)
    {
        //rather than doing this in save manager lets just do it here?
        // SaveManager.RebindMidi(midiName);

        StopCoroutine("midiRebind");
        StartCoroutine(midiRebind(midiName));

    }

    public static IEnumerator midiRebind(string midiName)
    {
        bool inputMade = false;

        while (!inputMade)
        {
            for (int i = 0; i <= 127; i++)
            {
                if (MidiJack.MidiMaster.GetKeyDown(i))
                {
                    //we got a key to rebind
                    //set that shit in the player prefs
                    PlayerPrefs.SetInt(midiName, i);
                    DebugManager.current.print("rebound " + midiName + " to " + i);

                    inputMade = true;
                    break;
                }
            }
            yield return new WaitForEndOfFrame();

        }
    }

    public GameObject debugWindow;
    public void ToggleDebugWindow()
    {
        debugWindow.SetActive(!debugWindow.activeSelf);
    }

}
