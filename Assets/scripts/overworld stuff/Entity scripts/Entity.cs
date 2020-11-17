using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Entity : MonoBehaviour
{
    //anything that moves uses this
    //for now its just got lerp to rotation but alot of interact range stuff can be handled in here possibly too

    //this will probably need some refinement

    //TODO: make entities enable shadows cast on their sprite renderers by default
    public GameObject spriteContainer;
    public int facingDirection = 1;

    private void Start()
    {
        spriteContainer = transform.GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    public IEnumerator LerpToRotation(float endRotation, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startRotation = transform.rotation.eulerAngles.y;
        float lerpRotation = startRotation;

        float i = 0f;
        float rate = 1 / time;
        while (i <= 1)
        {
            i += Time.deltaTime * rate;

            lerpRotation = Mathf.Lerp(startRotation, endRotation, i);
            transform.rotation = Quaternion.Euler(0f, lerpRotation, 0f);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, endRotation, 0f);
    }

    public IEnumerator LerpToScale(float endScale, float time, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        //remember that this script is going to need to rotate not the whole object just the sprite renderer part
        float startScale = spriteContainer.transform.localScale.x;
        float lerpScale = startScale;


        float i = 0f;
        float rate = 1 / time;
        while (i <= 1)
        {
            i += Time.deltaTime * rate;

            lerpScale = Mathf.Lerp(startScale, endScale, i);
            spriteContainer.transform.localScale = new Vector3(lerpScale, 1, 1);
            yield return null;
        }

        spriteContainer.transform.localScale = new Vector3(endScale, 1, 1);

        facingDirection = (int)endScale;
    }
}