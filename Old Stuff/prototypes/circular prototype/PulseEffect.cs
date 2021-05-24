using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PulseEffect : MonoBehaviour
{

    // Update is called once per frame
    Vector3 originalSize;
    public float shrinkSpeed;
    private void Start()
    {
        originalSize = transform.localScale;
    }
    void Update()
    {
        if (transform.localScale.x > originalSize.x)
        {
            transform.localScale -= (transform.localScale * Time.deltaTime * shrinkSpeed);
        }
    }

    public void TriggerPulse(Vector3 newSize)
    {
        transform.localScale = newSize;
    }
}


