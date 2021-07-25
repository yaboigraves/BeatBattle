using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform target;

    void LateUpdate()
    {
        transform.localPosition = target.localPosition;
    }
}
