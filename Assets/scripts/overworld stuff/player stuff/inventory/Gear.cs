using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Item", menuName = "BeatBattle/Gear", order = 0)]
public class Gear : GameItem
{
    // public string itemName;
    public string gearFunction;
    [TextArea]
    public string gearDescription;
    public Mesh mesh;
    public int powerCost;

    // public virtual void ApplyEffect()
    // {
    //     typeof(GearEffects).GetMethod(gearFunction);
    //     typeof(GearEffects).GetMethod(gearFunction).Invoke(null, null);
    // }

}