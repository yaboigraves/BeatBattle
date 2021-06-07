using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnInfo : MonoBehaviour
{
    public TextMeshProUGUI sampleName, numericValue;

    public void SetInfo(string samplename, string numericvalue)
    {
        sampleName.text = samplename;
        numericValue.text = numericvalue;
    }

}
