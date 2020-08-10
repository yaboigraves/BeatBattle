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

    public bool playerTurn;

    public BattleEnemy enemy;


    //battle ui obkjects 

    public Transform indicators;
    public GameObject kickIndicator;



    public GameObject testEnemy;

    void Awake()
    {
        current = this;
        // TrackManager.current.currAudio.Pause();

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

        if (SceneManage.current == null)
        {
            //use the testing enemy and the backup track that was loaded in the battle track manager
            enemy.setEnemy(testEnemy);
            SetupIndicators(BattleTrackManager.current.currentTrack);

            setPlayerEnemyHealth(10, 10, testEnemy.GetComponent<Enemy>().health, testEnemy.GetComponent<Enemy>().maxHealth);

        }

        else
        {
            //spawn the enemy in and turn off their move function 
            enemy.setEnemy(SceneManage.current.enemyInBattle);

            SetupIndicators(TrackManager.current.currTrack);
        }

        changeTurn();
    }

    //TODO: this needs to just take in a string for the trackname, it then needs to look into the folder with all the json objects 
    void SetupIndicators(Track track)
    {
        //creates 5 loops
        for (int x = 0; x < 5; x++)
        {
            //get the current audio tracks shit

            for (int i = 0; i < track.kickBeats.Count; i++)
            {
                //TODO: fix this distribution, probably needs to be like trackbars * x + beats[i]

                Vector3 kickPos = new Vector3(-1, (x * track.numBars * 4) + 100 + (track.kickBeats[i]), 0);
                //each unit is 1 bar 
                //therefore we need to start the next batck of indicators at wherever the loop ends
                //probablyh easiest for now just to bake the length of the loop into the track object 

                Instantiate(kickIndicator, kickPos, Quaternion.identity, indicators);

            }

            for (int i = 0; i < track.snareBeats.Count; i++)
            {
                Vector3 kickPos = new Vector3(1, (x * track.numBars * 4) + 100 + (track.snareBeats[i]), 0);
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
            battleStarted = true;
            StartBattle();
        }

        //check for enemy death
    }

    void StartBattle()
    {
        //TrackManager.current.UnPauseTrack();
        //TrackManager.current.StartBattle();
        //for now this just plays the song but this is going to get moved to a battletrack manager

        playerTurn = true;
        BattleTrackManager.current.StartBattle();


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
        playerHealth -= damage;

        if (playerHealth <= 0)
        {

            //check if we're in test mode if we are just reload the scene

            if (SceneManage.current == null)
            {
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
            else
            {
                SceneManage.current.LeaveBattle();
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
                //unload the battle scene (do this after a fade or something next time)
                SceneManage.current.LeaveBattle();
                //theres gonna be a lot more steps here, going to need to stop the battle song, start another one, and destroy the enemy
                TrackManager.current.playRandomBackgroundTrack();
            }
        }
        BattleUIManager.current.updateEnemyHealth(enemyHealth);
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
