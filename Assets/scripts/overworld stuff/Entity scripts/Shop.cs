using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn.Unity;

public class Shop : MonoBehaviour
{
    //so npcs can have shop components (rather than needing to extned this down)
    //the yarn script will call a function in the npc that tries to open the shop
    //the shop contains items and then sends these to the ui manager to open that particular panel
    //shop contains an items tab, a tracks tab, and a gear tab

    //issue is, if there are multiple shops in the scene we need to differentiate between who needs to open
    //options
    //-pass an argument of the name of the shop we want to open (kinda ugly but not too bad)

    //going to just pass the shopname as an argument for now, shouldnt scale too bad as their will only ever be maybe 3
    //shops max per scene 
    public string shopName;
    //when we load into a scene we need to find the dialogue runner 
    public DialogueRunner dialogueRunner;

    public List<GameItem> inventory;

    private void Start()
    {
        dialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler("OpenShop", OpenShop);

        print("lol");
    }

    public void OpenShop(string[] parameters)
    {
        print("opening shop");
        if (parameters[0] == shopName)
        {

            //tell the player that they're in the shop now
            GameManager.current.player.inShop = true;
            //actually open the shop

            //tell the uimanager to open a store with the given inventory
            UIManager.current.OpenShop(this);
        }
    }
}