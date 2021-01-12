using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] attackPanels;
    public Button[] buttons;

    public Image demonstrationIndicator;
    public Button playerInteractButton;

    public PlayerRecorder playerRecorder;

    public void ActivatePanel(int i)
    {
        //disable all the buttons
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }

        switch (i)
        {
            case 0:

                //LightWeightTimeManager.current.beatBuffer.Add(Ability0); //beat buffer (next beat)
                LightWeightTimeManager.current.messageBuffer.Add("Player used ability 0!");
                LightWeightTimeManager.current.messageFuncBuffer.Add(RPGBattleManager.current.MakeAnnouncement);

                LightWeightTimeManager.current.barBuffer.Add(Ability0); //bar buffer(next 1)
                break;
            case 1:
                LightWeightTimeManager.current.beatBuffer.Add(Ability0);
                break;
            case 2:
                LightWeightTimeManager.current.beatBuffer.Add(Ability0);
                break;
            case 3:
                LightWeightTimeManager.current.beatBuffer.Add(Ability0);
                break;
        }
    }

    public void EnablePlayerButtons()
    {
        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
    }
    public void Ability0()
    {

        //turn on the bar visualizer
        playerRecorder.barVisualizer.gameObject.SetActive(true);
        playerRecorder.barVisualizer.InitIndicators();

        //make an announcement
        float[] beats = new float[] { 2, 4 };
        attackPanels[0].SetActive(true);
        //so this is basically just simon says
        //for now just pre program a bar of 1 beat hits u need to follow
        //basic idea 
        //store list of beats you need to hit [2,4]

        //1.demonstrate these to the player over one bar
        //so we need to queue up stuff in the beat que that can happen after a certain amount of time
        //create a new list that stores a tuple 

        LightWeightTimeManager.current.longBeatBuffer.Add((2, LightUpPad));
        LightWeightTimeManager.current.longBeatBuffer.Add((3, LightUpPad));
        LightWeightTimeManager.current.longBeatBuffer.Add((4, TriggerPlayerRecording));
        LightWeightTimeManager.current.longBeatBuffer.Add((8, EndPlayerRecording));
        LightWeightTimeManager.current.longBeatBuffer.Add((10, RPGBattleManager.current.EndPlayerTurn));


        //2.record the player input over the next bar and accumulate how much damage is done depending on the amount of correct timings
        //3.display some damage to the enemy
        //flip to enemies turn

    }

    List<float> beats = new List<float> { 1, 2 };

    public void EnemyAbility0()
    {

        //turn on the bar visualizer
        playerRecorder.barVisualizer.gameObject.SetActive(true);
        playerRecorder.barVisualizer.InitIndicators();

        attackPanels[0].SetActive(true);

        LightWeightTimeManager.current.longBeatBuffer.Add((2, LightUpPad));
        LightWeightTimeManager.current.longBeatBuffer.Add((3, LightUpPad));
        LightWeightTimeManager.current.longBeatBuffer.Add((4, TriggerPlayerRecording));
        LightWeightTimeManager.current.longBeatBuffer.Add((8, EndPlayerRecording));
        LightWeightTimeManager.current.longBeatBuffer.Add((10, RPGBattleManager.current.EndEnemyTurn));

    }

    public void LightUpPad()
    {
        //just turn the pad on with a coroutine (this is bad for now but dont worry about it)
        StartCoroutine(lightUpRoutine());
    }

    //so this function triggers the player recording phase

    public void TriggerPlayerRecording()
    {
        playerInteractButton.interactable = true;
        playerInteractButton.GetComponent<Image>().color = Color.red;
        playerRecorder.StartRecord(new List<float> { 1, 3 });

    }

    public void EndPlayerRecording()
    {
        playerInteractButton.interactable = false;
        playerInteractButton.GetComponent<Image>().color = Color.grey;
        //turn off all the attack panels 

        foreach (GameObject panel in attackPanels)
        {
            panel.SetActive(false);
        }

        playerRecorder.StopRecord();
    }

    IEnumerator lightUpRoutine()
    {
        demonstrationIndicator.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        demonstrationIndicator.color = Color.grey;

        //light up the indicator for half a beat0
    }



    public void Ability1()
    {
        attackPanels[1].SetActive(true);

    }
    public void Ability2()
    {
        attackPanels[2].SetActive(true);
    }
    public void Ability3()
    {
        attackPanels[3].SetActive(true);
    }
    public void DeactivatePanel()
    {

    }
}
