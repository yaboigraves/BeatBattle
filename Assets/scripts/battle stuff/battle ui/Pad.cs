using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public GameObject indicator;
    public string padID;

    public KeyCode binding;
    public int midiKeyNum;

    // Start is called before the first frame update
    void Start()
    {
        //so when we initialize in a battle we need to tell the pads what midi key they're coordinated with
        //this is probably just gonna be passed into the battlemanager

        //actually no we can jsut load it from the playerprefs
        if (PlayerPrefs.GetInt(padID) != 0)
        {
            midiKeyNum = PlayerPrefs.GetInt(padID);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(binding) || MidiJack.MidiMaster.GetKeyDown(midiKeyNum))
        {
            //DebugManager.current.print("pad pressed " + midiKeyNum);
            if (indicator != null)
            {
                //print("hit");
                Indicator indic = indicator.GetComponent<Indicator>();

                if (indic.indicatorType == "Heady")
                {
                    BattleManager.current.processPadHit(false);
                }
                else
                {
                    BattleManager.current.processPadHit(true);
                }

                Destroy(indicator);
            }
            else
            {
                //print("miss");
                BattleManager.current.processPadHit(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            indicator = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "indicator")
        {
            //check if its a heady note 

            if (other.gameObject.GetComponent<Indicator>().indicatorType == "Heady")
            {
                //if its a heady note then this is considered success 
                BattleManager.current.processPadHit(true);
                Destroy(other.gameObject);


            }
            indicator = null;
        }
    }


}
