using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public ParticleSystem footDust;

    //TODO: this should absorb some responsibilities from player, player can just maybe hold a reference to this
    // Start is called before the first frame update

    public float rotationSpeed = 1;
    Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
    Vector3 targetScale = Vector3.one;

    //rotation is either -1 or 1 depending on the direction
    public void flip(float rotation)
    {
        // StartCoroutine(LerpToRotation(rotation, 0.1f, 0.1f));
        //StartCoroutine(LerpToScale(rotation, 0.1f, 0.1f));
        CreateDust();
        targetRotation = Quaternion.Euler(0, 180, 0);
        targetScale = Vector3.left;
    }

    public void CreateDust()
    {
        footDust.Play();
    }

    private void Update()
    {
        // float step = rotationSpeed * Time.deltaTime;

        // transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, step);
    }



}
