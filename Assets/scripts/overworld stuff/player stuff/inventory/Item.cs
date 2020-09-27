using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "BeatBattle/Item", order = 0)]
public class Item : GameItem
{
    //public string itemName;
    public string effectFunction;
    [TextArea]
    public string itemDescription;
    public virtual void Use()
    {
        typeof(ItemEffects).GetMethod(effectFunction).Invoke(null, null);
    }



}