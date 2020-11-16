using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractIndicator : MonoBehaviour
{

    public Image indicator;

    private void Start()
    {
        indicator = transform.GetComponentInChildren<Image>();
    }

    void ToggleIndicator(bool toggle)
    {
        if (toggle)
        {
            indicator.CrossFadeAlpha(1, 1, true);
        }
        else
        {
            indicator.CrossFadeAlpha(0, 1, true);

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //enable the indicator
            ToggleIndicator(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //enable the indicator
            ToggleIndicator(false);

        }
    }
}
