using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    public static BattleManager current;
    public bool battleStarted;
    //maybe move these somewhere but honestly doesnt need to be in the player 
    public int playerHealth, enemyHealth, playerMaxHealth, enemyMaxHealth;
    public Track[] playerTracks;
    public bool playerTurn = true;
    public BattleEnemy enemy;
    //battle ui obkjects 
    public Transform indicators;
    public GameObject kickIndicator;
    public GameObject testEnemy;

    bool firstTurn = true;

    //this variable keeps track of whether or not the player or the enemy did the first attack

    //REVAMP NOTES
    /*
       1. figure out who's turn it is 
       2. setup the track audio via the battle track manager 
       3. setup the currentTrack variables 
       4. setup the indicators starting from 5 away from the current beat
       5. play 
       6. on a turn swap repoeat from step 2 

    */
    void Awake()
    {
        current = this;
        //this is for debugging, if theres no track manager that means launc shit in test mode
        if (TrackManager.current == null)
        {
            //TODO: load some kind of presets so testing the battles is easier

        }
        else
        {
            TrackManager.current.PauseTrack();
            //find info for battle to start from the scene manager 
            setPlayerEnemyHealth(SceneManage.current.playerHealth, SceneManage.current.playerMaxHealth, SceneManage.current.enemyHealth, SceneManage.current.enemyMaxHealth);
            playerTracks = GameManager.current.player.GetComponent<PlayerInventory>().playerTracks;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //check if we're in testmode (which means theirs no scene manager present)
        if (TrackManager.current == null)
        {
            //use the testing enemy and the backup track that was loaded in the battle track manager
            enemy.setEnemies(testEnemy, null);
            //SetupIndicators(BattleTrackManager.current.testPlayerTracks[0]);
            setPlayerEnemyHealth(10, 10, testEnemy.GetComponent<Enemy>().health, testEnemy.GetComponent<Enemy>().maxHealth);
        }
        else
        {
            //spawn the enemy in and turn off their move function 
            enemy.setEnemies(SceneManage.current.enemyInBattle, GameManager.current.player.battleRangeChecker.enemiesInRange);
            //SetupIndicators(TrackManager.current.currTrack);
            //grab the equipped tracks of the player

        }
        changeTurn();
        firstTurn = false;
    }

    //TODO: spawn everything 1 bar on top of 
    void SetupIndicators(Track track)
    {
        //creates 5 loops
        for (int x = 0; x < 1; x++)
        {
            for (int i = 0; i < track.kickBeats.indicatorPositions.Length; i++)
            {
                Vector3 kickPos = new Vector3(-1, 4 + (x * track.numBars * 4) + 100 + (track.kickBeats.indicatorPositions[i]), 0);
                //each unit is 1 bar 
                //therefore we need to start the next batck of indicators at wherever the loop ends
                //probablyh easiest for now just to bake the length of the loop into the track object 

                Instantiate(kickIndicator, kickPos, Quaternion.identity, indicators);

            }

            for (int i = 0; i < track.snareBeats.indicatorPositions.Length; i++)
            {
                Vector3 kickPos = new Vector3(1, 4 + (x * track.numBars * 4) + 100 + (track.snareBeats.indicatorPositions[i]), 0);
                Instantiate(kickIndicator, kickPos, Quaternion.identity, indicators);
            }
        }

    }

    public void setupTurnIndicators(Track newTrack)
    {
        Track track = newTrack;
        //for now this will just use the testing track
        //BattleTrackManager.current.totalBeats tells us what the current beat is, so when we want to setup the next turns indicaotrs
        //we look at the currentbeat + 4 for where we should start initialization

        for (int i = 0; i < track.kickBeats.indicatorPositions.Length; i++)
        {

            Vector3 kickPos = new Vector3(-1, 4 + 100 + (track.kickBeats.indicatorPositions[i]), 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            Instantiate(kickIndicator, kickPos, Quaternion.identity, indicators);

        }

        for (int i = 0; i < track.snareBeats.indicatorPositions.Length; i++)
        {
            Vector3 kickPos = new Vector3(1, 4 + 100 + (track.snareBeats.indicatorPositions[i]), 0);
            Instantiate(kickIndicator, kickPos, Quaternion.identity, indicators);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: defer this to the inputhandler
        if (Input.GetKeyDown(KeyCode.Space) && !battleStarted)
        {
            //wait till after the countdown to set this
            battleStarted = true;
            StartBattle();
        }
    }

    void StartBattle()
    {
        playerTurn = true;
        //1 2 3 4 
        BattleTrackManager.current.StartCountIn();
    }

    public void processPadHit(bool hit)
    {
        //hit is true 
        if (playerTurn)
        {
            if (hit)
            {
                enemyTakeDamage(1);
            }
            else
            {
                //print("u missed lol");
            }

        }
        else
        {
            if (hit)
            {
                //u dont take damage
                //print("block");
            }
            else
            {
                playerTakeDamage(1);
            }
        }
    }

    public void playerTakeDamage(int damage)
    {

        if (playerHealth <= 0)
        {

            //check if we're in test mode if we are just reload the scene

            if (SceneManage.current == null)
            {
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
            else
            {
                //TODO: end battle sequence

                BattleUIManager.current.EndBattleSequence(false);
                TrackManager.current.playRandomBackgroundTrack();
                playerHealth = playerMaxHealth;
            }

        }
        BattleUIManager.current.updatePlayerHealth(playerHealth);
    }

    public void enemyTakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            if (SceneManage.current == null)
            {
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
            else
            {
                StartStopBattle(true);
            }
        }
        BattleUIManager.current.updateEnemyHealth(enemyHealth);
    }

    public void StartStopBattle(bool playerWon)
    {
        //tell the ui manager to display the win text
        BattleUIManager.current.EndBattleSequence(true);

        //turn off the scrolling
        battleStarted = false;

        //tell the track to turn off for now
        BattleTrackManager.current.StopBattle();
    }

    public void EndStopBattle(bool playerWon)
    {
        SceneManage.current.LeaveBattle(playerWon);
    }

    public void setPlayerEnemyHealth(int playerHealthArg, int playerMaxHealthArg, int enemyHealthArg, int enemyMaxHealthArg)
    {
        playerHealth = playerHealthArg;
        enemyHealth = enemyHealthArg;
        playerMaxHealth = playerMaxHealthArg;
        enemyMaxHealth = enemyMaxHealthArg;

        BattleUIManager.current.setMaxHealths(playerMaxHealth, enemyMaxHealth);
        BattleUIManager.current.updatePlayerHealth(playerHealth);
        BattleUIManager.current.updateEnemyHealth(enemyHealth);
    }


    //TODO: This is a little too unwieldy right now rewrite this at some point
    public void changeTurn()
    {

        //first we check who's turn it is
        if (playerTurn)
        {
            print("players turn");
            //so we need to find which track the player has selected
            //for now we just use the 0th position 
            BattleTrackManager.current.switchBattleTrack(BattleTrackManager.current.playerSelectedTrack, firstTurn);
            IndicatorManager.current.changeIndicatorColors(new Color(255, 0, 0, 1));

        }
        else
        {
            print("enemies turn");
            BattleTrackManager.current.switchBattleTrack(BattleTrackManager.current.testEnemyTracks[0], firstTurn);
            IndicatorManager.current.changeIndicatorColors(new Color(0, 0, 255, 1));
        }

        playerTurn = !playerTurn;
        BattleCameraController.current.trackSwitcher();
    }
}
