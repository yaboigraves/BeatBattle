using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

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

    void Awake()
    {
        current = this;
        enemies = new List<Enemy>();

    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
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
                enemy.gameObject.SetActive(on);
            }

        }
    }

    public void TransitionToBattle(GameObject enemy, Track battleTrack)
    {
        inBattle = true;
        enemyInBattle = enemy;
        //TODO: LEFT OFF HERE, this causes the enemy in the battle to spawn deactivated, need to fix this
        toggleEnemiesOn(false);

        if (mainCamera == null)
        {
            print("shit no main camera component in scene manager");
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        UIManager.current.screenWipe();
        cameraReturnPosition = mainCamera.transform.position;
        mainCamera.gameObject.SetActive(false);


        //pause the overworld(audio)
        playerHealth = player.health;
        playerMaxHealth = player.maxHealth;
        enemyHealth = enemy.GetComponent<Enemy>().health;
        enemyMaxHealth = enemy.GetComponent<Enemy>().maxHealth;

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
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
        UIManager.current.screenWipe();
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
            Destroy(enemyInBattle.gameObject);
        }
        else
        {
            //TODO: consider what happens when player loses a battle
            //if the player loses then boot them back to their spawn position but leave this off for now
        }

        TrackManager.current.inBattle = false;

        TrackManager.current.playRandomBackgroundTrack();
    }

    public void loadInterior(string sceneName)
    {
        //wipe the screen
        if (UIManager.current == null)
        {
            print("shit no ui manager");
        }

        UIManager.current.screenWipe();
        spawnPlayer();
        SceneManager.LoadScene(sceneName);
    }

    public void spawnPlayer()
    {
        player.interactRange.objectsInRange.Clear();
        player.transform.position = GameObject.FindGameObjectWithTag("playerSpawn").transform.position;
    }
}
