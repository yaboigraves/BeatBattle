using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//so every single one of these functions can be called by items by their function name 
//all the functions for all consumable items just goes in here

public static class ItemEffects
{
    public static void test(bool enhancedEffect)
    {
        if (enhancedEffect)
        {
            Debug.Log("mega meme");
        }
        else
        {
            Debug.Log("meme");
        }

    }
}