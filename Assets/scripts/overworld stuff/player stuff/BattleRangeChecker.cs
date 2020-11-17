using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleRangeChecker : MonoBehaviour
{
    [Header("Keeps track of how many enemies will be pulled into the battle")]
    public List<GameObject> enemiesInRange;
    private void Start()
    {

        enemiesInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Enemy>() != null)
        {

            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Enemy>() != null && enemiesInRange.Contains(other.gameObject))
        {

            enemiesInRange.Remove(other.gameObject);
        }
    }
}
