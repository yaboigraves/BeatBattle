using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundDialogManager : MonoBehaviour
{
    List<DialogBubble> dialogBubbles;
    public static BackgroundDialogManager current;
    public GameObject dialogBubblePrefab;
    public Canvas dialogBubbleCanvas;

    private void Awake()
    {
        current = this;
        //for now we just start with 4, this should be good for now but if we need to we can make more
        CreateDialogBubbles(4);
    }

    public void CreateDialogBubbles(int numBubbles)
    {
        dialogBubbles = new List<DialogBubble>();

        for (int i = 0; i < numBubbles; i++)
        {
            DialogBubble bubble = Instantiate(dialogBubblePrefab).GetComponent<DialogBubble>();
            bubble.transform.SetParent(dialogBubbleCanvas.transform);
            bubble.gameObject.SetActive(false);
            dialogBubbles.Add(bubble);
        }
    }

    public void SpawnDialog(string text, Transform target, float time = 3)
    {

        //so we gotta look in the pool of available bubbles and find one thats available, if not we gotta spawn a new bubble
        foreach (DialogBubble bubble in dialogBubbles)
        {
            if (bubble.available)
            {
                bubble.ActivateDialogBubble(text, target, time);
                return;
            }
        }

        //create a new bubble
    }
}
