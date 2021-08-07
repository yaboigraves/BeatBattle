using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public ParticleSystem footDust;
    public PlayerRootCollider playerRoot;
    public Rigidbody rb;
    public int maxHealth, health;
    public PlayerInteractionManager interactRange;
    public bool inBattle;
    float horizontalIn;
    float verticalIn;
    public bool inDialogue, inShop, inHack;
    Vector3 deltaPos;
    public Transform spriteHolder;
    public BattleRangeChecker battleRangeChecker;
    public PlayerInventory inventory;
    // Start is called before the first frame update
    public GameObject homieObj;
    public Homie homie;
    public Cinemachine.CinemachineVirtualCamera pickupItemCam;

    PlayerHack playerHack;

    private void Awake()
    {
        interactRange = GetComponentInChildren<PlayerInteractionManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.current.setManagerReferences(this);
        //testing this to see how it works, may remove later 
        //DontDestroyOnLoad(this.gameObject);
        //rb.useGravity = false;

        //look for a playerspawner tag and go there
        Transform playerSpawnPoint = GameObject.FindGameObjectWithTag("playerSpawn").transform;

        battleRangeChecker = GetComponentInChildren<BattleRangeChecker>();

        inventory = GetComponent<PlayerInventory>();
        //LoadHomie();

        playerHack = GetComponent<PlayerHack>();
    }

    public void LoadHomie()
    {
        //print("creating homie");
        homie = Instantiate(homieObj, transform.position + Vector3.left * 2, Quaternion.identity).GetComponent<Homie>();
    }

    public void TalkToHomie()
    {
        homie.TalkToHomie();
        InputHandler.current.LockPlayerMovement(true);
    }

    public void enterDialogue()
    {
        inDialogue = true;
        InputHandler.current.LockPlayerMovement(true);

    }

    public void leaveDialogue()
    {
        inDialogue = false;
        //InputHandler.current.LockPlayerMovement(false);
        DialogCameraController.current.LeaveDialogue();
    }

    public void enterBattle()
    {
        inBattle = true;
        InputHandler.current.LockPlayerMovement(true);
    }


    public void clearObjectsInRange()
    {
        for (int i = 0; i < interactRange.objectsInRange.Count; i++)
        {
            if (interactRange.objectsInRange[i] == null)
            {
                interactRange.objectsInRange.RemoveAt(i);
            }
        }
    }

    //controls stuff 
    // public void jump()
    // {
    //     rb.AddForce(new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z), ForceMode.Impulse);
    //     CreateDust();

    //     //also tell the homie to jump
    //     homie.jump(new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z));
    // }

    public void flip(float rotation)
    {
        // StartCoroutine(LerpToRotation(rotation, 0.1f, 0.1f));
        //StartCoroutine(LerpToScale(rotation, 0.1f, 0.1f));
        CreateDust();
    }


    public void CreateDust()
    {
        footDust.Play();
    }

    public void ResetDeltaPos()
    {
        deltaPos = Vector3.zero;
    }




    //saving/loading functions 
    public PlayerData savePlayerData()
    {
        PlayerData playerData = new PlayerData();
        //temp for now
        playerData.playerName = "Yancey";
        playerData.playerHealth = health;
        playerData.playerMaxHealth = maxHealth;

        playerData.skrillaCount = inventory.coins;

        //get the names of all the battle tracks
        string[] trackNames = new string[inventory.battleEquippedTracks.Length];

        for (int i = 0; i < inventory.battleEquippedTracks.Length; i++)
        {
            if (inventory.battleEquippedTracks[i] != null)
            {
                trackNames[i] = inventory.battleEquippedTracks[i].itemName;
            }
        }

        //get the names of all the gear equipped

        string[] gearNames = new string[inventory.equippedGear.Count];

        for (int i = 0; i < inventory.equippedGear.Count; i++)
        {
            gearNames[i] = inventory.equippedGear[i].itemName;
        }

        playerData.tracks = trackNames;
        playerData.gear = gearNames;

        return playerData;
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        health = playerData.playerHealth;
        maxHealth = playerData.playerMaxHealth;
        inventory.coins = playerData.skrillaCount;

        if (playerData.tracks != null)
        {
            foreach (string trackName in playerData.tracks)
            {
                if (trackName != null)
                {
                    inventory.LoadItemByName(trackName);
                }
            }
        }
        //equip all the tracks from the playerDAta.tracks 

        //loop through all the gear from t

        if (playerData.gear != null)
        {
            foreach (string gearName in playerData.gear)
            {
                inventory.LoadItemByName(gearName);
            }
        }
    }


    //run this when moving between rooms (reloads stuff figures out which door we should go to)
    public void RoomTransition()
    {
        //interactRange.objectsInRange.Clear();
        //transform.position = GameObject.FindGameObjectWithTag("playerSpawn").transform.position;
    }

    public void ToggleHackMode(bool toggle)
    {
        playerHack.ToggleHack(toggle);
        inHack = toggle;
    }

    public void SetHackSong(int song)
    {
        playerHack.SetRadioSong(song);
    }
}
