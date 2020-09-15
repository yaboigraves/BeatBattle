using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public static DebugManager current;
    public TextMeshProUGUI debugText;
    private void Awake()
    {
        current = this;
        debugText.text = "";
    }

    public void print(string debugLog)
    {
        debugText.text += "\n" + debugLog;
    }



}
