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

    public bool sentenceRendered;




    //coroutines 
    Coroutine letterRenderCoroutine;



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

    //need a function to basically just render the rest of the letters

    public void renderRestOfLetters(Sentence sentence, int i)
    {

        //TODO: refactor this god forsaken script
        print("Rendering rest of letters");
        //StopCoroutine(letterRenderCoroutine);
        StopAllCoroutines();
        //render the rest of the fuckin sentencel lol
        for (int j = i; j < sentence.text.Length; j++)
        {
            if (sentence.text[j] != ' ')
            {
                Vector3 offset = new Vector3(j * 25, 0, 0);
                Image letterSprite = Instantiate(letter, letterStartPos.position + offset, Quaternion.identity, dialogueContainer.transform).GetComponent<Image>();

                if (font.ContainsKey(sentence.text[j]))
                {
                    letterSprite.sprite = font[sentence.text[j]];
                }
                else
                {
                    print("WARNING : " + sentence.text[j] + " doesn't exist in the letters dictionary");
                }
            }

        }

        for (int x = 0; x < sentence.responses.Length; x++)
        {
            for (int j = 0; j < sentence.responses[x].Length; j++)
            {
                if (sentence.responses[x][j] != ' ')
                {
                    Vector3 offset = new Vector3(j * 25, 0, 0);
                    Image letterSprite = Instantiate(letter, dialogueResponseContainers[x].transform.position + offset, Quaternion.identity, dialogueResponseContainers[x].transform).GetComponent<Image>();
                    //lookup the letter sprite;

                    //make sure this shit exists 
                    if (font.ContainsKey(sentence.responses[x][j]))
                    {
                        letterSprite.sprite = font[sentence.responses[x][j]];
                    }
                    else
                    {
                        print("WARNING : " + sentence.responses[x][j] + " doesn't exist in the letters dictionary");
                    }
                }
            }
        }

        sentenceRendered = true;
    }

    public int letterIndex;
    IEnumerator renderLetters(Sentence sentence, int i)
    {
        letterIndex = i;

        if (i < sentence.text.Length && sentence.text[i] != ' ')
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

        yield return new WaitForSeconds(0.1f);

        if (i < sentence.text.Length)
        {
            StartCoroutine(renderLetters(sentence, i + 1));
        }
        else
        {
            for (int x = 0; x < sentence.responses.Length; x++)
            {
                for (int j = 0; j < sentence.responses[x].Length; j++)
                {
                    if (sentence.responses[x][j] != ' ')
                    {
                        Vector3 offset = new Vector3(j * 25, 0, 0);
                        Image letterSprite = Instantiate(letter, dialogueResponseContainers[x].transform.position + offset, Quaternion.identity, dialogueResponseContainers[x].transform).GetComponent<Image>();
                        //lookup the letter sprite;

                        //make sure this shit exists 
                        if (font.ContainsKey(sentence.responses[x][j]))
                        {
                            letterSprite.sprite = font[sentence.responses[x][j]];
                        }
                        else
                        {
                            print("WARNING : " + sentence.responses[x][j] + " doesn't exist in the letters dictionary");
                        }
                    }
                }
            }

            sentenceRendered = true;
        }
    }

    public void renderText(Sentence sentence)
    {
        //TODO: add markup support
        clearText();
        sentenceRendered = false;
        letterRenderCoroutine = StartCoroutine(renderLetters(sentence, 0));
    }
}


