using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "BeatBattle/Item", order = 0)]
public class Item : ScriptableObject
{


    public string effectFunction;


    public virtual void Use()
    {
        typeof(ItemEffects).GetMethod(effectFunction).Invoke(null, null);
    }

}