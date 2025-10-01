using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoorScript : MonoBehaviour
{
    public Animator animator;
    public bool doorOpen;

    private bool inDoorRange = true;

    void Update()
    {
        if (inDoorRange && Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("DoorOpeningAnimation", 0, 0f);
            doorOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inDoorRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inDoorRange = false;
        }
    }
}
