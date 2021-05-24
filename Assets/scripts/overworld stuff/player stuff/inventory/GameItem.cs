using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameItem : ScriptableObject
{
    public string itemName = "Item";
    public int cost = 1;
    public string description = "This is the default item description. Cool.";

    public bool unique;

    public Sprite itemIcon;

}