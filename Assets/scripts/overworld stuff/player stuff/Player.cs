using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerRootCollider playerRoot;
    Rigidbody rb;

    public int maxHealth, health;
    public float speed;
    public InteractRange interactRange;
    public bool inBattle;

    float horizontalIn;
    float verticalIn;
    public bool inDialogue;

    [Range(1, 10)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.current.setManagerReferences(this);
        //testing this to see how it works, may remove later 
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame

    //input stuff 
    void Update()
    {
        //TODO: boi
        if (!inDialogue && !inBattle)
        {
            horizontalIn = Input.GetAxisRaw("Horizontal");
            verticalIn = Input.GetAxisRaw("Vertical");

            Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn) * speed * Time.deltaTime;

            rb.MovePosition(transform.position + deltaPos);

            if (Input.GetKeyDown(KeyCode.D))
            {
                flip(0);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                flip(180);
            }
        }
        GetInput();
    }

    private void FixedUpdate()
    {
        Vector3 deltaPos = new Vector3(horizontalIn, 0, verticalIn) * speed * Time.deltaTime;
        // transform.position = Vector3.MoveTowards(transform.position, transform.position + deltaPos, 0.2f);
        // transform.Translate(deltaPos);
    }

    void clearObjectsInRange()
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
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //clear the list of any missing game objects or fucked up references
            clearObjectsInRange();

            //check if we're already interacting with something 

            if (inDialogue)
            {
                //go to the next dialogue 
                if (UIManager.current == null)
                {
                    print("no ui manager");
                }
                UIManager.current.NPCNextTalk();
            }


            else if (interactRange.objectsInRange.Count > 0)
            {
                interactRange.objectsInRange[0].GetComponent<IInteractable>().Interact();

                if (interactRange.objectsInRange.Count > 0 && interactRange.objectsInRange[0].GetComponent<NPC>() != null)
                {
                    inDialogue = true;
                }
            }
        }


        //process the jump
        if (Input.GetButtonDown("Jump") && playerRoot.onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }

        //better jump code

        if (rb.velocity.y < 0)
        {

            rb.velocity += new Vector3(rb.velocity.x, Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime, rb.velocity.z);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {

            rb.velocity += new Vector3(rb.velocity.x, Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime, rb.velocity.z);
        }
    }


    void flip(float rotation)
    {
        StartCoroutine(LerpToRotation(rotation, 0.1f, 0.1f));
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

}
