using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class Homie : MonoBehaviour
{
    NavMeshAgent navAgent;
    public Player player;
    public float stoppingDistance = 3;

    //so this needs to get updated depending on where we are
    public YarnProgram currentDialogue;

    public string currentDialogueNode = "";


    public float speed, maxSpeed;

    bool rampingUp, rampingDown;

    // Start is called before the first frame update
    void Start()
    {

        navAgent = GetComponent<NavMeshAgent>();

        if (GameManager.current != null)
        {
            player = GameManager.current.player;

            if (currentDialogue != null)
            {
                DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
                dialogueRunner.Add(currentDialogue);
            }
        }


    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < stoppingDistance)
        {
            rampingUp = false;
            if (!rampingDown)
            {
                StopAllCoroutines();
                StartCoroutine(rampSpeedDown());
                rampingDown = true;
            }


        }
        else
        {
            rampingDown = false;
            if (!rampingUp)
            {
                StopAllCoroutines();
                StartCoroutine(rampSpeedUp());
                rampingUp = true;
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);


    }

    IEnumerator rampSpeedUp()
    {
        while (speed < maxSpeed)
        {
            yield return new WaitForSeconds(0.1f);
            speed += 0.1f;
        }
    }

    IEnumerator rampSpeedDown()
    {
        while (speed > 0)
        {
            yield return new WaitForSeconds(0.1f);
            speed -= 0.1f;
        }
        speed = 0;
    }

    public void TalkToHomie()
    {
        FindObjectOfType<DialogueRunner>().StartDialogue(currentDialogueNode);


    }
}
