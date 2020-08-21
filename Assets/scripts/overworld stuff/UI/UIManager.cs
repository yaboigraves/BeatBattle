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
    public GameObject trackInfoUI, playerInfoUI;
    //trackinfo stuff could maybe be a dictionary or something
    public TextMeshProUGUI trackTitleText, trackArtistText, playerHealthText, coinText;
    //public TextRender dialogueRenderer;
    public static UIManager current;
    //public GameObject dialoguePanel;
    Text dialogueText;
    public Player player;
    Canvas canvas;
    public CanvasGroup faderCanvas;
    public float fadeTime;
    //ui elements
    //public Text coinsText;
    //Yarn Variables 
    public TextMeshProUGUI dialogTextContainer;
    public DialogueRunner dialogueRunner;

    public TextEffectManager textEffectManager;

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
    }


    public void applyMarkup(string[] parameters)
    {

        print("applying markup");
        //take in an array of strings of effects to apply and the indexes in the string to apply them
        //ex: wiggle-4,6

        //TODO: write parser for text effects
        //need to parse these into the effects to call and the 

        List<string> effects = new List<string>();

        foreach (string effectString in parameters)
        {
            int seperatingSymbolPosition = effectString.IndexOf('-');
            int commaPos = effectString.IndexOf(',');

            string effectName = effectString.Substring(0, seperatingSymbolPosition);
            string num1 = effectString.Substring(seperatingSymbolPosition + 1, commaPos - 1 - seperatingSymbolPosition);
            string num2 = effectString.Substring(commaPos + 1, effectString.Length - 1 - commaPos);

            // print("effect name " + effectName);
            // print("num 1 " + num1);
            // print("num 2 " + num2);

            int startMarkupPos = int.Parse(num1);
            int stopMarkupPos = int.Parse(num2);

            //for now we just tell the text effect manager the starting and ending position of the wiggle

            textEffectManager.wiggleStart = startMarkupPos;
            textEffectManager.wiggleEnd = stopMarkupPos;
        }
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






}
