using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePointScript : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //check if player has all hostages
            //if yes, go to next scene
        }
    }
}
