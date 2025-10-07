using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(1);
            Debug.Log("change");
        }
    }
}
