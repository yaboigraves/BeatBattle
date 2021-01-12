using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
this should really probably be done with a particle system later for efficiency

*/

public class CombatText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float maxFadeTime = 2,fadeTime = 2;
  
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        fadeTime-= Time.deltaTime;
        text.CrossFadeColor(new Color(255,255,255,0),maxFadeTime,false,true);
        transform.Translate(new Vector3(0,Time.deltaTime * 40,0));
        if(fadeTime <= 0){
            Destroy(this.gameObject);
        }
    }
}
