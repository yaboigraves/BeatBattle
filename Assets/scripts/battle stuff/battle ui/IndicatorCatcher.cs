using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCatcher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            if (!BattleManager.current.playerTurn)
            {
                //print("player takes damage");
                BattleManager.current.playerTakeDamage(1);
                Destroy(other.gameObject);
            }
            else
            {
                //print("player loses swing");
                Destroy(other.gameObject);
            }
        }
    }
}
