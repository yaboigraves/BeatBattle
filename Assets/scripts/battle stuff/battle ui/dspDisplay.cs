using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dspDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = TrackTimeManager.debugDSPTIME.ToString();
    }
}
