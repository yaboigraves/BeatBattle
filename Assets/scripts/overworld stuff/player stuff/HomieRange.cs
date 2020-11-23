using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomieRange : MonoBehaviour
{
    // Start is called before the first frame update
    public Homie homie;
    public float playerTolerance = 20;
    private void Start()
    {
        //homie = transform.parent.GetComponent<Homie>();
    }

    private void Update()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, homie.player.transform.position)) >= playerTolerance)
        {
            homie.TeleportToPlayer();
        }
    }

}
