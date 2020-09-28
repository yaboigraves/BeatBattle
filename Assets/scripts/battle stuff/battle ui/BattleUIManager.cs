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
    public GameObject[] trackSelectButtons;

    public Slider enemyHealthSlider;

    public Slider playerHealthSlider;

    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BattleEndUI.SetActive(false);

        //setup the buttons for the players track options
        InitTrackButtons();

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

    public void UpdateMetronome(int currentBeat, bool playAudio)
    {
        metronomeText.text = (currentBeat + 1).ToString();
        if (playAudio)
        {
            audioSource.clip = metronomeCounts[currentBeat];
            audioSource.Play();
        }
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
    }


    public GameObject damageNumber;
    public GameObject damageNumberContainer;
    public void CreateDamageNumber(int damage)
    {
        //find the screenspace position that the ui element needs to be spawned at 
        //for now just gonna spawn it at the enemies healthbar 
        DamageNumber dmg = Instantiate(damageNumber, enemyHealthSlider.transform.position + (Vector3.up * 40), Quaternion.identity, damageNumberContainer.transform).GetComponent<DamageNumber>();
        dmg.setDamage(damage);

    }


    //battle icon shit


    public GameObject itemIcon, gearIconContainer;
    public void CreateGearIcons(List<Gear> gears)
    {
        foreach (Gear gear in gears)
        {
            UIIcon gearIcon = Instantiate(itemIcon, gearIconContainer.transform.position, Quaternion.identity, gearIconContainer.transform).GetComponent<UIIcon>();
            gearIcon.SetIconItem(gear);
        }

    }
}
