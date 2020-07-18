using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{

    public static IndicatorManager current;

    void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update

    //inefficient just for testing 

    //during players turn red

    public void changeIndicatorColors(Color color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
    }

}
