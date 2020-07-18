using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    [TextArea(3, 10)]
    public string[] sentenceStrings;
    public Sentence[] sentences;

    public void initDialogue()
    {
        sentences = new Sentence[sentenceStrings.Length];


        //parses the sentences as json objects?
        for (int i = 0; i < sentenceStrings.Length; i++)
        {
            sentences[i] = JsonUtility.FromJson<Sentence>(sentenceStrings[i]);
        }
    }
}
