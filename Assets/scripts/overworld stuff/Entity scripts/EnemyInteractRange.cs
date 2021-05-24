using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractRange : MonoBehaviour
{
    bool playerInRange;
    EnemyMove enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyMove>();
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.chasePlayer(true, other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.chasePlayer(false, other.gameObject.transform);
        }
    }
}
