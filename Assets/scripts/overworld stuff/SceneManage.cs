using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Yarn.Unity;
public class SceneManage : MonoBehaviour
{
    //public Dictionary<Enemy, string> enemySceneDictionary;
    public List<Enemy> enemies;
    //stored for moving the player around
    public static SceneManage current;
    public Player player;
    public GameObject enemyInBattle;
    public bool inBattle;
    //pos for camera to return to after battle
    Vector3 cameraReturnPosition;
    public GameObject mainCamera;

    public DialogueRunner dialogueRunner;

    public string currentSceneName;

    //so this dictionary maps scene names to positions
    //basically if you've visited a room before we need to store the position you left it in
    //if you rejoin this room we ignore the playerspawn object and instead spawn you where this left you
    public Dictionary<string, Vector3> sceneSpawnPositions;

    void Awake()
    {
        if (current == null)
        {
            current = this;
            enemies = new List<Enemy>();
        }
        else
        {
            Destroy(this.gameObject);
        }

        sceneSpawnPositions = new Dictionary<string, Vector3>();
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    // Start is called before the first frame update

    //variables used for scene transition
    public int playerHealth, playerMaxHealth, enemyHealth, enemyMaxHealth;

    //enable/disable enemy movement while the battle scene is active
    void toggleEnemiesOn(bool on)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject != enemyInBattle)
            {
                //rather than setting them all the way inactive just disable their movement
                //enemy.gameObject.SetActive(on);
                enemy.GetComponent<EnemyMove>().enabled = on;
            }
        }
    }

    public void TransitionToBattle(GameObject enemy, Track battleTrack)
    {

        //lower the priority of the players camera

        inBattle = true;
        enemyInBattle = enemy;

        toggleEnemiesOn(false);
        //lock the player movement
        InputHandler.current.LockPlayerMovement(true);

        if (mainCamera == null)
        {
            print("shit no main camera component in scene manager");
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        UIManager.current.screenWipe("battle");
        cameraReturnPosition = mainCamera.transform.position;
        mainCamera.gameObject.SetActive(false);

        //pause the overworld(audio)
        playerHealth = player.health;
        playerMaxHealth = player.maxHealth;
        enemyHealth = enemy.GetComponent<Enemy>().health;
        enemyMaxHealth = enemy.GetComponent<Enemy>().maxHealth;

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);

        CameraManager.current.updatePlayerCameraPriority(-15);

        //tell the battleManager to set the enemy sprite to whatever sprite was collided with 

        TrackManager.current.UpdateCurrentTrack(battleTrack);
        TrackManager.current.inBattle = true;
        TrackManager.current.StopCurrentTrack();

        player.enterBattle();
        //turn off the main camera so it doesnt warp to follow the player into the battle
    }

    //TODO: this could probably take a battleResult object that tells us if the player won or not 
    public void LeaveBattle(bool playerWon)
    {



        //reset the players camera priority

        //reset the time scale 
        Time.timeScale = 1;

        //first tell the battleui manager to run its thing 
        UIManager.current.screenWipe("loading");
        Destroy(BattleCameraController.current.gameObject);
        inBattle = false;
        SceneManager.UnloadSceneAsync("BattleScene");

        toggleEnemiesOn(true);
        player.inBattle = false;

        mainCamera.gameObject.SetActive(true);
        mainCamera.transform.position = cameraReturnPosition;

        if (playerWon)
        {
            //todo: trigger death animation
            //TODO: reimpliment after multi enemies battle are back in
            // foreach (GameObject en in player.battleRangeChecker.enemiesInRange)
            // {
            //     Destroy(en);
            // }

            //player.battleRangeChecker.enemiesInRange.Clear();

            Destroy(enemyInBattle);
        }
        else
        {
            //TODO: consider what happens when player loses a battle
            //if the player loses then boot them back to their spawn position but leave this off for now
        }

        //emppty out the players enemies in range variable 

        TrackManager.current.inBattle = false;
        TrackManager.current.playRandomBackgroundTrack();

        //unlock player movement
        InputHandler.current.LockPlayerMovement(false);

        //update the players inventory for any used items 
        //GameManager.current.player.inventory.items = BattleUIManager.current.battleItems;

        //reset the priority of the player camera
        CameraManager.current.updatePlayerCameraPriority(15);


    }


    //TODO: rewrite this a bit
    public void loadLevel(string sceneName, Vector3 loadPosition)
    {

        sceneSpawnPositions[sceneName] = loadPosition;

        //wipe the screen
        if (UIManager.current == null)
        {
            print("shit no ui manager");
        }

        //trunctate the camera positiions 
        DialogCameraController.current.ClearDialogCameraPositions();


        UIManager.current.screenWipe("loading");

        SceneManager.LoadScene(sceneName);



        //spawnPlayer();

        currentSceneName = SceneManager.GetActiveScene().name;


    }

    public void spawnPlayer()
    {
        // player.interactRange.objectsInRange.Clear();
        //player.RoomTransition();
    }

    public void LoadOverworldMinigame()
    {
        
    }


}
