using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneManage : MonoBehaviour
{
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
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    // Start is called before the first frame update

    //variables used for scene transition
    public int playerHealth, playerMaxHealth, enemyHealth, enemyMaxHealth;

    public void TransitionToBattle(GameObject enemy, Track battleTrack)
    {

        if (mainCamera == null)
        {
            print("shit no main camera component in scene manager");
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        UIManager.current.screenWipe();
        cameraReturnPosition = mainCamera.transform.position;
        mainCamera.gameObject.SetActive(false);
        print(battleTrack);

        inBattle = true;
        enemyInBattle = enemy;

        //pause the overworld(audio)
        playerHealth = player.health;
        playerMaxHealth = player.maxHealth;
        enemyHealth = enemy.GetComponent<Enemy>().health;
        enemyMaxHealth = enemy.GetComponent<Enemy>().maxHealth;

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        //tell the battleManager to set the enemy sprite to whatever sprite was collided with 

        TrackManager.current.UpdateCurrentTrack(battleTrack);
        TrackManager.current.inBattle = true;

        player.inBattle = true;
        //turn off the main camera so it doesnt warp to follow the player into the battle
    }

    public void LeaveBattle()
    {
        UIManager.current.screenWipe();
        Destroy(BattleCameraController.current.gameObject);
        inBattle = false;
        SceneManager.UnloadSceneAsync("BattleScene");
        player.inBattle = false;

        mainCamera.gameObject.SetActive(true);
        mainCamera.transform.position = cameraReturnPosition;
        //todo: trigger death animation
        Destroy(enemyInBattle.gameObject);

        TrackManager.current.inBattle = false;
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
    }
}
