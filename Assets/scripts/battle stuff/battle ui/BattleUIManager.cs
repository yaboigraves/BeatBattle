using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager current;

    public Text playerHealthText, enemyHealthText;

    public int maxPlayerHealth, maxEnemyHealth;


    private void Awake()
    {
        current = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        // maxPlayerHealth = BattleManager.current.playerMaxHealth;
        // maxEnemyHealth = BattleManager.current.enemyMaxHealth;
    }

    public void setMaxHealths(int mPHealth, int mEHealth)
    {
        maxPlayerHealth = mPHealth;
        maxEnemyHealth = mEHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updatePlayerHealth(int newHealth)
    {
        print("max pl he");
        print(maxPlayerHealth);
        playerHealthText.text = newHealth.ToString() + "/" + maxPlayerHealth.ToString();
    }

    public void updateEnemyHealth(int newHealth)
    {
        enemyHealthText.text = newHealth.ToString() + "/" + maxEnemyHealth.ToString();
    }




}
