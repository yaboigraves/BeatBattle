using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{


    //TODO: optimization stuff and uniform speeds 
    //so rather than instantiating all the indicators and letting them do their own thing with their own transforms
    //instantiate chunks of 4 bars with the bars and everything
    //then the amount of updates that need to run is cut down by a shit ton and each indicator doesnt really need to manage
    //its own position, rather the chunk does it all




    public static IndicatorManager current;

    void Awake()
    {
        current = this;
    }

    public void changeIndicatorColors(Color color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Indicator>().UpdateColor(color);
        }
    }

}
