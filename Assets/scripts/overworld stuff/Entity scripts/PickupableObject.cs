using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IPickupable
{
    public LeanTweenType easeType;
    void Start()
    {
        LeanTween.moveY(gameObject, 1f, 1f).setLoopPingPong().setEase(easeType);
    }


    public void Pickup(PlayerInventory inventory)
    {
        inventory.getCoin();
        Destroy(this.gameObject);
    }
}
