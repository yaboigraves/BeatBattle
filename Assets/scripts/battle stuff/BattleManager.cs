using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{


    //REVAMP NOTES
    /*
        once user hits play, the beats start rolling and a 1 2 3 4 countdown is done in the bpm of the current track
        1.the indicators should be offset by one bar
        2.1 2 3 4 countdown needs to be implemented and synced to bpm (probably a recursive coroutine)
        
        after the battle is over there needs to be a short delay where the enemies defeated and then 
        show how much kush we got and how much xp we got

        
        

    */



    public static BattleManager current;
    public bool battleStarted;
    //maybe move these somewhere but honestly doesnt need to be in the player 
    public int playerHealth, enemyHealth, playerMaxHealth, enemyMaxHealth;
    public bool playerTurn;
    public BattleEnemy enemy;
    //battle ui obkjects 
    public Transform indicators;
    public GameObject kickIndicator;
    public GameObject testEnemy;

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
            SetupIndicators(BattleTrackManager.current.testTrack);

            setPlayerEnemyHealth(10, 10, testEnemy.GetComponent<Enemy>().health, testEnemy.GetComponent<Enemy>().maxHealth);
        }
        else
        {
            //spawn the enemy in and turn off their move function 
            enemy.setEnemies(SceneManage.current.enemyInBattle, GameManager.current.player.battleRangeChecker.enemiesInRange);

            SetupIndicators(TrackManager.current.currTrack);
        }
        changeTurn();
    }

    //TODO: spawn everything 1 bar on top of 
    void SetupIndicators(Track track)
    {
        //creates 5 loops
        for (int x = 0; x < 5; x++)
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


        //playerHealth -= damage;

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

    public void changeTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            IndicatorManager.current.changeIndicatorColors(new Color(255, 0, 0, 1));
        }
        else
        {
            IndicatorManager.current.changeIndicatorColors(new Color(0, 0, 255, 1));
        }
        BattleCameraController.current.trackSwitcher();
    }
}
