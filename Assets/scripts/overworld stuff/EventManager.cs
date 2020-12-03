using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //so basically this thing just handles dispatching events where they need to go
    //when a player pulls a lever or pushes a switch for example


    public static EventManager current;

    private void Awake()
    {
        current = this;
    }


}
