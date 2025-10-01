using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePointScript : MonoBehaviour
{
    public GameObject Deur;

    private bool hostagesSaved;

    void Update()
    {
        hostagesSaved = Deur.GetComponentInChildren<CageDoorScript>().doorOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hostagesSaved)
        {
            //check if player has all hostages
            //if yes, go to next scene
        }
    }
}
