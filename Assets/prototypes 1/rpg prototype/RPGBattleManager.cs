using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPGBattleManager : MonoBehaviour
{
    //general outline of what we need here
    //-start the battle and a track plays
    //-when you make an input depending on what ability you use a minigame will pop up (maybe the track effects this too)
    //-minigame will wait to start until the next 1 beat
    //-after you do your minigame it prompts the enemy to take an action which creates a defense minigame for you to do
    //-if you do your minigame flawlessly you combo into a new minigame on a new track, but you gotta adapt for the bpm switchup on the fly

    public Track battleTrack;
    AudioSource audioSource;
    public static RPGBattleManager current;
    public RPGBattlePlayer player;
    public RPGBattleEnemy enemy;

    public Camera cam;

    public Color playerTurnColor, enemyTurnColor;

    public TextMeshProUGUI announcementText;

    public AttackPanel attackPanel;

    public int playerHealth = 120, enemyHealth = 60;

    public TextMeshProUGUI playerHealthText, enemyHealthText;

    public bool isPlayerTurn = true;

    public GameObject combatTextPrefab;
    public Transform combatTextHolder;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //first things first we start playin the track
        playerHealthText.text = playerHealth.ToString();
        enemyHealthText.text = enemyHealth.ToString();

        StartPlayerTurn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartPlayerTurn()
    {
        isPlayerTurn = true;
        audioSource.clip = battleTrack.trackClip;
        audioSource.Play();
        LightWeightTimeManager.current.SetCurrentDSPStartTime(battleTrack);

        attackPanel.EnablePlayerButtons();



        player.StartTurn();
        cam.backgroundColor = playerTurnColor;
    }

    public void EndPlayerTurn()
    {
        //re turn off the active ability panel
        player.EndTurn();

        //plug starting the enemy turn into the bar buffer
        LightWeightTimeManager.current.barBuffer.Add(StartEnemyTurn);
    }

    public void StartEnemyTurn()
    {
        isPlayerTurn = false;
        cam.backgroundColor = enemyTurnColor;

        //so the enemies turn will start with 1 bar of just showing you the bpm of the new song 
        //a name of the ability the enemy is using will display for 1 bar
        //after the display bar (which lets you get the bpm down) we then open the panel and start a quick time

        //for now just use the same like simon says system

        enemy.StartTurn();
    }

    public void EndEnemyTurn()
    {
        //still player cause they use the same panel
        player.EndTurn();
        LightWeightTimeManager.current.barBuffer.Add(StartPlayerTurn);
    }

    public void MakeAnnouncement(string announcement)
    {
        announcementText.transform.parent.gameObject.SetActive(true);
        announcementText.text = announcement;
        //wait till the next bar to turn it off
        LightWeightTimeManager.current.barBuffer.Add(StopAnnouncement);
    }

    public void StopAnnouncement()
    {
        announcementText.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateTrack(Track newTrack)
    {
        audioSource.Stop();
        audioSource.clip = newTrack.trackClip;
        audioSource.Play();
        LightWeightTimeManager.current.SetCurrentDSPStartTime(newTrack);
    }

    //hit status's are either 
    //1-full hit 
    //0.5-half hit
    //0-miss
    public void ProcessHit(float hitStatus)
    {




        print(hitStatus);
        if (hitStatus == 1)
        {
            if (isPlayerTurn)
            {

                enemyHealth -= 5;
                enemyHealthText.transform.gameObject.GetComponent<Pulse>().pulse();
                SpawnCombatText("Hit!");
            }
            else
            {
                //no penalty for blocking perfectly
                SpawnCombatText("Block!");
            }
        }
        else if (hitStatus == 0.5f)
        {
            if (isPlayerTurn)
            {
                enemyHealth -= 3;
                enemyHealthText.transform.gameObject.GetComponent<Pulse>().pulse();
                SpawnCombatText("Off Hit!");
            }
            else
            {
                playerHealth -= 3;
                playerHealthText.transform.gameObject.GetComponent<Pulse>().pulse();
                SpawnCombatText("Ouch!");
            }
        }
        else
        {
            if (isPlayerTurn)
            {
                //1 damage penalty
                playerHealth -= 1;
                playerHealthText.transform.gameObject.GetComponent<Pulse>().pulse();
                SpawnCombatText("Miss!");
            }

            else
            {
                playerHealth -= 5;
                playerHealthText.transform.gameObject.GetComponent<Pulse>().pulse();
                SpawnCombatText("Ouch!!!");
            }
        }

        updateHealthTexts();
    }

    public void updateHealthTexts()
    {
        playerHealthText.text = playerHealth.ToString();
        enemyHealthText.text = enemyHealth.ToString();
    }

    public void SpawnCombatText(string text)
    {
        GameObject combatText = Instantiate(combatTextPrefab, combatTextHolder.transform.position, Quaternion.identity, combatTextHolder.transform);
        combatText.GetComponent<TextMeshProUGUI>().text = text;
    }

}
