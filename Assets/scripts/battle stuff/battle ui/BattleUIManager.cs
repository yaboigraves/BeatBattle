using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager current;
    public int maxPlayerHealth, maxEnemyHealth;

    public TextMeshProUGUI metronomeText, playerHealthText, enemyHealthText;

    [Header("UI Sounds")]

    AudioSource audioSource;
    public AudioClip[] metronomeCounts;

    public GameObject BattleEndUI;

    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BattleEndUI.SetActive(false);
    }

    public void setMaxHealths(int mPHealth, int mEHealth)
    {
        maxPlayerHealth = mPHealth;
        maxEnemyHealth = mEHealth;
    }

    public void updatePlayerHealth(int newHealth)
    {
        playerHealthText.text = newHealth.ToString() + "/" + maxPlayerHealth.ToString();
    }

    public void updateEnemyHealth(int newHealth)
    {
        enemyHealthText.text = newHealth.ToString() + "/" + maxEnemyHealth.ToString();
    }

    public void UpdateMetronome(int currentBeat, bool playAudio)
    {
        metronomeText.text = (currentBeat + 1).ToString();

        if (playAudio)
        {
            audioSource.clip = metronomeCounts[currentBeat];
            audioSource.Play();
        }
    }


    public void EndBattleSequence(bool playerWon)
    {
        //so we also n
        StartCoroutine(endBattleRoutine(playerWon));
        BattleEndUI.SetActive(true);
    }



    IEnumerator endBattleRoutine(bool playerWon)
    {
        print("beep boop end");
        yield return new WaitForSeconds(3);
        BattleManager.current.EndStopBattle(playerWon);
    }
}
