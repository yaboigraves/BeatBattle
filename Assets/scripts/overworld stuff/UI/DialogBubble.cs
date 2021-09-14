using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBubble : MonoBehaviour
{
    public Vector2 offset = new Vector2(0, 200);
    public bool available = true;
    public TextMeshProUGUI text;
    Transform target;
    //so when we enable one of these we need to basically change its text and bind it to follow a transform
    private void Start()
    {

    }

    public void ActivateDialogBubble(string text, Transform target, float time)
    {
        available = false;
        this.gameObject.SetActive(true);
        this.text.text = text;
        this.target = target;

        StartCoroutine(bubbleLiftime(this, time));
    }

    IEnumerator bubbleLiftime(DialogBubble bubble, float time)
    {
        yield return new WaitForSeconds(time);
        bubble.available = true;
        bubble.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position) + (Vector3)offset;
        }
    }


}
