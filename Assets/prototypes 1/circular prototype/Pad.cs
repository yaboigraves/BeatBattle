using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public GameObject indicator;
    public string padID;
    public KeyCode binding;
    public int midiKeyNum;
    //PulseEffect pulse;

    //public Vector3 pulseSize;

    Pulse pulse;
    // Start is called before the first frame update
    void Start()
    {
        pulse = GetComponent<Pulse>();
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

                //pulse.TriggerPulse(pulseSize);

                if (indic.indicatorType == "Heady")
                {

                    print("you hit on a green note");
                    Destroy(indicator);

                    //in order not to trigger an ontriggerexit we need to set the type not to heady anymore
                    indic.indicatorType = "";
                    BattleManager.current.processPadHit(false);
                }
                else
                {
                    BattleManager.current.processPadHit(true);
                    pulse.pulse();
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
            indicator.GetComponent<SpriteRenderer>().color = Color.green;
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
                //BattleManager.current.processPadHit(true);
                //Destroy(other.gameObject);
            }

            //indicator.GetComponent<SpriteRenderer>().color = Color.red;
            indicator = null;
        }
    }
}
