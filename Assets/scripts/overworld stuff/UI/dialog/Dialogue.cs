using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Xml;

//TODO: this can get deleted i think
[System.Serializable]
public class Dialogue
{
    public string npcName;
    [TextArea(3, 10)]
    public string[] sentenceStrings;
    public Sentence[] sentences;

    //so the camera control system basically takes a sentence index and then a transform position to move the camera to

    //this is a hacky solution to serialize it in the editor 

    [System.Serializable]
    public struct sentenceTransformStruct
    {
        public int index;
        public CinemachineVirtualCamera camera;
    }
    public sentenceTransformStruct[] sentenceTransformArrays;
    public Dictionary<int, CinemachineVirtualCamera> sentenceCameras;


    public void initDialogue()
    {
        sentenceCameras = new Dictionary<int, CinemachineVirtualCamera>();
        foreach (sentenceTransformStruct sent in sentenceTransformArrays)
        {

            sentenceCameras.Add(sent.index, sent.camera);

        }

        sentences = new Sentence[sentenceStrings.Length];

        //parses the sentences as json objects?
        for (int i = 0; i < sentenceStrings.Length; i++)
        {
            sentences[i] = JsonUtility.FromJson<Sentence>(sentenceStrings[i]);
        }
    }
}
