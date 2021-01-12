using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBattleEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Track enemyTrack;
    public GameObject attackPanel;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartTurn()
    {
        //first thing we gotta do is switchup the song
        //for now we're just gonna do this right tf now

        RPGBattleManager.current.UpdateTrack(enemyTrack);

        //add a couple things to the buffer
        //first we're just gonna make an announcement

        //then on the next bar we turn on the attack panel for the enemy, we're gonna play basically the same ability too

        LightWeightTimeManager.current.messageBuffer.Add("Enemy used ability!");
        LightWeightTimeManager.current.messageFuncBuffer.Add(RPGBattleManager.current.MakeAnnouncement);
        LightWeightTimeManager.current.beatBuffer.Add(AttackBuffer);


    }

    public void AttackBuffer()
    {
        LightWeightTimeManager.current.barBuffer.Add(EnemyAttack);
    }



    //hack we're just using the same function as the player
    public AttackPanel attack;

    public void EnemyAttack()
    {

        attackPanel.SetActive(true);
        attack.EnemyAbility0();
    }




}
