using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Yarn.Unity;



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
    //Yarn Variables 
    public Text dialogTextContainer;

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

    public void ping()
    {
        print("ping");
    }

    [YarnCommand("commandPing")]
    public void commandPing()
    {
        print("command ping");
    }
}
