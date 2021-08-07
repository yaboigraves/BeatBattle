using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodeable : MonoBehaviour
{
    ParticleSystem system;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();
    }
    public void Explode()
    {
        //deactivate the object
        transform.parent.GetComponent<MeshRenderer>().enabled = false;
        transform.parent.GetComponent<Collider>().enabled = false;
        system.Play();
        StartCoroutine(particleComplete());


    }

    IEnumerator particleComplete()
    {
        while (system.isPlaying)
        {
            yield return null;
        }

        Destroy(this.transform.parent.gameObject);
    }

}
