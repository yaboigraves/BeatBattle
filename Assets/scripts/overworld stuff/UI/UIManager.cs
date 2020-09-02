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

        print("applying markup");

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
        trackTitleText.text = newTrack.trackName;

        //enable the trackinfo for a sec
        if (!trackInfoUI.activeSelf)
        {
            StartCoroutine(showTrackInfoUI(trackInfoUI, -300));
        }

    }

    IEnumerator showTrackInfoUI(GameObject uiElement, float distance)
    {
        //TODO: make this tween from the right side of the screen or something
        uiElement.SetActive(true);
        LeanTween.moveX(uiElement, uiElement.transform.position.x + distance, 1);
        yield return new WaitForSeconds(6);
        LeanTween.moveX(uiElement, uiElement.transform.position.x - distance, 1);
        yield return new WaitForSeconds(1);
        uiElement.SetActive(false);
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
        newTrackUI.GetComponentInChildren<TextMeshProUGUI>().text = newTrack.trackName;
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
        trackName.text = track.trackName;
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

    public TextMeshProUGUI gearNameText, gearDescriptionText;

    public void SelectGear(Gear gear)
    {
        gearNameText.text = gear.gearName;
        gearDescriptionText.text = gear.gearDescription;
    }

    public void UpdateGearInventory(Gear newGear)
    {
        GameObject newGearUI = Instantiate(inventoryGearUIPrefab, inventoryGearContainer.transform);
        newGearUI.GetComponent<InventoryGear>().gear = newGear;
    }

    public void UpdatePowerUse(int newPowerUse)
    {

    }

    public void UpdatePowerUse(int newPowerUse, int maxPowerUse)
    {

    }
}
