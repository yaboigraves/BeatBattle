using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0, vertical = 0;
        if (Input.GetKey(KeyCode.U))
        {
            vertical = 1;
        }
        if (Input.GetKey(KeyCode.H))
        {
            horizontal = -1;
        }
        if (Input.GetKey(KeyCode.J))
        {
            vertical = -1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            horizontal = 1;
        }

        Vector3 rotate = new Vector3(horizontal, vertical, 0);

        transform.Rotate(rotate * rotateSpeed * Time.deltaTime);

    }
}
