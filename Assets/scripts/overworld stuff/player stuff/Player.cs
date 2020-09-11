using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem footDust;
    public PlayerRootCollider playerRoot;
    public Rigidbody rb;

    public int maxHealth, health;
    public float speed;
    public InteractRange interactRange;
    public bool inBattle;
    float horizontalIn;
    float verticalIn;
    public bool inDialogue, inShop;

    [Range(1, 100)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float gravityScale = 1;
    public float globalGravity = -1.81f;
    Vector3 deltaPos;
    public Transform spriteHolder;

    public BattleRangeChecker battleRangeChecker;

    public PlayerInventory inventory;


    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.current.setManagerReferences(this);
        //testing this to see how it works, may remove later 
        DontDestroyOnLoad(this.gameObject);
        rb.useGravity = false;

        //look for a playerspawner tag and go there
        Transform playerSpawnPoint = GameObject.FindGameObjectWithTag("playerSpawn").transform;

        battleRangeChecker = GetComponentInChildren<BattleRangeChecker>();

        inventory = GetComponent<PlayerInventory>();
    }


    public void inputMoveCommand(Vector3 deltaPos)
    {
        this.deltaPos = deltaPos;
    }

    public void Move(Vector3 deltaPos)
    {
        deltaPos *= (speed * Time.deltaTime);
        rb.MovePosition(transform.position + deltaPos);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + deltaPos, 0.1f);
    }

    public void enterDialogue()
    {
        inDialogue = true;
        deltaPos = Vector3.zero;
    }

    public void leaveDialogue()
    {
        inDialogue = false;
    }

    public void enterBattle()
    {
        inBattle = true;
        deltaPos = Vector3.zero;
    }


    private void FixedUpdate()
    {

        Move(deltaPos);

        //better jump code
        if (rb.velocity.y < 0)
        {
            //rb.velocity += new Vector3(rb.velocity.x, Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime, rb.velocity.z);
            gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //rb.velocity += new Vector3(rb.velocity.x, Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime, rb.velocity.z);
            gravityScale = lowJumpMultiplier;
        }
        else
        {
            gravityScale = 1;
        }

        //custom gravity
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
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
    public void jump()
    {
        rb.AddForce(new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z), ForceMode.Impulse);
        CreateDust();
    }

    public void flip(float rotation)
    {
        StartCoroutine(LerpToRotation(rotation, 0.1f, 0.1f));
        CreateDust();
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
            //fix this to apply the rotation rather than manually setting rotation
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, endRotation, 0f);
    }

    void CreateDust()
    {
        footDust.Play();
    }
}
