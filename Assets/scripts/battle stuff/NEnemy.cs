using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEnemy : MonoBehaviour
{
    public int maxhp, hp;
    public int attack, defense;
    private void Start()
    {
        hp = maxhp;
    }
}
