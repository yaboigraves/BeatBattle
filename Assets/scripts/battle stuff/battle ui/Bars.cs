using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bars : MonoBehaviour
{
    //TODO: this probably doesnt need to exist lol
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject barr = Instantiate(bar, transform);

            barr.transform.position = new Vector3(0, 100 + i, 0);

        }
    }


}
