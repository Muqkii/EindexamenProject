using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    private bool doorOpen;
    private bool notFollowing;

    void Update()
    {
        notFollowing = gameObject.GetComponentInParent<Following>().closeToPlayer;
        doorOpen = gameObject.GetComponentInParent<Following>().cageDoorOpen;
        Debug.Log(notFollowing);

        if(!notFollowing && doorOpen)
        {
            //animator.Play("walking", 0, 1f);
            animator.SetBool("InPlayerRange", false);
        }
        else
        {
            animator.SetBool("InPlayerRange", true);
            //animator.Play("idle", 0, 0f);
        }
    }
}
