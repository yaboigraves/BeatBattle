using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InputHandler : MonoBehaviour
{
    public float horizontalIn, verticalIn;
    public GameObject playerPrefab;

    //note: we cant actually do this player variable here the reference might get broke
    //just look to the gaem manager
    public Player player;
    PlayerMove playerMove;

    //tracks whether the player can move or not
    public bool playerMovementLocked;

    public static InputHandler current;

    public bool inPickupCutscene;

    PlayerInteractionManager playerInteract;

    //so we need some way of basically cycling what you're about to interact with
    //maybe hitting e to cycle and then holding e to interact

    //we can basically handle this by just swapping shit around to the first index in the array

    //TODO: Refactor this whole bitch with a command interface


    void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //player variables for movement restrictions 

    void Start()
    {
        //player = GameManager.current.playerObj.GetComponentInChildren<Player>();
        if (player == null)
        {
            print("ERROR : INPUT HANDLER PLAYER REFERENCE BROKEN");
        }

        playerMove = player.gameObject.GetComponent<PlayerMove>();

        playerInteract = player.GetComponentInChildren<PlayerInteractionManager>();
    }

    void Update()
    {
        // Remove all player control when we're in dialogue

        horizontalIn = Input.GetAxisRaw("Horizontal");
        verticalIn = Input.GetAxisRaw("Vertical");

        //if we're just running around the overworld

        //Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn) * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && !player.inBattle)
        {
            //TODO: this will probably cause alot of bugs later refactor this error
            //clear the list of any missing game objects or fucked up references
            player.clearObjectsInRange();

            //Debug.Log("Trying to handle an E input");

            //Debug.Log(player.interactRange.objectsInRange.Count);
            if (player.inDialogue)
            {
                //go to the next dialogue 
                if (UIManager.current == null)
                {
                    print("no ui manager");
                }
                UIManager.current.NPCNextTalk();
            }
            else if (inPickupCutscene)
            {
                //TODO: rewrite for new cutscene manager 
                //CutsceneManager.current.EndPickupItemCutscene();
            }

            else if (player.inShop)
            {
                //TODO: if the player pressed e in the shop probably just have them buy whatever thing they have selected
            }
            else if (player.interactRange.objectsInRange.Count > 0)
            {

                //so when you try and interact with an npc, not all npc's are interactable
                //we need to enter you into the state only if interacting is actually possible

                //there are also multiple different interaction types, opening doors, cycling through things


                //bool interact = player.interactRange.objectsInRange[0].GetComponent<IInteractable>().Interact();

                playerInteract.InteractWithSelected();

                //TODO: so this needs to be based on whatever is curently selected, pass this along to the interactableRange

                if (player.interactRange.objectsInRange.Count > 0 && player.interactRange.objectsInRange[0].GetComponent<NPC>() != null)
                {
                    player.enterDialogue();
                    Debug.Log("entering conversation");
                    ResetInputAxis();
                }

            }
        }

        // if ((!player.inDialogue && !player.inBattle) && !player.inShop)
        if (!playerMovementLocked)
        {
            //print("sending a move");
            Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn);
            //player.Move(deltaPos);
            //playerMove.inputMoveCommand(deltaPos);

            //check for flips
            //TODO: fix this so its uniform
            if (Input.GetKeyDown(KeyCode.D))
            {
                player.spriteManager.flip(1f);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                player.spriteManager.flip(-1f);
            }
        }


        //process the jump
        // if (Input.GetButtonDown("Jump") && player.playerRoot.onGround && !player.inDialogue && !player.inShop && !player.inBattle)
        if (Input.GetButtonDown("Jump") && player.playerRoot.onGround && !playerMovementLocked)
        {
            // rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
            //playerMove.jump();

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            //check if we're in dialogue
            if (player.inDialogue)
            {

            }
            else if (player.inShop)
            {
                UIManager.current.CloseShop();
            }
            else
            {
                //activate the inventory menu
                UIManager.current.ToggleInventoryMenu();
                Time.timeScale = Mathf.Abs(Time.timeScale - 1);
            }


        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UIManager.current.ToggleDebugWindow();
        }


        //talk to the homie 

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!player.inBattle && !player.inShop && !player.inBattle)
            {
                //talk to the homie
                if (!player.inDialogue)
                {
                    //GameManager.current.player.TalkToHomie();
                    //player.inDialogue = true;
                    //ResetInputAxis();
                }
                else
                {
                    // UIManager.current.NPCNextTalk();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            print("TESTING MANAGERS");
            print(GameManager.current);
            print(UIManager.current);
            print(SceneManage.current);
            print(TrackManager.current);
            print("DONE TESTING");
        }

        //radio hacking
        if (Input.GetKeyDown(KeyCode.X) && !player.inBattle && !player.inDialogue)
        {
            //enter hack mode or leave hack mode
            if (player.inHack)
            {
                LockPlayerMovement(false);
            }
            else
            {
                LockPlayerMovement(true);
                //so now we need to have the camera shoot a ray out, this can probably be handled by a hacking component
            }

            player.inHack = !player.inHack;
            player.ToggleHackMode(player.inHack);
        }
    }

    public void ResetInputAxis()
    {
        horizontalIn = 0;
        verticalIn = 0;

        //playerMove.ResetDeltaPos();
    }

    public void LockPlayerMovement(bool lockMove)
    {
        //reset the input axis
        ResetInputAxis();
        playerMovementLocked = lockMove;
        playerMove.LockPlayerMovement(lockMove);
    }

    public void TogglePickupCutscene(bool toggle)
    {
        LockPlayerMovement(toggle);
        inPickupCutscene = toggle;
    }
}
