using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 normalScale,pulsedScale = Vector3.one * 1.5f;
    public float decayRate = 0.3f;
    void Start()
    {
        normalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > normalScale.x){
            transform.localScale -= Vector3.one * (decayRate * Time.deltaTime);
        }
    }
    
    public void pulse(){
        transform.localScale = pulsedScale;
    }
}
