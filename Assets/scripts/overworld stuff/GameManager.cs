using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{

    //todo: manage spawning of managers
    //the managers should not be tied to a particular scene, and if they are tie them to this one object
    //game manager will spawn all the other managers
    public GameObject playerObj;
    public static GameManager current;
    public Player player;
    InputHandler input;


    private void Awake()
    {
        if (current != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            current = this;
            DontDestroyOnLoad(this.gameObject);

            if (player == null)
            {
                //IMPORTANT: this needs to be done because the player object referenced in the inputhandler needs
                //to actually be the INSTANCE of the prefab object NOT the prefab itself
                //all this does is replace the referenced prefab with this particular INSTANCE of the prefab
                //playerObj = Instantiate(playerObj);

                //tell the orbit camera heres the player



                //move the player to a spawner if one exists
                Transform spawnPos = GameObject.FindGameObjectWithTag("playerSpawn").transform;

                if (spawnPos)
                {
                    playerObj.transform.position = spawnPos.position;
                }


            }
            input = GetComponent<InputHandler>();
        }
    }

    private void Start()
    {
        //load shit
        StartCoroutine(LateStart());
    }



    public void setManagerReferences(Player player)
    {
        this.player = player;
        UIManager.current.player = player;
        SceneManage.current.player = player;
        //CameraManager.current.setCameraFollow(player.transform);

    }


    //this basically just runs after start so think of it as latestart or something 
    public IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate();

        SaveManager.LoadSettings();

        GameStateData data = SaveManager.loadGame();

        //TODO: later make a save select screen where we load this shit from
        if (data == null)
        {
            print("creating data");
            SaveManager.saveGame();
        }

        else
        {
            // print(data.playerData.playerName);
            //dispense loading jobs to various managers
            //loading the level should be done via the save select screen (save select -> loading menu -> scene)

            //load the players stats (health money gear and stuff)
            player.LoadPlayerData(data.playerData);

            //dispense all the story variables
            //load all the story variables into yarns variable storage

            foreach (QuestData q in data.storyData.questList.quests)
            {
                UIManager.current.dialogueRunner.variableStorage.SetValue(q.questName, q.questStatus);
            }

        }


        //maybe send out a big global init to everyone telling them to update now
        UIManager.current.InitUIManager();


    }


    //testing saving/loading stuff 
    //PlayerPrefs.Set("key",value)
    //stored like a json and then you can PlayerPrefs.Get() shit too

    // public void SaveDataTest()
    // {
    //     //so for now lets save that the tracks the player equips'

    //     PlayerPrefs.SetString("EquippedTrack0", "CuntyBlap");

    //     for (int i = 0; i < 1; i++)
    //     {
    //         string trackName = PlayerPrefs.GetString("EquippedTrack" + i.ToString());
    //         //then find that track by its name and equip them 
    //         player.inventory.LoadItemByName(trackName);
    //     }
    // }




}
