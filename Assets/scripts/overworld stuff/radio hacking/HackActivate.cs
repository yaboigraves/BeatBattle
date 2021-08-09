using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackActivate : MonoBehaviour
{

    //so this is used when things have animations that need activating
    //platforms mostly

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        Debug.Assert(animator != null, "NO ANIMATOR");

        animator.SetTrigger("toggle");
    }

}
