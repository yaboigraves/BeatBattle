using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour, IActivateable
{
    // Start is called before the first frame update

    public float speed = 1f;
    public float delta = 3f;
    Rigidbody rigidbody;

    //should be able to move between all waypoitns
    public Transform[] wayPoints;
    int currentWaypoint = 0;

    private void Start()
    {
        enabled = false;

        //resize waypoints and add the origin as the first spot 

        //System.Array.Resize(ref wayPoints, wayPoints.Length + 1);
        //currentWaypoint = wayPoints.Length - 1;

        //wayPoints[currentWaypoint] = transform;
        //currentWaypoint = 0;
    }

    public void Activate()
    {
        enabled = !enabled;
    }

    // Update is called once per frame
    void Update()
    {
        //check distance to current waypoint
        if (Vector3.Distance(transform.position, wayPoints[currentWaypoint].position) < 0.01f)
        {
            currentWaypoint++;

            if (currentWaypoint >= wayPoints.Length)
            {
                currentWaypoint = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentWaypoint].position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerRootCollider>() != null)
        {
            //this means the players root collider(feet) touched the platform
            //set the player 
            other.gameObject.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerRootCollider>() != null)
        {
            //this means the players root collider(feet) touched the platform
            //set the player 
            other.gameObject.transform.parent.SetParent(null);
            DontDestroyOnLoad(other.gameObject.transform.parent);
        }
    }
}
