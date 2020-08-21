using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: this can go too
public class LetterMotion : MonoBehaviour
{
    public float rotateTime = 1.5f;

    public float rotateBy = 8f;

    float rotationA, rotationB;

    RectTransform rectTransform;

    bool rotated;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        transform.Rotate(0, 0, Random.Range(-rotateBy, rotateBy));

        StartCoroutine(rotate());
    }

    // Update is called once per frame

    IEnumerator rotate()
    {

        if (rotated)
        {
            //transform.Rotate(0, 0, rotateBy);
            transform.rotation = Quaternion.Euler(0, 0, rotateBy);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -rotateBy);
        }
        rotated = !rotated;

        yield return new WaitForSeconds(Random.Range(0.8f, 1.6f));

        StartCoroutine(rotate());
    }

    //TODO: add some stuff for the markup language to do stuff with
}
