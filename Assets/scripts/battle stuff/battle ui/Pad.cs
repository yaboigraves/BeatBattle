using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public GameObject indicator;

    public KeyCode binding;
    public int midiKeyNum;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(binding) || MidiJack.MidiMaster.GetKeyDown(midiKeyNum))
        {
            //print("yeet");
            if (indicator != null)
            {
                //print("hit");
                BattleManager.current.processPadHit(true);
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
            indicator = null;
        }
    }


}
