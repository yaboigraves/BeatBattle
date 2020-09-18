using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Entity : MonoBehaviour
{

    //anything that moves uses this
    //for now its just got lerp to rotation but alot of interact range stuff can be handled in here possibly too





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
}