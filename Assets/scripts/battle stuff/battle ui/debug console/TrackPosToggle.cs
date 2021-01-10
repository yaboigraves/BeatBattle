using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosToggle : MonoBehaviour
{
    public Transform lane;
    // Start is called before the first frame update
    void Start()
    {

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
