using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemy : MonoBehaviour
{
    //so basically this lad just manages animation triggers for the enemy as well as the instantation for the sprite and animations 

    //subsequent child classes can add enemy abilities and stuff, this class will also hold enemy health and stuff so the 
    //game manager doesnt have to, and this will also be extendable hopefully to multiple enemies in a battle

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    //this takes in the enemy you collided with which is the main 
    public void setEnemies(GameObject enemy, List<GameObject> enemiesInRange)
    {
        //remove the enemy that we collided with from the list so that it doesnt get instantiated twice 
        if (enemiesInRange != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }

        //instnatiate a clone and set it to just float
        GameObject enemySprite = Instantiate(enemy, transform);
        enemySprite.transform.position = transform.position;
        enemySprite.GetComponent<EnemyMove>().enabled = false;
        enemySprite.GetComponent<Collider>().enabled = true;

        //do the same thing for all the other lads 

        if (enemiesInRange != null)
        {
            Vector3 offset = new Vector3(1, 0, 1);

            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                GameObject en = Instantiate(enemiesInRange[i], transform.position + (offset * (i + 1)), Quaternion.identity, transform);
                en.GetComponent<EnemyMove>().enabled = false;
                en.GetComponent<Collider>().enabled = true;
            }

        }
    }
}
