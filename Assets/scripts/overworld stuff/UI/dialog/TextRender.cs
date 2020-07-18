using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRender : MonoBehaviour
{
    public char[] chars;
    public Sprite[] letterSprites;
    public Dictionary<char, Sprite> font;
    public Queue<string> sentences;
    public Transform letterStartPos;
    RectTransform rectTransform;
    public GameObject letter;
    public GameObject dialogueContainer;

    public GameObject[] dialogueResponseContainers;

    // Start is called before the first frame update
    void Start()
    {
        dialogueContainer = transform.GetChild(0).gameObject;

        chars = "abcdefghijklmnopqrstuwxyz".ToCharArray();
        font = new Dictionary<char, Sprite>();
        for (int i = 0; i < chars.Length; i++)
        {
            font.Add(chars[i], letterSprites[i]);
        }
        rectTransform = GetComponent<RectTransform>();
    }


    public void clearText()
    {
        for (int i = 0; i < dialogueContainer.transform.childCount; i++)
        {
            Destroy(dialogueContainer.transform.GetChild(i).gameObject);
        }

        //also clear all the shit thats children of the response containers
        foreach (GameObject dialogueResponseContainer in dialogueResponseContainers)
        {
            for (int i = 0; i < dialogueResponseContainer.transform.childCount; i++)
            {
                Destroy(dialogueResponseContainer.transform.GetChild(i).gameObject);
            }
        }

    }


    public void renderText(Sentence sentence)
    {
        //TODO: add markup support

        clearText();

        for (int i = 0; i < sentence.text.Length; i++)
        {
            if (sentence.text[i] != ' ')
            {
                Vector3 offset = new Vector3(i * 25, 0, 0);
                Image letterSprite = Instantiate(letter, letterStartPos.position + offset, Quaternion.identity, dialogueContainer.transform).GetComponent<Image>();
                //lookup the letter sprite;

                //make sure this shit exists 
                if (font.ContainsKey(sentence.text[i]))
                {
                    letterSprite.sprite = font[sentence.text[i]];
                }
                else
                {
                    print("WARNING : " + sentence.text[i] + " doesn't exist in the letters dictionary");
                }
            }
        }

        //after the text is rendered check if there are dialog options

        for (int i = 0; i < sentence.responses.Length; i++)
        {

            for (int j = 0; j < sentence.responses[i].Length; j++)
            {
                if (sentence.responses[i][j] != ' ')
                {
                    Vector3 offset = new Vector3(j * 25, 0, 0);
                    Image letterSprite = Instantiate(letter, dialogueResponseContainers[i].transform.position + offset, Quaternion.identity, dialogueResponseContainers[i].transform).GetComponent<Image>();
                    //lookup the letter sprite;

                    //make sure this shit exists 
                    if (font.ContainsKey(sentence.responses[i][j]))
                    {
                        letterSprite.sprite = font[sentence.responses[i][j]];
                    }
                    else
                    {
                        print("WARNING : " + sentence.responses[i][j] + " doesn't exist in the letters dictionary");
                    }
                }
            }
        }
    }
}


