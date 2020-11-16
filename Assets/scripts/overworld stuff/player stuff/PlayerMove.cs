using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    float horizontalIn;
    float verticalIn;
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float gravityScale = 1;
    public float globalGravity = -1.81f;

    Player player;
    Vector3 deltaPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    public void Move(Vector3 deltaPos)
    {
        deltaPos *= (speed * Time.fixedDeltaTime);
        rb.MovePosition(transform.position + deltaPos);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + deltaPos, 0.1f);
    }

    public void inputMoveCommand(Vector3 deltaPos)
    {
        this.deltaPos = deltaPos;
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



    public void jump()
    {
        rb.AddForce(new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z), ForceMode.Impulse);
        player.CreateDust();

        //also tell the homie to jump
        player.homie.jump(new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z));
    }



}
