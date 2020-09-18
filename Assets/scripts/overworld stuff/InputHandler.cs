using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InputHandler : MonoBehaviour
{
    public float horizontalIn, verticalIn;
    public GameObject playerPrefab;
    public Player player;




    //player variables for movement restrictions 



    void Start()
    {

        player = GameManager.current.playerObj.GetComponentInChildren<Player>();
        if (player == null)
        {
            print("ERROR : INPUT HANDLER PLAYER REFERENCE BROKEN");
        }
    }
    // Update is called once per frame
    void Update()
    {



        // Remove all player control when we're in dialogue

        horizontalIn = Input.GetAxisRaw("Horizontal");
        verticalIn = Input.GetAxisRaw("Vertical");



        //if we're just running around the overworld

        //Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn) * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //TODO: this will probably cause alot of bugs later refactor this error
            //clear the list of any missing game objects or fucked up references
            player.clearObjectsInRange();


            if (player.inDialogue)
            {
                //go to the next dialogue 
                if (UIManager.current == null)
                {
                    print("no ui manager");
                }
                UIManager.current.NPCNextTalk();
            }

            else if (player.inShop)
            {
                //TODO: if the player pressed e in the shop probably just have them buy whatever thing they have selected
            }
            else if (player.interactRange.objectsInRange.Count > 0)
            {
                player.interactRange.objectsInRange[0].GetComponent<IInteractable>().Interact();

                if (player.interactRange.objectsInRange.Count > 0 && player.interactRange.objectsInRange[0].GetComponent<NPC>() != null)
                {

                    player.enterDialogue();
                    ResetInputAxis();
                }
            }
        }

        if ((!player.inDialogue && !player.inBattle) && !player.inShop)
        {
            //print("sending a move");
            Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn);
            //player.Move(deltaPos);
            player.inputMoveCommand(deltaPos);


            //check for flips
            if (Input.GetKeyDown(KeyCode.D))
            {
                player.flip(0);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                player.flip(180);
            }
        }


        //process the jump
        if (Input.GetButtonDown("Jump") && player.playerRoot.onGround && !player.inDialogue && !player.inShop && !player.inBattle)
        {
            // rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
            player.jump();

        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
                    GameManager.current.player.TalkToHomie();
                    player.inDialogue = true;
                    ResetInputAxis();
                }
                else
                {
                    UIManager.current.NPCNextTalk();
                }

            }
        }
    }

    public void ResetInputAxis()
    {
        horizontalIn = 0;
        verticalIn = 0;
        player.ResetDeltaPos();
    }
}
