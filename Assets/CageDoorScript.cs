using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoorScript : MonoBehaviour
{
    public Animator animator;
    private bool inDoorRange;

    void Update()
    {
        if (inDoorRange && Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("DoorOpeningAnimation", 0, 0f);
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
