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

    public void changeIndicatorColors(Color color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
    }

}
