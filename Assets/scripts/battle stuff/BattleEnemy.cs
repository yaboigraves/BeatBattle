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


    public void setEnemy(GameObject enemy)
    {
        //instnatiate a clone and set it to just float
        GameObject enemySprite = Instantiate(enemy, transform);
        enemySprite.transform.position = transform.position;
        enemySprite.GetComponent<EnemyMove>().enabled = false;
        enemySprite.GetComponent<Collider>().enabled = true;
    }

}
