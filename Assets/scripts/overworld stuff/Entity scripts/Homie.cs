using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class Homie : Entity
{
    NavMeshAgent navAgent;
    public Player player;
    public float stoppingDistance = 3, maxPlayerDistance = 15;

    public float acceleration, deceleration;

    //so this needs to get updated depending on where we are
    public YarnProgram currentDialogue;

    public string currentDialogueNode = "";

    public float speed, maxSpeed;

    bool rampingUp, rampingDown;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        //navAgent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();


        if (GameManager.current != null)
        {
            player = GameManager.current.player;

            if (currentDialogue != null)
            {
                DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
                if (!dialogueRunner.NodeExists(currentDialogueNode))
                {
                    dialogueRunner.Add(currentDialogue);
                }

            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        // if (Vector3.Distance(transform.position, player.transform.position) < stoppingDistance)
        // {
        //     rampingUp = false;
        //     if (!rampingDown)
        //     {
        //         StopAllCoroutines();
        //         StartCoroutine(rampSpeedDown());
        //         rampingDown = true;
        //     }


        // }
        // else
        // {
        //     rampingDown = false;
        //     if (!rampingUp)
        //     {
        //         StopAllCoroutines();
        //         StartCoroutine(rampSpeedUp());
        //         rampingUp = true;
        //     }

        // }

        // Vector3 direction = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        // rigidbody.MovePosition(direction);

        //going to try and rewrite this to be a little less spammy
        //dont really need any physics stuff so we're just gonna translate this bad boy

        if (Vector3.Distance(transform.position, player.transform.position) > maxPlayerDistance)
        {
            //teleport to the player
            transform.position = player.transform.position - Vector3.left;
        }




        //so if the distance between us and the player is larger than the stopping distance
        //1.ramp up the speed to move towards them if its not at its max already 

        //if the player is within range ramp the speed down

        if (Vector3.Distance(transform.position, player.transform.position) > stoppingDistance)
        {
            //check if our currentSpeed has ramped up to the correct speed

            if (speed <= maxSpeed)
            {
                speed = speed + acceleration * Time.deltaTime;
            }

            //figure out the direction we should be moving

        }
        else
        {
            if (speed > deceleration * Time.deltaTime)
            {
                speed = speed - deceleration * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }

        if (speed > 0)
        {

            Vector3 dir = (player.transform.position - transform.position).normalized;
            checkFlip(dir.x);
            dir.y = 0;
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }


    public void TalkToHomie()
    {
        FindObjectOfType<DialogueRunner>().StartDialogue(currentDialogueNode);
    }

    public void jump(Vector3 jumpForce)
    {
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    void checkFlip(float dX)
    {
        if (dX > 0)
        {
            //going right
            if (facingDirection != 1)
            {
                facingDirection = 1;
                StartCoroutine(LerpToScale(1, 0.1f, 0));
            }
        }
        else
        {
            if (facingDirection != -1)
            {
                facingDirection = -1;
                StartCoroutine(LerpToScale(-1, 0.1f, 0));
            }
        }
    }


    //if you get too far away
    public void TeleportToPlayer()
    {
        transform.position = player.transform.position;

    }
}