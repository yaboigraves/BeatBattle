using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CutsceneEntity : MonoBehaviour
{
    public string entityName;
    public GameObject obj;
    public Transform[] wayPoints;
    public int currentWaypoint = 0;

    public void moveToWayPoint()
    {
        StartCoroutine(moveEntity());
    }




    IEnumerator moveEntity()
    {

        while (Vector3.Distance(transform.position, wayPoints[currentWaypoint].position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentWaypoint].position, 0.2f);
            yield return new WaitForEndOfFrame();
        }
    }
}
