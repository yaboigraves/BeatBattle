using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    //trackinfo stuff 
    public Text trackTitleText, trackArtistText, playerHealthText;
    public TextRender dialogueRenderer;
    public static UIManager current;
    public GameObject dialoguePanel;
    Text dialogueText;
    public Player player;
    Canvas canvas;
    public CanvasGroup faderCanvas;
    public float fadeTime;
    //ui elements
    public Text coinsText;

    CinemachineVirtualCamera activeDialogCamera;

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
        dialogueText = dialoguePanel.transform.GetChild(0).GetComponent<Text>();
        coinsText.text = "Skrilla : 0";
        faderCanvas.alpha = 0;
    }

    // Update is called once per frame
    public void toggleCanvas()
    {
        canvas.enabled = !canvas.enabled;
    }

    Sentence[] npcText;
    int currentNpcText;
    //this is more like NPCStartTalk


    Dialogue currentDialogue;
    public void NPCStartTalk(Sentence[] npcTextArg, Dialogue dialogue)
    {
        currentDialogue = dialogue;
        npcText = npcTextArg;
        dialoguePanel.GetComponent<Image>().enabled = true;
        dialogueRenderer.renderText(npcText[currentNpcText]);

        DialogCameraController.current.InitDialogCamera(currentDialogue);

        //DialogCameraController.current.checkCameraUpdate(currentDialogue, currentNpcText);
    }

    public void NPCNextTalk()
    {

        //check the dialogue and see if this is an index that has a cinemachine

        //if the letters have all been rendered out
        //MOVE TO THE NEXT BIT OF DIALOG


        if (dialogueRenderer.sentenceRendered)
        {

            currentNpcText++;
            if (currentNpcText < npcText.Length)
            {
                DialogCameraController.current.checkCameraUpdate(currentDialogue, currentNpcText);
                dialogueRenderer.renderText(npcText[currentNpcText]);
            }
            else
            {
                leaveDialogue();
            }
        }

        //render the rest of the dialog
        else
        {
            DialogCameraController.current.checkCameraUpdate(currentDialogue, currentNpcText);
            dialogueRenderer.renderRestOfLetters(npcText[currentNpcText], dialogueRenderer.letterIndex);
        }

    }

    public void leaveDialogue()
    {
        DialogCameraController.current.StopDialogCamera();

        currentNpcText = 0;
        player.inDialogue = false;

        //delete all text in the box
        dialogueRenderer.clearText();

        //dialoguePanel.SetActive(false);
        dialoguePanel.GetComponent<Image>().enabled = false;

    }

    public void updateCurrentTrack(Track newTrack)
    {
        trackArtistText.text = newTrack.artist;
        trackTitleText.text = newTrack.trackName;
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
        coinsText.text = "Skrilla: " + numCoins.ToString();
    }

    public void handleDialogChoices(int choice)
    {
        print("dialog option chosen " + choice.ToString());

        currentNpcText++;
        if (currentNpcText < npcText.Length)
        {
            dialogueRenderer.renderText(npcText[currentNpcText]);
        }
        else
        {
            leaveDialogue();
        }
    }
}
