using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{

    public GameObject selectedIcon;
    //raise the image over the radio
    public void Select(bool toggle)
    {
        selectedIcon.SetActive(toggle);
    }


}
