using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCatcher : MonoBehaviour
{
    // Start is called before the first frame update
    public float shrinkSpeed = 2;
    private void Update()
    {
        if (transform.localScale.x > 1)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            transform.localScale = Vector3.one * 1.5f;
            Destroy(other.gameObject);
        }

    }

}