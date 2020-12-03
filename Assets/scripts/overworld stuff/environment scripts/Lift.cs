using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour, IActivateable
{
    // Start is called before the first frame update

    public float speed = 1f;
    public float delta = 3f;

    private void Start()
    {
        enabled = false;
    }

    public void Activate()
    {
        enabled = !enabled;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(speed * Time.time, delta);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;
    }
}
