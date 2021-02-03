using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    /*
        --battle rewrite notes--
        
        okie dokie so yea time to do this shit i suppose
        so first thing to do is add two different battle modes
        longmix mode (probably for bosses)
        quickmix mode (the mashup mode )

        longmix mode 
            -one long track initiated
                *no dynamic track instantiation or bpm switchups

        quickmix mode
            -8/16 beats of a song will be initiated followed by a transition
            -following the transition another 8/16 beats will be instantiated (rest will be dynamic after that)

            -instead of trying to dynamically use the same audio source setup 3 audio channels in the track manager
                -mix1 (left track)
                -transitions
                -mix2 (right track)


    */

    public static BattleManager current;

    public enum BattleType
    {
        longMix,
        quickMix
    };




    public BattleType battleType;
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
    //int maxVibe = 50, minVibe = -50;

    public float barsPerTurn = 2, barsPerTransition = 1;

    //list of delegates for the gear effects
    List<GearEffects.GearEffect> equippedGearEffects = new List<GearEffects.GearEffect>();

    AudioSource soundFxAudioSource;

    public AudioClip[] fuckupSounds;

    //Important
    //mix1,mix2,transition
    public string battlePhase = "mix1", lastMix = "";

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

        soundFxAudioSource = GetComponent<AudioSource>();
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
        //non test mode
        else
        {
            //print(enemy);
            //spawn the enemy in and turn off their move function 
            enemy.setEnemies(SceneManage.current.enemyInBattle, GameManager.current.player.battleRangeChecker.enemiesInRange);
            //grab the equipped tracks of the player

            //grab the equipped gear from the player inventory
            LoadGear();
            LoadItems(GameManager.current.player.inventory.items);
        }


        setupBattle();
        firstTurn = false;
    }

    public void SetBattlePhase(string newPhase)
    {
        if (newPhase == "transition")
        {
            lastMix = battlePhase;
        }
        battlePhase = newPhase;
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


    // Update is called once per frame
    void Update()
    {
        //TODO: defer this to the inputhandler
        if (Input.GetKeyDown(KeyCode.Space) && !battleStarted)
        {
            //wait till after the countdown to set this

            StartBattle();
        }


        //tell the track time manager to update 
        TrackTimeManager.ManualUpdate();
    }

    void StartBattle()
    {
        battleStarted = true;
        playerTurn = true;
        //1 2 3 4 

        if (battleType == BattleType.quickMix)
        {
            BattleTrackManager.current.StartQuickMixBattle();
        }
        else if (battleType == BattleType.longMix)
        {
            //add later :)
        }

        //BattleTrackManager.current.StartCountIn();
    }

    //this should be passed to a battleUImanager object

    public void processPadHit(bool hit, int padIndex)
    {

        //hit is true 
        if (playerTurn)
        {
            if (hit)
            {
                //enemyTakeDamage(1);
                currentStreak++;
                //vibe += BattleTrackManager.current.currentTrack.trackStats.vibePerHit;
                vibe += 1;
                BattleUIManager.current.SpawnHitText(padIndex, "GOOD!", Color.green);


                //spawn a indicator number 
            }
            else
            {
                currentStreak = 0;
                //print("u missed lol");
                vibe -= 1;
                //play the fuckup sound
                BattleUIManager.current.SpawnHitText(padIndex, "FUCK!", Color.red);
                PlayFuckupSound();
            }

        }
        else
        {
            if (hit)
            {
                //vibe += BattleTrackManager.current.currentTrack.trackStats.vibePerHit;
                vibe += 1;
                currentStreak = 1;
                BattleUIManager.current.SpawnHitText(padIndex, "GOOD!", Color.green);
                //u dont take damage
                //print("b");
            }
            else
            {
                currentStreak = 0;
                //playerTakeDamage(1);
                vibe--;
                BattleUIManager.current.SpawnHitText(padIndex, "FUCK!", Color.red);
                //play the fuckup sound
                PlayFuckupSound();


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
    public void setupBattle()
    {

        //rewrite starts here 

        BattleUIManager.current.ToggleTrackSelectorOn(false);

        //check which type of mix it is 
        if (battleType == BattleType.longMix)
        {

        }
        else if (battleType == BattleType.quickMix)
        {
            print("loading quick mix");

            //things to do for loading a quick mix 
            //1.track manager needs to setup a queue of songs (for now the same song with a transition between them)

            BattleTrackManager.current.setupQuickMix();

            //2.once thats set up, the track manager tells the indicator manager to setup the whole que for now 
            //3.wait for an input to start then we good
        }

        BattleCameraController.current.trackSwitcher(playerTurn);


        // if (playerTurn)
        // {
        //     //toggle the track selector 

        //     BattleUIManager.current.ToggleTrackSelectorOn(false);

        //     print("players turn");
        //     //so we need to find which track the player has selected
        //     //for now we just use the 0th position 

        //     //if we're in longmix mode set the battle track here 

        //     if (battleType == BattleType.longMix)
        //     {
        //         BattleTrackManager.current.setBattleTrack(BattleTrackManager.current.playerSelectedTrack, firstTurn);

        //     }
        //     else if (battleType == BattleType.quickMix)
        //     {

        //     }
        // }
        // else
        // {

        //     BattleUIManager.current.ToggleTrackSelectorOn(true);

        //     print("enemies turn");
        //     BattleTrackManager.current.setBattleTrack(BattleTrackManager.current.testEnemyTracks[0], firstTurn);
        // }

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

    public void ResetVibe()
    {
        this.vibe = 0;
    }

    //TODO: theres some random bs other functions with similar names/functions maybe kill this later but its mostly for debug
    public void StopBattle()
    {
        ResetVibe();
        battleStarted = false;
        BattleTrackManager.current.StopBattle();
    }

    public void PlayFuckupSound()
    {
        //randomly pick an audioclip from the list to play

        //maybe randomize the pitch

        soundFxAudioSource.PlayOneShot(fuckupSounds[Random.Range(0, fuckupSounds.Length - 1)], soundFxAudioSource.volume);
    }

    public void DoTransition(string newPhase)
    {
        /*
        
        TODO:definitly going to need to create a queue of track objects that are going to be played for both mixes
        TODO:definitly going to need to package the track objects with a list of possible transitions

        so the phases we can possibly go into are 
            -mix1
                -start playing mix1's new track

                -fade the transition track out
                
            -mix2
                -start playing mix2s new track
                -fade the transition out 

            -transition
                -store what the last tracked we played before the transition was in lastBattlePhase
                -fade out the last midis track
                -play a transition from the last track
        */


    }


}
