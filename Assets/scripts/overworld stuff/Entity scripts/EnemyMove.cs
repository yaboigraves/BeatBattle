using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : Entity
{
    //TODO: rewriting this to use the navmesh agent

    //references the animator and triggers movement animations
    Animator animator;
    public Transform playerPos;
    public bool chasingPlayer, idle;
    public float speed;
    public Transform[] patrolRoute;
    public float patrolTolerance = 0.3f;
    int currentPatrol = 0;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (patrolRoute.Length > 1)
        {
            agent.SetDestination(patrolRoute[currentPatrol].position);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: hacky short fix
        if (!SceneManage.current.inBattle)
        {
            if (agent.isOnNavMesh && chasingPlayer && !SceneManage.current.inBattle)
            {
                //transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
                agent.SetDestination(playerPos.position);
                //check where player is so we know when to rotate sprite 

                //TODO: refactor this to a seperate function (sprite flips should be basically a function we can call)
                CheckForRotation(playerPos.position);
            }

            //if we're not chasing the player then we just patrol to the next patrol point
            else if (!idle)
            {
                animator.SetBool("isMoving", true);
                if (agent.isOnNavMesh && agent.remainingDistance < agent.stoppingDistance)
                {
                    //we need to go to the next spot in the patrol route

                    StartCoroutine(standIdly());

                    // goToNextWaypoint();
                }
            }
        }
    }

    private void goToNextWaypoint()
    {
        currentPatrol = (currentPatrol + 1) % patrolRoute.Length;
        agent.SetDestination(patrolRoute[currentPatrol].position);

        //check the x position of the next waypoint and see if we need to flip the sprite
        CheckForRotation(patrolRoute[currentPatrol].position);
    }

    public void CheckForRotation(Vector3 targetLookPos)
    {
        if (transform.position.x > targetLookPos.x)
        {
            //to the right of player 
            if (transform.rotation.y == 0)
            {
                StartCoroutine(LerpToRotation(180, 0.1f, 0.1f));
            }
        }

        else
        {
            //to the left of the player
            if (transform.rotation.y - 180 <= 0.001f)
            {
                StartCoroutine(LerpToRotation(0, 0.1f, 0.1f));
            }
        }
    }

    public void chasePlayer(bool chasing, Transform player)
    {
        idle = false;
        chasingPlayer = chasing;
        playerPos = player;
        animator.SetBool("isMoving", chasing);
    }

    IEnumerator standIdly()
    {
        idle = true;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        idle = false;
        //then start walking again to the next waypoint 

        if (patrolRoute.Length > 0)
        {
            goToNextWaypoint();
        }

    }

    // IEnumerator LerpToRotation(float endRotation, float time, float delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     float startRotation = transform.rotation.eulerAngles.y;
    //     float lerpRotation = startRotation;

    //     float i = 0f;
    //     float rate = 1 / time;
    //     while (i <= 1)
    //     {
    //         i += Time.deltaTime * rate;

    //         lerpRotation = Mathf.Lerp(startRotation, endRotation, i);
    //         transform.rotation = Quaternion.Euler(0f, lerpRotation, 0f);
    //         yield return null;
    //     }
    //     transform.rotation = Quaternion.Euler(0f, endRotation, 0f);
    // }
}
