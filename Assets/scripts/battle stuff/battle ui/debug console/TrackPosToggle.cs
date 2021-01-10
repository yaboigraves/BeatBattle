using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrackPosToggle : MonoBehaviour
{
    public Transform lane;
    TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        if (lane.transform.position.x == -3)
        {
            dropdown.value = 0;
        }
        if (lane.transform.position.x == -1)
        {
            dropdown.value = 1;
        }
        if (lane.transform.position.x == 1)
        {
            dropdown.value = 2;
        }
        if (lane.transform.position.x == 3)
        {
            dropdown.value = 3;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLanePos(int choice)
    {
        if (choice == 0)
        {
            choice = -3;
        }
        else if (choice == 1)
        {
            choice = -1;
        }
        else if (choice == 2)
        {
            choice = 1;
        }
        else if (choice == 3)
        {
            choice = 3;
        }
        Vector3 lanePos = new Vector3(choice, 99, 0);
        lane.transform.position = lanePos;
    }
}
