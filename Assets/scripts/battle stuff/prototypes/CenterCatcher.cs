using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCatcher : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            Destroy(other.gameObject);
        }

    }

}