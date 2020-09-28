using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/*

    so ui icons are just icons on the ui that when moused over will provide some kind of tooltip info
    example for now is gear in the ui thats equipped should do 2 things
    1 it should flash when it's triggered 
    2 when moused over it should provide a blurb


    this can be used for 
    -buffs in battle (from consumables or other abilities)
    -gear equipped so you can check the effects

*/


public class UIIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //these variables wont change once instantiated so maybe not much rreason to have them?
    public GameItem item;
    public TextMeshProUGUI descriptionText, itemName;
    public GameObject descriptionPanel;
    public Image iconSprite;

    //maybe make this a 3d model later might be cool to throw the 3d modelled gear around
    //maybe do this urself or get dalton to maybe hop on it (he would eat that project uppp)

    private void Start()
    {


    }

    public void SetIconItem(GameItem item)
    {


        this.item = item;

        //check if its gear or an item
        Gear g = (Gear)item;

        if (g != null)
        {
            descriptionText.text = g.gearDescription;
        }


        //so we gotta set the text of this shit to whatever the items description is
        itemName.text = item.itemName;
        iconSprite.sprite = item.itemIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //toggle the blurb text 
        descriptionPanel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}
