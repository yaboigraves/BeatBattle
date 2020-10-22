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
    public bool playerTurn;
    public BattleEnemy enemy;
    //battle ui obkjects 
    public Transform indicators;


    [Header("UI Stuff")]
    public GameObject indicatorContainer;
    public GameObject indicator;
    public GameObject testEnemy;
    public GameObject bar;
    public Gear testGear;
    public Item[] testItems;


    bool firstTurn = true;
    //this variable keeps track of whether or not the player or the enemy did the first attack
    //vibe bar stuff 
    public int vibe = 0;
    int maxVibe = 50, minVibe = -50;

    public float barsPerTurn = 2;




    //list of delegates for the gear effects
    List<GearEffects.GearEffect> equippedGearEffects = new List<GearEffects.GearEffect>();


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
            playerTracks = GameManager.current.player.inventory.battleEquippedTracks;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //check if we're in testmode (which means theirs no scene manager present)
        if (TrackManager.current == null || SceneManage.current == null)
        {
            //use the testing enemy and the backup track that was loaded in the battle track manager
            enemy.setEnemies(testEnemy, null);

            setPlayerEnemyHealth(10, 10, testEnemy.GetComponent<Enemy>().health, testEnemy.GetComponent<Enemy>().maxHealth);

            //turn on the testing gear effects

            BattleUIManager.current.CreateGearIcons(new List<Gear>() { testGear });
            equippedGearEffects.Add(GearEffects.sp404);

            List<Item> testItems = new List<Item>();
            foreach (Item i in this.testItems)
            {
                testItems.Add(i);
            }
            LoadItems(testItems);
        }
        else
        {
            //print(enemy);
            //spawn the enemy in and turn off their move function 
            enemy.setEnemies(SceneManage.current.enemyInBattle, GameManager.current.player.battleRangeChecker.enemiesInRange);
            //grab the equipped tracks of the player

            //grab the equipped gear from the player inventory
            LoadGear();
            LoadItems(GameManager.current.player.inventory.items);

            //load the items from our inventory
        }
        changeTurn();
        firstTurn = false;

        //load all the spectators for the battle
    }

    void LoadGear()
    {
        equippedGearEffects = GameManager.current.player.inventory.gearEffects;
        //load all the gear icons onto the ui

        BattleUIManager.current.CreateGearIcons(GameManager.current.player.inventory.equippedGear);
    }

    void LoadItems(List<Item> items)
    {
        //load the items from either the players inventory or load them from a testlist
        BattleUIManager.current.LoadItemsList(items);
    }


    GameObject lastIndicatorContainer;
    public void setupTurnIndicators(Track newTrack)
    {

        if (lastIndicatorContainer != null)
        {
            Destroy(lastIndicatorContainer);
        }

        Track track = newTrack;

        //TIMESCALE STUFF
        //so the uniform timescale is 60bpm. therefore the timescale we want is whatever the bpm of the curre

        Time.timeScale = newTrack.bpm / 60;

        GameObject indicContainer = Instantiate(indicatorContainer, Vector3.zero, Quaternion.identity, indicators);
        TrackTimeManager.current.currIndicatorContainer = indicContainer;

        //instantiate uhhh 4 bars of bars so 16 total

        for (int i = 0; i <= 16; i++)
        {
            GameObject _bar = Instantiate(bar, Vector3.up * (100 + i), Quaternion.identity, indicContainer.transform.GetChild(1));
        }



        for (int i = 0; i < track.kickBeats.indicatorPositions.Length; i++)
        {
            Vector3 kickPos = new Vector3(-1, 0 + 100 + (track.kickBeats.indicatorPositions[i]), 0);
            //each unit is 1 bar 
            //therefore we need to start the next batck of indicators at wherever the loop ends
            //probablyh easiest for now just to bake the length of the loop into the track object 
            Instantiate(indicator, kickPos, Quaternion.identity, indicContainer.transform.GetChild(0));

        }

        for (int i = 0; i < track.snareBeats.indicatorPositions.Length; i++)
        {
            Vector3 kickPos = new Vector3(1, 0 + 100 + (track.snareBeats.indicatorPositions[i]), 0);
            Instantiate(indicator, kickPos, Quaternion.identity, indicContainer.transform.GetChild(0));
        }

        lastIndicatorContainer = indicContainer;


    }

    // Update is called once per frame
    void Update()
    {
        //TODO: defer this to the inputhandler
        if (Input.GetKeyDown(KeyCode.Space) && !battleStarted)
        {
            //wait till after the countdown to set this

            StartBattle();
        }
    }

    void StartBattle()
    {
        battleStarted = true;
        playerTurn = true;
        //1 2 3 4 
        BattleTrackManager.current.StartCountIn();
    }

    //this should be passed to a battleUImanager object

    public void processPadHit(bool hit)
    {
        //hit is true 
        if (playerTurn)
        {
            if (hit)
            {
                enemyTakeDamage(1);
                currentStreak++;
                vibe += BattleTrackManager.current.currentTrack.trackStats.vibePerHit;

                //spawn a indicator number 
            }
            else
            {
                currentStreak = 0;
                //print("u missed lol");
            }

        }
        else
        {
            if (hit)
            {
                vibe += BattleTrackManager.current.currentTrack.trackStats.vibePerHit;

                currentStreak = 1;
                //u dont take damage
                //print("b");
            }
            else
            {
                currentStreak = 0;
                playerTakeDamage(1);
            }
        }
        BattleUIManager.current.UpdateVibe(vibe);
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

    //for now to check if we're buffed by the sp404 effect this is done manually, this needs to be abstracted

    public bool sp404Buff;
    public void enemyTakeDamage(int damage)
    {
        if (sp404Buff)
        {
            sp404Buff = false;
            damage *= 4;
            BattleUIManager.current.ToggleUiIconBorder("sp404", false);
        }

        //spawn a damage number from the ui manager
        BattleUIManager.current.CreateDamageNumber(damage);

        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            //TODO: when you reload into a new scene the scene manager current reference becomes broken



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
        print("ENDING THE BATTLE");
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
        playerTurn = !playerTurn;
        //first we check who's turn it is
        if (playerTurn)
        {
            //toggle the track selector 

            BattleUIManager.current.ToggleTrackSelectorOn(false);

            print("players turn");
            //so we need to find which track the player has selected
            //for now we just use the 0th position 
            BattleTrackManager.current.switchBattleTrack(BattleTrackManager.current.playerSelectedTrack, firstTurn);
            //IndicatorManager.current.changeIndicatorColors(new Color(255, 0, 0, 1));

            //indicatorContainer.GetComponent<Indicator>().UpdateColor(new Color(255, 0, 0));

        }
        else
        {

            BattleUIManager.current.ToggleTrackSelectorOn(true);

            print("enemies turn");
            BattleTrackManager.current.switchBattleTrack(BattleTrackManager.current.testEnemyTracks[0], firstTurn);
            //IndicatorManager.current.changeIndicatorColors(new Color(0, 0, 255, 1));
            //indicatorContainer.GetComponent<Indicator>().UpdateColor(new Color(0, 255, 0));
        }


        BattleCameraController.current.trackSwitcher(playerTurn);
    }



    public int currentStreak;

    public struct BattleState
    {
        public int beatStreak;

    }

    public void UpdateGearPipeline()
    {
        //so we need a list of delegates to run through, this list of delegates will be updated by equipping gear
        //each piece of gear needs to be passed stats about the current game state
        //for now we're passing 
        //-current streak of beats
        //-player health
        //-enemey health

        //so every time this is called we assemble all the neccessary info into a struct and send it off to the gear effects

        BattleState currentState;
        currentState.beatStreak = currentStreak;

        foreach (GearEffects.GearEffect gearEffect in equippedGearEffects)
        {
            gearEffect(currentState);
        }
    }


    //this is called every beat, if we're above 25 or under -25 vibe we take 1 damage every beat
    public void VibeUpdate()
    {
        if (vibe > 25)
        {
            playerTakeDamage(1);
        }

        else if (vibe < -25)
        {
            playerTakeDamage(1);
        }
    }


}
