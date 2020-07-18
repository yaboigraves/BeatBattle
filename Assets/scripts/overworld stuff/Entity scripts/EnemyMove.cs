using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    //references the animator and triggers movement animations
    Animator animator;

    public Transform playerPos;
    public bool chasingPlayer, idle;
    public float speed;

    public Transform[] patrolRoute;
    public float patrolTolerance = 0.3f;
    int currentPatrol = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (chasingPlayer && !SceneManage.current.inBattle)
        {

            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);

            //check where player is so we know when to rotate sprite 

            if (transform.position.x > playerPos.position.x)
            {
                //to the right of player 
                if (transform.rotation.y == 0)
                {
                    StartCoroutine(LerpToRotation(180, 0.1f, 0.1f));
                }
            }
            else
            {
                if (transform.rotation.y - 180 <= 0.001f)
                {
                    StartCoroutine(LerpToRotation(0, 0.1f, 0.1f));
                }
            }
        }

        //if we're not chasing the player then we just patrol to the next patrol point
        else if (!idle)
        {
            animator.SetBool("isMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, patrolRoute[currentPatrol].position, speed * Time.deltaTime);
            //if we get within a certain tolerance value of the position
            if (Mathf.Abs(Vector3.Distance(transform.position, patrolRoute[currentPatrol].position)) < patrolTolerance)
            {
                //stand still for a sec, then this coroutine calls the goToNextWaypoint function
                StartCoroutine(standIdly());
            }
        }
    }

    private void goToNextWaypoint()
    {
        currentPatrol = (currentPatrol + 1) % patrolRoute.Length;

        //check the x position of the next waypoint and see if we need to flip the sprite
        if (transform.position.x > patrolRoute[currentPatrol].position.x)
        {

            StartCoroutine(LerpToRotation(180, 0.1f, 0.1f));
        }
        else
        {

            StartCoroutine(LerpToRotation(0, 0.1f, 0.1f));
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
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        idle = false;

        //then start walking again to the next waypoint 
        goToNextWaypoint();
    }

    IEnumerator LerpToRotation(float endRotation, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startRotation = transform.rotation.eulerAngles.y;
        float lerpRotation = startRotation;

        float i = 0f;
        float rate = 1 / time;
        while (i <= 1)
        {
            i += Time.deltaTime * rate;

            lerpRotation = Mathf.Lerp(startRotation, endRotation, i);
            transform.rotation = Quaternion.Euler(0f, lerpRotation, 0f);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, endRotation, 0f);
    }
}
